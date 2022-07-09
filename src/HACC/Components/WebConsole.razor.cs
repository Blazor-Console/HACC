using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
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
using System.Globalization;
using Terminal.Gui;
using Point = System.Drawing.Point;

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

    private Queue<WebInputResult> _inputResultQueue = new();
    private int _screenHeight = 480;
    private int _screenWidth = 640;
    internal bool _firstRender = true;

    [Parameter] public long Height { get; set; }

    [Parameter] public long Width { get; set; }

    public WebApplication? WebApplication { get; private set; }

    public WebConsoleDriver? WebConsoleDriver { get; private set; }

    public WebMainLoopDriver? WebMainLoopDriver { get; private set; }

    [Parameter] public EventCallback OnLoaded { get; set; }

    public bool CanvasInitialized => this._canvas2DContext != null;

    public event Func<WebInputResult, Task>? ReadConsoleInput;
    public event Func<Task>? RunIterationNeeded;

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
            _firstRender = false;
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
        Logger.LogDebug(message: "DrawBufferToFrame: end");
    }

    private string? curColor;

    public async Task DrawCursorToCanvas(TerminalSettings terminalSettings)
    {
        if (this._canvas2DContext == null) return;
        var left = terminalSettings.CursorPosition.X;
        var top = terminalSettings.CursorPosition.Y;
        if (left == -1 || top == -1)
            return;

        Logger.LogDebug(message: "DrawCursorToCanvas");
        var textWidthEm = left * terminalSettings.FontSpacePixels;
        var letterWidthPx = terminalSettings.FontSizePixels * terminalSettings.CursorHeight / terminalSettings.CursorHeight;
        await this._canvas2DContext!.SetFontAsync(
                value: $"{letterWidthPx}px " +
                       $"{terminalSettings.FontType}");
        await this._canvas2DContext.SetTextBaselineAsync(value: TextBaseline.Top);
        var measuredText = 1 * terminalSettings.CursorSize / terminalSettings.CursorSize;
        if (string.IsNullOrEmpty(this.curColor))
        {
            this.curColor = terminalSettings.CursorColor.ToString();
        }
        else if (this.curColor == terminalSettings.CursorColor.ToString())
        {
            this.curColor = terminalSettings.CursorAlternateColor.ToString();
        }
        else
        {
            this.curColor = terminalSettings.CursorColor.ToString();
        }
        await this._canvas2DContext!.SetFillStyleAsync(
                value: curColor);
        await this._canvas2DContext.FillRectAsync(
            x: textWidthEm,
            y: top * letterWidthPx,
            width: measuredText,
            height: letterWidthPx);
        await this._canvas2DContext!.SetStrokeStyleAsync(
            value: "green");
        await this._canvas2DContext.StrokeTextAsync(text: "",
            x: textWidthEm,
            y: top * letterWidthPx,
            maxWidth: measuredText);

        Logger.LogDebug(message: "DrawCursorToCanvas: end");
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

    private bool _isReadConsoleInput;

    public virtual void OnReadConsoleInput()
    {
        if (this.ReadConsoleInput == null)
            return;

        this._isReadConsoleInput = true;
        while (this._inputResultQueue.Count > 0)
        {
            this.ReadConsoleInput?.Invoke(arg: this._inputResultQueue.Dequeue());
            this.OnRunIterationNeeded();
        }
        this._isReadConsoleInput = false;
    }

    private bool _isRunIterationNeeded;

    public virtual void OnRunIterationNeeded()
    {
        this._isRunIterationNeeded = true;
        this.RunIterationNeeded?.Invoke();
        this._isRunIterationNeeded = false;
    }

    public virtual void OnTimeout()
    {
        if (!this._isReadConsoleInput && !this._isRunIterationNeeded)
            this.OnRunIterationNeeded();
    }

    public virtual void OnWakeup()
    {
        if (!this._isReadConsoleInput && !this._isRunIterationNeeded)
            this.OnRunIterationNeeded();
    }

    private WebMouseButtonState? _lastButtonPressed;
    private WebMouseEvent _lastMouseEvent;
    private Point? point;
    private bool processButtonClick;

    [JSInvokable]
    public ValueTask OnCanvasMouse(MouseEventArgs obj)
    {
        if (!this.GetMouseEvent(obj, out WebMouseEvent me))
            return ValueTask.CompletedTask;
        // of relevance: ActiveConsole
        var inputResult = new WebInputResult
        {
            EventType = WebEventType.Mouse,
            MouseEvent = me
        };
        _lastMouseEvent = me;
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        if (obj.Type == "mousedown")
        {
            Task.Run(async () => await ProcessContinuousButtonPressedAsync());
        }
        else if (obj.Type == "mouseup" && this.processButtonClick)
        {
            this.ProcessClickEvent(me);
        }
        return ValueTask.CompletedTask;
    }

    private async Task ProcessContinuousButtonPressedAsync()
    {
        while (this._lastButtonPressed != null)
        {
            await Task.Delay(100);
            var view = Application.WantContinuousButtonPressedView;
            if (view == null)
            {
                break;
            }
            if (this._lastButtonPressed != null)
            {
                var inputResult = new WebInputResult
                {
                    EventType = WebEventType.Mouse,
                    MouseEvent = _lastMouseEvent
                };
                this._inputResultQueue.Enqueue(inputResult);
                this.OnReadConsoleInput();
            }
            else
            {
                break;
            }
        }
    }

    private void ProcessClickEvent(WebMouseEvent me)
    {
        var mouseEvent = new WebMouseEvent();
        WebMouseButtonState buttonState;
        switch (me.ButtonState)
        {
            case WebMouseButtonState.Button1Released:
                buttonState = WebMouseButtonState.Button1Clicked;
                break;
            case WebMouseButtonState.Button2Released:
                buttonState = WebMouseButtonState.Button2Clicked;
                break;
            case WebMouseButtonState.Button3Released:
                buttonState = WebMouseButtonState.Button3Clicked;
                break;
            case WebMouseButtonState.Button4Released:
                buttonState = WebMouseButtonState.Button4Clicked;
                break;
            default:
                return;
        }
        mouseEvent.ButtonState = buttonState;
        mouseEvent.Position = me.Position;
        var inputResult = new WebInputResult
        {
            EventType = WebEventType.Mouse,
            MouseEvent = mouseEvent
        };
        this.processButtonClick = false;
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
    }

    private bool GetMouseEvent(MouseEventArgs me, out WebMouseEvent mouseEvent)
    {
        mouseEvent = new WebMouseEvent();
        var terminalSettings = this.WebConsoleDriver!.TerminalSettings;
        if (me.OffsetX > terminalSettings.WindowWidthPixels
            || me.OffsetY > terminalSettings.WindowHeightPixels)
            return false;
        mouseEvent.Position.X = (int) me.OffsetX / terminalSettings.FontSpacePixels;
        mouseEvent.Position.Y = (int) me.OffsetY / terminalSettings.FontSizePixels;
        WebMouseButtonState buttonState;
        switch (me.Type)
        {
            case "mousedown":
                this._lastButtonPressed = buttonState = GetButtonPressed();
                if (this.point == null)
                {
                    this.point = mouseEvent.Position;
                }
                break;
            case "mouseup":
                buttonState = GetButtonReleased(mouseEvent);
                this._lastButtonPressed = null;
                this.point = null;
                break;
            case "mousemove":
                buttonState = WebMouseButtonState.ReportMousePosition;
                if (this._lastButtonPressed != null)
                    buttonState |= (WebMouseButtonState) this._lastButtonPressed;
                break;
            default:
                return false;
        }
        mouseEvent.ButtonState = buttonState;
        return true;

        WebMouseButtonState GetButtonPressed()
        {
            return me.Button switch
            {
                0 => WebMouseButtonState.Button1Pressed,
                1 => WebMouseButtonState.Button2Pressed,
                2 => WebMouseButtonState.Button3Pressed,
                _ => WebMouseButtonState.Button4Pressed,
            };
        }

        WebMouseButtonState GetButtonReleased(WebMouseEvent mouseEvent)
        {
            if (me.Detail == 1)
            {
                if (this.point != null && (((Point) this.point).X == mouseEvent.Position.X
                    && ((Point) this.point).Y == mouseEvent.Position.Y))
                {
                    this.processButtonClick = true;
                }
                else
                {
                    this.point = null;
                }

                return me.Button switch
                {
                    0 => WebMouseButtonState.Button1Released,
                    1 => WebMouseButtonState.Button2Released,
                    2 => WebMouseButtonState.Button3Released,
                    _ => WebMouseButtonState.Button4Released,
                };
            }
            else if (me.Detail == 2)
            {
                return me.Button switch
                {
                    0 => WebMouseButtonState.Button1DoubleClicked,
                    1 => WebMouseButtonState.Button2DoubleClicked,
                    2 => WebMouseButtonState.Button3DoubleClicked,
                    _ => WebMouseButtonState.Button4DoubleClicked,
                };
            }
            else
            {
                return me.Button switch
                {
                    0 => WebMouseButtonState.Button1TripleClicked,
                    1 => WebMouseButtonState.Button2TrippleClicked,
                    2 => WebMouseButtonState.Button3TripleClicked,
                    _ => WebMouseButtonState.Button4TripleClicked,
                };
            }
        }
    }

    [JSInvokable]
    public ValueTask OnCanvasWheel(WheelEventArgs obj)
    {
        if (!this.GetWheelEvent(obj, out WebMouseEvent me))
            return ValueTask.CompletedTask;
        var inputResult = new WebInputResult
        {
            EventType = WebEventType.Mouse,
            MouseEvent = me
        };
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        return ValueTask.CompletedTask;
    }

    private bool GetWheelEvent(WheelEventArgs we, out WebMouseEvent mouseEvent)
    {
        mouseEvent = new WebMouseEvent();
        WebMouseButtonState buttonState;
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
        if (we.OffsetX > terminalSettings.WindowWidthPixels
            || we.OffsetY > terminalSettings.WindowHeightPixels)
            return false;
        mouseEvent.Position.X = (int) we.OffsetX / terminalSettings.FontSpacePixels;
        mouseEvent.Position.Y = (int) we.OffsetY / terminalSettings.FontSizePixels;
        return true; ;

        WebMouseButtonState GetWheelDeltaX()
        {
            return we.DeltaX switch
            {
                > 0 => WebMouseButtonState.ButtonWheeledRight,
                _ => WebMouseButtonState.ButtonWheeledLeft,
            };
        }

        WebMouseButtonState GetWheelDeltaY()
        {
            return we.DeltaY switch
            {
                > 0 => WebMouseButtonState.ButtonWheeledDown,
                _ => WebMouseButtonState.ButtonWheeledUp,
            };
        }
    }

    [JSInvokable]
    public ValueTask OnCanvasKey(KeyboardEventArgs obj)
    {
        var consoleKey = MapKeyboardEventArgsCode(obj.Code);
        var keyChar = MapKeyboardEventArgsKey(obj.Key, ref consoleKey);
        var inputResult = new WebInputResult
        {
            EventType = WebEventType.Key,
            KeyEvent = new WebKeyEvent
            {
                KeyDown = GetKeyType(obj.Type),
                ConsoleKeyInfo = new ConsoleKeyInfo(keyChar, consoleKey,
                    obj.ShiftKey, obj.AltKey, obj.CtrlKey)
            }
        };
        this._inputResultQueue.Enqueue(inputResult);
        this.OnReadConsoleInput();
        return ValueTask.CompletedTask;
    }

    private bool GetKeyType(string type)
    {
        return type switch
        {
            "keydown" => true,
            _ => false
        };
    }

    private char MapKeyboardEventArgsKey(string key, ref ConsoleKey consoleKey)
    {
        switch (key)
        {
            case var k when k.Length == 1:
                return key[0];
            case "Home":
                consoleKey = ConsoleKey.Home;
                return '\0';
            case "End":
                consoleKey = ConsoleKey.End;
                return '\0';
            case "PageDown":
                consoleKey = ConsoleKey.PageDown;
                return '\0';
            case "PageUp":
                consoleKey = ConsoleKey.PageUp;
                return '\0';
            case "ArrowLeft":
                consoleKey = ConsoleKey.LeftArrow;
                return '\0';
            case "ArrowRight":
                consoleKey = ConsoleKey.RightArrow;
                return '\0';
            case "ArrowUp":
                consoleKey = ConsoleKey.UpArrow;
                return '\0';
            case "ArrowDown":
                consoleKey = ConsoleKey.DownArrow;
                return '\0';
            case "Delete":
                consoleKey = ConsoleKey.Delete;
                return '\0';
            case "Insert":
                consoleKey = ConsoleKey.Insert;
                return '\0';
            default:
                return '\0';
        }
    }

    private ConsoleKey MapKeyboardEventArgsCode(string code)
    {
        switch (code)
        {
            case var c when c.StartsWith("Key"):
                var rest = code.Substring(3, code.Length - 3);
                Enum.TryParse(rest, out ConsoleKey consoleKey);
                return consoleKey;
            case var c when c.StartsWith("Digit"):
                rest = code.Substring(5, code.Length - 5);
                Enum.TryParse($"D{rest}", out consoleKey);
                return consoleKey;
            case "Minus":
            case "Equal":
            case "BracketLeft":
            case "Quote":
            case "IntlBackslash":
            case "Slash":
            case "Backquote":
                Enum.TryParse("OemMinus", out consoleKey);
                return consoleKey;
            case "Comma":
                Enum.TryParse("OemComma", out consoleKey);
                return consoleKey;
            case "Period":
                Enum.TryParse("OemPeriod", out consoleKey);
                return consoleKey;
            case "ArrowLeft":
                Enum.TryParse("LeftArrow", out consoleKey);
                return consoleKey;
            case "ArrowRight":
                Enum.TryParse("RightArrow", out consoleKey);
                return consoleKey;
            case "ArrowUp":
                Enum.TryParse("UpArrow", out consoleKey);
                return consoleKey;
            case "ArrowDown":
                Enum.TryParse("DownArrow", out consoleKey);
                return consoleKey;
            default:
                var success = Enum.TryParse(code, out consoleKey);
                return consoleKey;
        }
    }

    [JSInvokable]
    public ValueTask OnResize(int screenWidth, int screenHeight)
    {
        if (this._canvas2DContext == null) return ValueTask.CompletedTask;
        var terminalSettings = this.WebConsoleDriver!.TerminalSettings;
        var sw = screenWidth - terminalSettings.FontSpacePixels * 3;
        this._screenWidth = sw / terminalSettings.FontSpacePixels * terminalSettings.FontSpacePixels;
        var sh = screenHeight - terminalSettings.FontSizePixels / 2;
        this._screenHeight = sh / terminalSettings.FontSizePixels * terminalSettings.FontSizePixels;
        this._becanvas!.SetCanvasSizeAsync(this._screenWidth, this._screenHeight);
        this.InvokeAsync(StateHasChanged);
        var inputResult = new WebInputResult
        {
            EventType = WebEventType.Resize,
            ResizeEvent = new WebResizeEvent
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
        if (this._canvas2DContext == null) return default;
        HaccExtensions.WebClipboard.GetClipboardData();
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnBeforeUnload()
    {
        if (this._canvas2DContext == null) return ValueTask.CompletedTask;
        return ValueTask.CompletedTask;
    }
}