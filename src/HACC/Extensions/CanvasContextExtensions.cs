using HACC.Components;
using HACC.Models.Canvas.Canvas2D;
using HACC.Models.Canvas.WebGL;

namespace HACC.Extensions;

public static class CanvasContextExtensions
{
    public static Canvas2DContext CreateCanvas2D(this BECanvas canvas)
    {
        return (new Canvas2DContext(reference: canvas).InitializeAsync().GetAwaiter().GetResult() as Canvas2DContext)!;
    }

    public static async Task<Canvas2DContext> CreateCanvas2DAsync(this BECanvas canvas)
    {
        return (await new Canvas2DContext(reference: canvas).InitializeAsync()
            .ConfigureAwait(continueOnCapturedContext: false) as Canvas2DContext)!;
    }

    public static WebGLContext CreateWebGL(this BECanvas canvas)
    {
        return (new WebGLContext(reference: canvas).InitializeAsync().GetAwaiter().GetResult() as WebGLContext)!;
    }

    public static async Task<WebGLContext> CreateWebGLAsync(this BECanvas canvas)
    {
        return (await new WebGLContext(reference: canvas).InitializeAsync()
            .ConfigureAwait(continueOnCapturedContext: false) as WebGLContext)!;
    }

    public static WebGLContext CreateWebGL(this BECanvas canvas, WebGLContextAttributes attributes)
    {
        return (new WebGLContext(reference: canvas,
            attributes: attributes).InitializeAsync().GetAwaiter().GetResult() as WebGLContext)!;
    }

    public static async Task<WebGLContext> CreateWebGLAsync(this BECanvas canvas, WebGLContextAttributes attributes)
    {
        return (await new WebGLContext(reference: canvas,
                attributes: attributes).InitializeAsync()
            .ConfigureAwait(continueOnCapturedContext: false) as WebGLContext)!;
    }
}