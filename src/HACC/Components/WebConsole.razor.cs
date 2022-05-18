using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.Model;
using HACC.Applications;
using HACC.Extensions;
using HACC.Models;
using HACC.Models.Drivers;
using HACC.Models.Enums;
using HACC.Models.Structs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Drawing;
using System.Globalization;
using Terminal.Gui;

namespace HACC.Components;

public partial class WebConsole : ComponentBase
{
    private static readonly IJSRuntime JsInterop = HaccExtensions.GetService<IJSRuntime>();
    private static readonly ILogger Logger = HaccExtensions.CreateLogger<WebConsole>();

    protected readonly string Id = Guid.NewGuid().ToString();

    protected BECanvasComponent? _becanvas;

    protected Canvas2DContext? _canvas2DContext;

    /// <summary>
    ///     Null until after render
    /// </summary>
    private ElementReference? _divCanvas;

    private Queue<InputResult> _inputResultQueue = new();
    private int _screenHeight = 480;
    private int _screenWidth = 640;
    private bool _firstRender;

    [Parameter] public long Height { get; set; }

    [Parameter] public long Width { get; set; }

    public WebApplication? WebApplication { get; private set; }

    public WebConsoleDriver? WebConsoleDriver { get; private set; }

    public WebMainLoopDriver? WebMainLoopDriver { get; private set; }

    [Parameter] public EventCallback OnLoaded { get; set; }

    public bool CanvasInitialized => _canvas2DContext != null;

    public event Action<InputResult>? ReadConsoleInput;
    public event Action? RunIterationNeeded;

    protected override Task OnInitializedAsync()
    {
        this.WebConsoleDriver = new WebConsoleDriver(
            webClipboard: HaccExtensions.WebClipboard,
            webConsole: this);
        this.WebMainLoopDriver = new WebMainLoopDriver(webConsole: this);
        this.WebApplication = new WebApplication(
            webConsoleDriver: this.WebConsoleDriver,
            webMainLoopDriver: this.WebMainLoopDriver,
            webConsole: this);

        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Logger.LogDebug(message: "OnAfterRenderAsync");

            _firstRender = firstRender;

            _canvas2DContext = await _becanvas.CreateCanvas2DAsync();

            var thisObject = DotNetObjectReference.Create(value: this);
            await JsInterop!.InvokeVoidAsync(identifier: "initConsole",
                thisObject);
            // this will make sure that the viewport is correctly initialized
            await JsInterop!.InvokeAsync<object>(identifier: "consoleWindowResize",
                thisObject);
            await JsInterop!.InvokeAsync<object>(identifier: "consoleWindowFocus",
                thisObject);
            await JsInterop!.InvokeAsync<object>(identifier: "consoleWindowBeforeUnload",
                thisObject);

            await this.OnLoaded.InvokeAsync();
            this.OnReadConsoleInput();

            Logger.LogDebug(message: "OnAfterRenderAsync: end");
        }

        await base.OnAfterRenderAsync(firstRender: firstRender);
    }

    public async Task<object?> DrawBufferToPng()
    {
        if (!this.CanvasInitialized) return null;
        return await JsInterop!.InvokeAsync<object>(identifier: "canvasToPng");
    }

    public async Task<int?> MeasureText(string text,
        int fontSpacePixels)
    {
        if (!this.CanvasInitialized) return null;
        var result = await Task.Run(() => TextFormatter.GetTextWidth(text));

        return result * fontSpacePixels;
    }

    private async Task RedrawCanvas()
    {
        if (!this.CanvasInitialized) return;

        Logger.LogDebug(message: "InitializeNewCanvasFrame");

        // TODO: actually clear the canvas
        await this._canvas2DContext!.SetFillStyleAsync(value: "blue");
        await this._canvas2DContext.ClearRectAsync(
            x: 0,
            y: 0,
            width: this.WebConsoleDriver!.WindowWidthPixels,
            height: this.WebConsoleDriver.WindowHeightPixels);
        await this._canvas2DContext.FillRectAsync(
            x: 0,
            y: 0,
            width: this.WebConsoleDriver.WindowWidthPixels,
            height: this.WebConsoleDriver.WindowHeightPixels);


        //await this._canvas2DContextStdErr.SetFillStyleAsync(value: "blue");
        //await this._canvas2DContextStdErr.ClearRectAsync(
        //    x: 0,
        //    y: 0,
        //    width: this._webConsoleDriver.WindowWidthPixels,
        //    height: this._webConsoleDriver.WindowHeightPixels);
        //await this._canvas2DContextStdErr.FillRectAsync(
        //    x: 0,
        //    y: 0,
        //    width: this._webConsoleDriver.WindowWidthPixels,
        //    height: this._webConsoleDriver.WindowHeightPixels);
        Logger.LogDebug(message: "InitializeNewCanvasFrame: end");
    }

    public async Task DrawDirtySegmentToCanvas(
        List<DirtySegment> segments,
        TerminalSettings terminalSettings)
    {
        if (!this.CanvasInitialized) return;
        if (segments.Count == 0) return;

        Logger.LogDebug(message: "DrawBufferToFrame");
        var lastRow = segments[index: 0].Row;
        var textWidthEm = segments[index: 0].Column;

        try
        {
            foreach (var segment in segments)
            {
                if (segment.Row != lastRow)
                {
                    lastRow = segment.Row;
                    textWidthEm = segment.Column;
                }
                textWidthEm = Math.Max(textWidthEm, segment.Column * terminalSettings.FontSpacePixels);

                var letterWidthPx = terminalSettings.FontSizePixels;
                await this._canvas2DContext!.SetFontAsync(
                    value: $"{letterWidthPx}px " +
                           $"{terminalSettings.FontType}");
                await this._canvas2DContext.SetTextBaselineAsync(value: TextBaseline.Top);
                var measuredText = await this.MeasureText(text: segment.Text,
                    fontSpacePixels: terminalSettings.FontSpacePixels);
                await this._canvas2DContext!.SetFillStyleAsync(
                    value: $"{segment.BackgroundColor}");
                await this._canvas2DContext.FillRectAsync(
                    x: textWidthEm,
                    y: segment.Row * letterWidthPx,
                    width: (double) measuredText,
                    height: letterWidthPx);
                await this._canvas2DContext!.SetStrokeStyleAsync(
                    value: $"{segment.ForegroundColor}");
                await this._canvas2DContext.StrokeTextAsync(text: segment.Text,
                    x: textWidthEm,
                    y: segment.Row * letterWidthPx,
                    maxWidth: (double) measuredText);

                textWidthEm += (int) measuredText;
            }
            if (_firstRender)
            {


                //this.StateHasChanged();
                //await this._canvas2DContext!.FillRectAsync(0, 0, 640, 480);
                //_ = this._canvas2DContext!.ClearRectAsync(0, 0, 640, 480);
                //_ = this._canvas2DContext.BeginPathAsync();
                //this.OnReadConsoleInput();
                _firstRender = false;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
        Logger.LogDebug(message: "DrawBufferToFrame: end");
    }

    /// <summary>
    ///     Invoke the javascript beep function (copied from JavasScript/beep.js)
    /// </summary>
    /// <param name="duration">duration of the tone in milliseconds. Default is 500</param>
    /// <param name="frequency">frequency of the tone in hertz. default is 440</param>
    /// <param name="volume">volume of the tone. Default is 1, off is 0.</param>
    /// <param name="type">type of tone. Possible values are sine, square, sawtooth, triangle, and custom. Default is sine.</param>
    public async Task Beep(float? duration, float? frequency, float? volume, string? type)
    {
        if (duration is not null && frequency is not null && volume is not null && type is not null)
            // ReSharper disable HeapView.ObjectAllocation
            await JsInterop.InvokeAsync<Task>(
                identifier: "beep",
                duration.Value.ToString(provider: CultureInfo.InvariantCulture),
                frequency.Value.ToString(provider: CultureInfo.InvariantCulture),
                volume.Value.ToString(provider: CultureInfo.InvariantCulture),
                type);
        if (duration is not null && frequency is not null && volume is not null && type is null)
            await JsInterop.InvokeAsync<Task>(
                identifier: "beep",
                duration.Value.ToString(provider: CultureInfo.InvariantCulture),
                frequency.Value.ToString(provider: CultureInfo.CurrentCulture),
                volume.Value.ToString(provider: CultureInfo.InvariantCulture));
        if (duration is not null && frequency is not null && volume is null && type is null)
            await JsInterop.InvokeAsync<Task>(
                identifier: "beep",
                duration.Value.ToString(provider: CultureInfo.InvariantCulture),
                frequency.Value.ToString(provider: CultureInfo.InvariantCulture));
        if (duration is not null && frequency is null && volume is null && type is null)
            await JsInterop.InvokeAsync<Task>(
                identifier: "beep",
                duration.Value.ToString(provider: CultureInfo.CurrentCulture));
        if (duration is null && frequency is null && volume is null && type is null)
            await JsInterop.InvokeVoidAsync(
                identifier: "beep");
        // ReSharper restore HeapView.ObjectAllocation
    }

    private void OnReadConsoleInput()
    {
        if (this.ReadConsoleInput == null)
            return;

        while (_inputResultQueue.Count > 0)
        {
            this.ReadConsoleInput?.Invoke(obj: _inputResultQueue.Dequeue());
            this.RunIterationNeeded?.Invoke();
        }
    }

    [JSInvokable]
    public ValueTask OnCanvasMouse(MouseEventArgs obj)
    {
        if (!this.GetMouseEvent(obj, out WebMouseEvent me))
            return ValueTask.CompletedTask;
        // of relevance: ActiveConsole
        var inputResult = new InputResult
        {
            EventType = EventType.Mouse,
            MouseEvent = me
        };
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        return ValueTask.FromCanceled(new CancellationToken(true));
    }

    private bool GetMouseEvent(MouseEventArgs me, out WebMouseEvent mouseEvent)
    {
        mouseEvent = new WebMouseEvent();
        MouseButtonState buttonState;
        switch (me.Type)
        {
            case "mousedown":
                buttonState = GetButtonPressed();
                break;
            case "mouseup":
                buttonState = GetButtonReleased();
                break;
            case "mousemove":
                buttonState = MouseButtonState.ReportMousePosition;
                break;
            default:
                return false;
        }
        mouseEvent.ButtonState = buttonState;
        var terminalSettings = this.WebConsoleDriver!.TerminalSettings;
        if (me.ClientX > terminalSettings.WindowWidthPixels
            || me.ClientY > terminalSettings.WindowHeightPixels)
            return false;
        mouseEvent.Position.X = (int) me.ClientX / terminalSettings.FontSpacePixels;
        mouseEvent.Position.Y = (int) me.ClientY / terminalSettings.FontSizePixels;
        return true;

        MouseButtonState GetButtonPressed()
        {
            return me.Button switch
            {
                0 => MouseButtonState.Button1Pressed,
                1 => MouseButtonState.Button2Pressed,
                2 => MouseButtonState.Button3Pressed,
                3 => MouseButtonState.Button4Pressed,
            };
        }

        MouseButtonState GetButtonReleased()
        {
            return me.Button switch
            {
                0 => MouseButtonState.Button1Released,
                1 => MouseButtonState.Button2Released,
                2 => MouseButtonState.Button3Released,
                3 => MouseButtonState.Button4Released,
            };
        }
    }

    [JSInvokable]
    public ValueTask OnCanvasWheel(WheelEventArgs obj)
    {
        if (!this.GetWheelEvent(obj, out WebMouseEvent me))
            return ValueTask.CompletedTask;
        var inputResult = new InputResult
        {
            EventType = EventType.Mouse,
            MouseEvent = me
        };
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        return ValueTask.FromCanceled(new CancellationToken(true));
    }

    private bool GetWheelEvent(WheelEventArgs we, out WebMouseEvent mouseEvent)
    {
        mouseEvent = new WebMouseEvent();
        MouseButtonState buttonState;
        switch (we.Type)
        {
            case "mousewheel":
                if (we.DeltaX != 0)
                    buttonState = GetWheelDeltaX();
                else if (we.DeltaY != 0)
                    buttonState = GetWheelDeltaY();
                else
                    return false;
                break;
            default:
                return false;
        }
        mouseEvent.ButtonState = buttonState;
        var terminalSettings = this.WebConsoleDriver!.TerminalSettings;
        if (we.ClientX > terminalSettings.WindowWidthPixels
            || we.ClientY > terminalSettings.WindowHeightPixels)
            return false;
        mouseEvent.Position.X = (int)we.ClientX / terminalSettings.FontSpacePixels;
        mouseEvent.Position.Y = (int)we.ClientY / terminalSettings.FontSizePixels;
        return true; ;

        MouseButtonState GetWheelDeltaX()
        {
            return we.DeltaX switch
            {
                > 0 => MouseButtonState.ButtonWheeledRight,
                _ => MouseButtonState.ButtonWheeledLeft,
            };
        }

        MouseButtonState GetWheelDeltaY()
        {
            return we.DeltaY switch
            {
                > 0 => MouseButtonState.ButtonWheeledDown,
                _ => MouseButtonState.ButtonWheeledUp,
            };
        }
    }

    [JSInvokable]
    public ValueTask OnCanvasKey(KeyboardEventArgs obj)
    {
        var inputResult = new InputResult
        {
            EventType = EventType.Key,
            KeyEvent = new WebKeyEvent
            {
                KeyDown = true,
            },
        };
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnCanvasKeyDown(KeyboardEventArgs obj)
    {
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnCanvasKeyUp(KeyboardEventArgs obj)
    {
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnCanvasKeyPress(KeyboardEventArgs arg)
    {
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnResize(int screenWidth, int screenHeight)
    {
        if (this._canvas2DContext == null) return ValueTask.CompletedTask;
        var terminalSettings = this.WebConsoleDriver!.TerminalSettings;
        this._screenWidth = screenWidth - terminalSettings.FontSpacePixels * 3;
        this._screenHeight = screenHeight - terminalSettings.FontSizePixels / 2;
        var inputResult = new InputResult
        {
            EventType = EventType.Resize,
            ResizeEvent = new ResizeEvent
            {
                Size = new System.Drawing.Size(width: this._screenWidth,
                    height: this._screenHeight),
            },
        };
        this._inputResultQueue.Enqueue(item: inputResult);
        this.OnReadConsoleInput();
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnFocus()
    {
        if (this._canvas2DContext == null) return ValueTask.CompletedTask;
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnBeforeUnload()
    {
        if (this._canvas2DContext == null) return ValueTask.CompletedTask;
        return ValueTask.CompletedTask;
    }
}