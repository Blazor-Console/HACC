using System.Drawing;
using System.Globalization;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.Components;
using Blazor.Extensions.Canvas.Extensions;
using Blazor.Extensions.Canvas.Models;
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

namespace HACC.Components;

public partial class WebConsole : ComponentBase
{
    private static readonly IJSRuntime JsInterop = HaccExtensions.GetService<IJSRuntime>();
    private static readonly ILogger Logger = HaccExtensions.CreateLogger<WebConsole>();

    protected readonly string Id = Guid.NewGuid().ToString();

    private readonly Dictionary<string, TextMetrics> MeasuredText = new();

    protected BECanvas? _becanvasRef;
    

    /// <summary>
    ///     Null until after render
    /// </summary>
    private ElementReference? _divCanvas;

    private Queue<InputResult> _inputResultQueue = new();
    private int _screenHeight = 480;
    private int _screenWidth = 640;

    [Parameter] public long Height { get; set; }

    [Parameter] public long Width { get; set; }

    public WebApplication? WebApplication { get; private set; }

    public WebConsoleDriver? WebConsoleDriver { get; private set; }

    public WebMainLoopDriver? WebMainLoopDriver { get; private set; }

    [Parameter] public EventCallback OnLoaded { get; set; }

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

            Logger.LogDebug(message: "OnAfterRenderAsync: end");
        }

        await base.OnAfterRenderAsync(firstRender: firstRender);
    }

    public async Task<object?> DrawBufferToPng()
    {
        if (!this._becanvasRef!.CanvasInitialized) return null;
        return await JsInterop!.InvokeAsync<object>(identifier: "canvasToPng");
    }


    public async Task<TextMetrics?> MeasureText(string text)
    {
        if (!this._becanvasRef!.CanvasInitialized) return null;
        if (this.MeasuredText.ContainsKey(key: text))
            return this.MeasuredText[key: text];

        var result = await this._becanvasRef.MeasureTextAsync(text: text);

        this.MeasuredText.Add(
            key: text,
            value: result!);
        return result;
    }

    private async Task RedrawCanvas()
    {
        if (!this._becanvasRef!.CanvasInitialized) return;

        Logger.LogDebug(message: "InitializeNewCanvasFrame");

        // TODO: actually clear the canvas
        await this._becanvasRef.Canvas2DContext!.SetFillStyleAsync(value: "blue");
        await this._becanvasRef.Canvas2DContext.ClearRectAsync(
            x: 0,
            y: 0,
            width: this.WebConsoleDriver!.WindowWidthPixels,
            height: this.WebConsoleDriver.WindowHeightPixels);
        await this._becanvasRef.Canvas2DContext.FillRectAsync(
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
        if (!this._becanvasRef!.CanvasInitialized) return;
        if (segments.Count == 0) return;

        Logger.LogDebug(message: "DrawBufferToFrame");
        var lastRow = segments[index: 0].Row;
        double textWidthEm = segments[index: 0].Column;

        foreach (var segment in segments)
        {
            if (segment.Row != lastRow)
            {
                lastRow = segment.Row;
                textWidthEm = segment.Column;
            }

            var measuredText = await this.MeasureText(text: segment.Text);
            var letterWidthPx = terminalSettings.FontSizePixels;
            await this._becanvasRef.Canvas2DContext!.SetFontAsync(
                value: $"{letterWidthPx}px " +
                       $"{terminalSettings.FontType}");
            await this._becanvasRef.Canvas2DContext.SetTextBaselineAsync(value: TextBaseline.Top);
            await this._becanvasRef.Canvas2DContext!.SetFillStyleAsync(
                value: $"{segment.BackgroundColor}");
            await this._becanvasRef.Canvas2DContext.FillRectAsync(
                x: textWidthEm,
                y: segment.Row * letterWidthPx,
                width: segment.Text.Length * measuredText!.Width,
                height: letterWidthPx);
            await this._becanvasRef.Canvas2DContext!.SetStrokeStyleAsync(
                value: $"{segment.ForegroundColor}");
            await this._becanvasRef.Canvas2DContext.StrokeTextAsync(text: segment.Text,
                x: textWidthEm,
                y: segment.Row * letterWidthPx);

            textWidthEm += measuredText!.Width;
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

    private void OnReadConsoleInput(InputResult inputResult)
    {
        this.ReadConsoleInput?.Invoke(obj: inputResult);
        this.RunIterationNeeded?.Invoke();
    }

    [JSInvokable]
    public async Task OnCanvasClick(MouseEventArgs obj)
    {
        // of relevance: ActiveConsole
        var inputResult = new InputResult
        {
            EventType = EventType.Mouse,
            MouseEvent = new WebMouseEvent
            {
                ButtonState = MouseButtonState.Button1Clicked,
            },
        };
        this.OnReadConsoleInput(inputResult: inputResult);
        // no-op await to keep compiler happy
        await Task.Run(() =>
        {

        });
    }

    [JSInvokable]
    public async Task OnCanvasKeyDown(KeyboardEventArgs obj)
    {
        // no-op await to keep compiler happy
        await Task.Run(() =>
        {

        });
        // of relevance: ActiveConsole
        throw new NotImplementedException();
    }

    [JSInvokable]
    public async Task OnCanvasKeyUp(KeyboardEventArgs obj)
    {
        // no-op await to keep compiler happy
        await Task.Run(() =>
        {

        });
        // of relevance: ActiveConsole
        throw new NotImplementedException();
    }

    [JSInvokable]
    public async Task OnCanvasKeyPress(KeyboardEventArgs arg)
    {
        // no-op await to keep compiler happy
        await Task.Run(() =>
        {

        });
        // of relevance: ActiveConsole
        throw new NotImplementedException();
    }

    [JSInvokable]
    public ValueTask OnResize(int screenWidth, int screenHeight)
    {
        if (this._becanvasRef!.Canvas2DContext == null) return ValueTask.CompletedTask;
        this._screenWidth = screenWidth;
        this._screenHeight = screenHeight;
        var inputResult = new InputResult
        {
            EventType = EventType.Resize,
            ResizeEvent = new ResizeEvent
            {
                Size = new Size(width: screenWidth,
                    height: screenHeight),
            },
        };
        this.OnReadConsoleInput(inputResult: inputResult);
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnFocus()
    {
        if (this._becanvasRef!.Canvas2DContext == null) return ValueTask.CompletedTask;
        return ValueTask.CompletedTask;
    }

    [JSInvokable]
    public ValueTask OnBeforeUnload()
    {
        if (this._becanvasRef!.Canvas2DContext == null) return ValueTask.CompletedTask;
        return ValueTask.CompletedTask;
    }
}