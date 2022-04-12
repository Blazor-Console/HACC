using System.Drawing;
using System.Globalization;
using HACC.Applications;
using HACC.Extensions;
using HACC.Models;
using HACC.Models.Canvas;
using HACC.Models.Canvas.Canvas2D;
using HACC.Models.Drivers;
using HACC.Models.Enums;
using HACC.Models.Structs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace HACC.Components;

public partial class BECanvas : ComponentBase
{
    protected static readonly IJSRuntime JsInterop = HaccExtensions.GetService<IJSRuntime>();
    protected static readonly ILogger Logger = HaccExtensions.CreateLogger<WebConsole>();

    public readonly string Id = Guid.NewGuid().ToString();

    /// <summary>
    ///     Null until after render when we initialize it from the beCanvas reference
    /// </summary>
    private Canvas2DContext? _canvas2DContext;

    protected ElementReference _canvasRef;

    [Parameter] public long Height { get; set; }

    [Parameter] public long Width { get; set; }

    public ElementReference CanvasReference => this._canvasRef;

    public bool CanvasInitialized => this._canvas2DContext is { };

    [Parameter] public EventCallback OnLoaded { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Logger.LogDebug(message: "OnAfterRenderAsync");
            this._canvas2DContext = await this.CreateCanvas2DAsync();

            await this.OnLoaded.InvokeAsync();

            Logger.LogDebug(message: "OnAfterRenderAsync: end");
        }

        await base.OnAfterRenderAsync(firstRender: firstRender);
    }

    public async Task<object?> DrawBufferToPng()
    {
        if (!this.CanvasInitialized) return null;
        return await JsInterop!.InvokeAsync<object>(identifier: "canvasToPng");
    }
}