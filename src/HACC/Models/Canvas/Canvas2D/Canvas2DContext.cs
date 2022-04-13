using Blazor.Extensions.Canvas.Components;
using HACC.Components;
using Microsoft.AspNetCore.Components;

namespace HACC.Models.Canvas.Canvas2D;

public class Canvas2DContext : RenderingContext
{
    public Canvas2DContext(BECanvas reference) : base(reference: reference,
        contextName: CONTEXT_NAME)
    {
    }

    #region Constants

    private const string CONTEXT_NAME = "Canvas2d";
    private const string FILL_STYLE_PROPERTY = "fillStyle";
    private const string STROKE_STYLE_PROPERTY = "strokeStyle";
    private const string FILL_RECT_METHOD = "fillRect";
    private const string CLEAR_RECT_METHOD = "clearRect";
    private const string STROKE_RECT_METHOD = "strokeRect";
    private const string FILL_TEXT_METHOD = "fillText";
    private const string STROKE_TEXT_METHOD = "strokeText";
    private const string MEASURE_TEXT_METHOD = "measureText";
    private const string LINE_WIDTH_PROPERTY = "lineWidth";
    private const string LINE_CAP_PROPERTY = "lineCap";
    private const string LINE_JOIN_PROPERTY = "lineJoin";
    private const string MITER_LIMIT_PROPERTY = "miterLimit";
    private const string GET_LINE_DASH_METHOD = "getLineDash";
    private const string SET_LINE_DASH_METHOD = "setLineDash";
    private const string LINE_DASH_OFFSET_PROPERTY = "lineDashOffset";
    private const string SHADOW_BLUR_PROPERTY = "shadowBlur";
    private const string SHADOW_COLOR_PROPERTY = "shadowColor";
    private const string SHADOW_OFFSET_X_PROPERTY = "shadowOffsetX";
    private const string SHADOW_OFFSET_Y_PROPERTY = "shadowOffsetY";
    private const string BEGIN_PATH_METHOD = "beginPath";
    private const string CLOSE_PATH_METHOD = "closePath";
    private const string MOVE_TO_METHOD = "moveTo";
    private const string LINE_TO_METHOD = "lineTo";
    private const string BEZIER_CURVE_TO_METHOD = "bezierCurveTo";
    private const string QUADRATIC_CURVE_TO_METHOD = "quadraticCurveTo";
    private const string ARC_METHOD = "arc";
    private const string ARC_TO_METHOD = "arcTo";
    private const string RECT_METHOD = "rect";
    private const string FILL_METHOD = "fill";
    private const string STROKE_METHOD = "stroke";
    private const string DRAW_FOCUS_IF_NEEDED_METHOD = "drawFocusIfNeeded";
    private const string SCROLL_PATH_INTO_VIEW_METHOD = "scrollPathIntoView";
    private const string CLIP_METHOD = "clip";
    private const string IS_POINT_IN_PATH_METHOD = "isPointInPath";
    private const string IS_POINT_IN_STROKE_METHOD = "isPointInStroke";
    private const string ROTATE_METHOD = "rotate";
    private const string SCALE_METHOD = "scale";
    private const string TRANSLATE_METHOD = "translate";
    private const string TRANSFORM_METHOD = "transform";
    private const string SET_TRANSFORM_METHOD = "setTransform";
    private const string GLOBAL_ALPHA_PROPERTY = "globalAlpha";
    private const string SAVE_METHOD = "save";
    private const string RESTORE_METHOD = "restore";
    private const string DRAW_IMAGE_METHOD = "drawImage";
    private const string CREATE_PATTERN_METHOD = "createPattern";
    private const string GLOBAL_COMPOSITE_OPERATION_PROPERTY = "globalCompositeOperation";

    private readonly string[] _repeatNames =
    {
        "repeat", "repeat-x", "repeat-y", "no-repeat",
    };

    #endregion

    #region Properties

    public object FillStyle { get; private set; } = "#000";

    public string StrokeStyle { get; private set; } = "#000";

    public string Font { get; private set; } = "10px sans-serif";

    public TextAlign TextAlign { get; private set; }

    public TextDirection Direction { get; private set; }

    public TextBaseline TextBaseline { get; private set; }

    public float LineWidth { get; private set; } = 1.0f;

    public LineCap LineCap { get; private set; }

    public LineJoin LineJoin { get; private set; }

    public float MiterLimit { get; private set; } = 10;

    public float LineDashOffset { get; private set; }

    public float ShadowBlur { get; private set; }

    public string ShadowColor { get; private set; } = "black";

    public float ShadowOffsetX { get; private set; }

    public float ShadowOffsetY { get; private set; }

    public float GlobalAlpha { get; private set; } = 1.0f;

    public string GlobalCompositeOperation { get; private set; } = "source-over";

    #endregion Properties

    #region Property Setters

    public async Task SetFillStyleAsync(object value)
    {
        this.FillStyle = value;
        await this.BatchCallAsync(name: FILL_STYLE_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetStrokeStyleAsync(string value)
    {
        this.StrokeStyle = value;
        await this.BatchCallAsync(name: STROKE_STYLE_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetFontAsync(string value)
    {
        this.Font = value;
        await this.BatchCallAsync(name: "font",
            isMethodCall: false,
            value);
    }

    public async Task SetTextAlignAsync(TextAlign value)
    {
        this.TextAlign = value;
        await this.BatchCallAsync(name: "textAlign",
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetDirectionAsync(TextDirection value)
    {
        this.Direction = value;
        await this.BatchCallAsync(name: "direction",
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetTextBaselineAsync(TextBaseline value)
    {
        this.TextBaseline = value;
        await this.BatchCallAsync(name: "textBaseline",
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetLineWidthAsync(float value)
    {
        this.LineWidth = value;
        await this.BatchCallAsync(name: LINE_WIDTH_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetLineCapAsync(LineCap value)
    {
        this.LineCap = value;
        await this.BatchCallAsync(name: LINE_CAP_PROPERTY,
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetLineJoinAsync(LineJoin value)
    {
        this.LineJoin = value;
        await this.BatchCallAsync(name: LINE_JOIN_PROPERTY,
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetMiterLimitAsync(float value)
    {
        this.MiterLimit = value;
        await this.BatchCallAsync(name: MITER_LIMIT_PROPERTY,
            isMethodCall: false,
            value.ToString().ToLowerInvariant());
    }

    public async Task SetLineDashOffsetAsync(float value)
    {
        this.LineDashOffset = value;
        await this.BatchCallAsync(name: LINE_DASH_OFFSET_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetShadowBlurAsync(float value)
    {
        this.ShadowBlur = value;
        await this.BatchCallAsync(name: SHADOW_BLUR_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetShadowColorAsync(string value)
    {
        this.ShadowColor = value;
        await this.BatchCallAsync(name: SHADOW_COLOR_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetShadowOffsetXAsync(float value)
    {
        this.ShadowOffsetX = value;
        await this.BatchCallAsync(name: SHADOW_OFFSET_X_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetShadowOffsetYAsync(float value)
    {
        this.ShadowOffsetY = value;
        await this.BatchCallAsync(name: SHADOW_OFFSET_Y_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetGlobalAlphaAsync(float value)
    {
        this.GlobalAlpha = value;
        await this.BatchCallAsync(name: GLOBAL_ALPHA_PROPERTY,
            isMethodCall: false,
            value);
    }

    public async Task SetGlobalCompositeOperationAsync(string value)
    {
        this.GlobalCompositeOperation = value;
        await this.BatchCallAsync(name: GLOBAL_COMPOSITE_OPERATION_PROPERTY,
            isMethodCall: false,
            value);
    }

    #endregion Property Setters

    #region Methods

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void FillRect(double x, double y, double width, double height)
    {
        this.CallMethod<object>(method: FILL_RECT_METHOD,
            x,
            y,
            width,
            height);
    }

    public async Task FillRectAsync(double x, double y, double width, double height)
    {
        await this.BatchCallAsync(name: FILL_RECT_METHOD,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ClearRect(double x, double y, double width, double height)
    {
        this.CallMethod<object>(method: CLEAR_RECT_METHOD,
            x,
            y,
            width,
            height);
    }

    public async Task ClearRectAsync(double x, double y, double width, double height)
    {
        await this.BatchCallAsync(name: CLEAR_RECT_METHOD,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StrokeRect(double x, double y, double width, double height)
    {
        this.CallMethod<object>(method: STROKE_RECT_METHOD,
            x,
            y,
            width,
            height);
    }

    public async Task StrokeRectAsync(double x, double y, double width, double height)
    {
        await this.BatchCallAsync(name: STROKE_RECT_METHOD,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void FillText(string text, double x, double y, double? maxWidth = null)
    {
        this.CallMethod<object>(method: FILL_TEXT_METHOD,
            value: maxWidth.HasValue ? new object[] {text, x, y, maxWidth.Value} : new object[] {text, x, y});
    }

    public async Task FillTextAsync(string text, double x, double y, double? maxWidth = null)
    {
        await this.BatchCallAsync(name: FILL_TEXT_METHOD,
            isMethodCall: true,
            value: maxWidth.HasValue ? new object[] {text, x, y, maxWidth.Value} : new object[] {text, x, y});
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StrokeText(string text, double x, double y, double? maxWidth = null)
    {
        this.CallMethod<object>(method: STROKE_TEXT_METHOD,
            value: maxWidth.HasValue ? new object[] {text, x, y, maxWidth.Value} : new object[] {text, x, y});
    }

    public async Task StrokeTextAsync(string text, double x, double y, double? maxWidth = null)
    {
        await this.BatchCallAsync(name: STROKE_TEXT_METHOD,
            isMethodCall: true,
            value: maxWidth.HasValue ? new object[] {text, x, y, maxWidth.Value} : new object[] {text, x, y});
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public TextMetrics MeasureText(string text)
    {
        return this.CallMethod<TextMetrics>(method: MEASURE_TEXT_METHOD,
            text);
    }

    public async Task<TextMetrics> MeasureTextAsync(string text)
    {
        return await this.CallMethodAsync<TextMetrics>(method: MEASURE_TEXT_METHOD,
            text);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public float[] GetLineDash()
    {
        return this.CallMethod<float[]>(method: GET_LINE_DASH_METHOD);
    }

    public async Task<float[]> GetLineDashAsync()
    {
        return await this.CallMethodAsync<float[]>(method: GET_LINE_DASH_METHOD);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void SetLineDash(float[] segments)
    {
        this.CallMethod<object>(method: SET_LINE_DASH_METHOD,
            segments);
    }

    public async Task SetLineDashAsync(float[] segments)
    {
        await this.BatchCallAsync(name: SET_LINE_DASH_METHOD,
            isMethodCall: true,
            segments);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BeginPath()
    {
        this.CallMethod<object>(method: BEGIN_PATH_METHOD);
    }

    public async Task BeginPathAsync()
    {
        await this.BatchCallAsync(name: BEGIN_PATH_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ClosePath()
    {
        this.CallMethod<object>(method: CLOSE_PATH_METHOD);
    }

    public async Task ClosePathAsync()
    {
        await this.BatchCallAsync(name: CLOSE_PATH_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void MoveTo(double x, double y)
    {
        this.CallMethod<object>(method: MOVE_TO_METHOD,
            x,
            y);
    }

    public async Task MoveToAsync(double x, double y)
    {
        await this.BatchCallAsync(name: MOVE_TO_METHOD,
            isMethodCall: true,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void LineTo(double x, double y)
    {
        this.CallMethod<object>(method: LINE_TO_METHOD,
            x,
            y);
    }

    public async Task LineToAsync(double x, double y)
    {
        await this.BatchCallAsync(name: LINE_TO_METHOD,
            isMethodCall: true,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BezierCurveTo(double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y)
    {
        this.CallMethod<object>(method: BEZIER_CURVE_TO_METHOD,
            cp1X,
            cp1Y,
            cp2X,
            cp2Y,
            x,
            y);
    }

    public async Task BezierCurveToAsync(double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y)
    {
        await this.BatchCallAsync(name: BEZIER_CURVE_TO_METHOD,
            isMethodCall: true,
            cp1X,
            cp1Y,
            cp2X,
            cp2Y,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void QuadraticCurveTo(double cpx, double cpy, double x, double y)
    {
        this.CallMethod<object>(method: QUADRATIC_CURVE_TO_METHOD,
            cpx,
            cpy,
            x,
            y);
    }

    public async Task QuadraticCurveToAsync(double cpx, double cpy, double x, double y)
    {
        await this.BatchCallAsync(name: QUADRATIC_CURVE_TO_METHOD,
            isMethodCall: true,
            cpx,
            cpy,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Arc(double x, double y, double radius, double startAngle, double endAngle, bool? anticlockwise = null)
    {
        this.CallMethod<object>(method: ARC_METHOD,
            value: anticlockwise.HasValue
                ? new object[] {x, y, radius, startAngle, endAngle, anticlockwise.Value}
                : new object[] {x, y, radius, startAngle, endAngle});
    }

    public async Task ArcAsync(double x, double y, double radius, double startAngle, double endAngle,
        bool? anticlockwise = null)
    {
        await this.BatchCallAsync(name: ARC_METHOD,
            isMethodCall: true,
            value: anticlockwise.HasValue
                ? new object[] {x, y, radius, startAngle, endAngle, anticlockwise.Value}
                : new object[] {x, y, radius, startAngle, endAngle});
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ArcTo(double x1, double y1, double x2, double y2, double radius)
    {
        this.CallMethod<object>(method: ARC_TO_METHOD,
            x1,
            y1,
            x2,
            y2,
            radius);
    }

    public async Task ArcToAsync(double x1, double y1, double x2, double y2, double radius)
    {
        await this.BatchCallAsync(name: ARC_TO_METHOD,
            isMethodCall: true,
            x1,
            y1,
            x2,
            y2,
            radius);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Rect(double x, double y, double width, double height)
    {
        this.CallMethod<object>(method: RECT_METHOD,
            x,
            y,
            width,
            height);
    }

    public async Task RectAsync(double x, double y, double width, double height)
    {
        await this.BatchCallAsync(name: RECT_METHOD,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Fill()
    {
        this.CallMethod<object>(method: FILL_METHOD);
    }

    public async Task FillAsync()
    {
        await this.BatchCallAsync(name: FILL_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Stroke()
    {
        this.CallMethod<object>(method: STROKE_METHOD);
    }

    public async Task StrokeAsync()
    {
        await this.BatchCallAsync(name: STROKE_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DrawFocusIfNeeded(ElementReference elementReference)
    {
        this.CallMethod<object>(method: DRAW_FOCUS_IF_NEEDED_METHOD,
            elementReference);
    }

    public async Task DrawFocusIfNeededAsync(ElementReference elementReference)
    {
        await this.BatchCallAsync(name: DRAW_FOCUS_IF_NEEDED_METHOD,
            isMethodCall: true,
            elementReference);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ScrollPathIntoView()
    {
        this.CallMethod<object>(method: SCROLL_PATH_INTO_VIEW_METHOD);
    }

    public async Task ScrollPathIntoViewAsync()
    {
        await this.BatchCallAsync(name: SCROLL_PATH_INTO_VIEW_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Clip()
    {
        this.CallMethod<object>(method: CLIP_METHOD);
    }

    public async Task ClipAsync()
    {
        await this.BatchCallAsync(name: CLIP_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsPointInPath(double x, double y)
    {
        return this.CallMethod<bool>(method: IS_POINT_IN_PATH_METHOD,
            x,
            y);
    }

    public async Task<bool> IsPointInPathAsync(double x, double y)
    {
        return await this.CallMethodAsync<bool>(method: IS_POINT_IN_PATH_METHOD,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsPointInStroke(double x, double y)
    {
        return this.CallMethod<bool>(method: IS_POINT_IN_STROKE_METHOD,
            x,
            y);
    }

    public async Task<bool> IsPointInStrokeAsync(double x, double y)
    {
        return await this.CallMethodAsync<bool>(method: IS_POINT_IN_STROKE_METHOD,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Rotate(float angle)
    {
        this.CallMethod<object>(method: ROTATE_METHOD,
            angle);
    }

    public async Task RotateAsync(float angle)
    {
        await this.BatchCallAsync(name: ROTATE_METHOD,
            isMethodCall: true,
            angle);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Scale(double x, double y)
    {
        this.CallMethod<object>(method: SCALE_METHOD,
            x,
            y);
    }

    public async Task ScaleAsync(double x, double y)
    {
        await this.BatchCallAsync(name: SCALE_METHOD,
            isMethodCall: true,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Translate(double x, double y)
    {
        this.CallMethod<object>(method: TRANSLATE_METHOD,
            x,
            y);
    }

    public async Task TranslateAsync(double x, double y)
    {
        await this.BatchCallAsync(name: TRANSLATE_METHOD,
            isMethodCall: true,
            x,
            y);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Transform(double m11, double m12, double m21, double m22, double dx, double dy)
    {
        this.CallMethod<object>(method: TRANSFORM_METHOD,
            m11,
            m12,
            m21,
            m22,
            dx,
            dy);
    }

    public async Task TransformAsync(double m11, double m12, double m21, double m22, double dx, double dy)
    {
        await this.BatchCallAsync(name: TRANSFORM_METHOD,
            isMethodCall: true,
            m11,
            m12,
            m21,
            m22,
            dx,
            dy);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void SetTransform(double m11, double m12, double m21, double m22, double dx, double dy)
    {
        this.CallMethod<object>(method: SET_TRANSFORM_METHOD,
            m11,
            m12,
            m21,
            m22,
            dx,
            dy);
    }

    public async Task SetTransformAsync(double m11, double m12, double m21, double m22, double dx, double dy)
    {
        await this.BatchCallAsync(name: SET_TRANSFORM_METHOD,
            isMethodCall: true,
            m11,
            m12,
            m21,
            m22,
            dx,
            dy);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Save()
    {
        this.CallMethod<object>(method: SAVE_METHOD);
    }

    public async Task SaveAsync()
    {
        await this.BatchCallAsync(name: SAVE_METHOD,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Restore()
    {
        this.CallMethod<object>(method: RESTORE_METHOD);
    }

    public async Task RestoreAsync()
    {
        await this.BatchCallAsync(name: RESTORE_METHOD,
            isMethodCall: true);
    }

    public async Task DrawImageAsync(ElementReference elementReference, double dx, double dy)
    {
        await this.BatchCallAsync(name: DRAW_IMAGE_METHOD,
            isMethodCall: true,
            elementReference,
            dx,
            dy);
    }

    public async Task DrawImageAsync(ElementReference elementReference, double dx, double dy, double dWidth,
        double dHeight)
    {
        await this.BatchCallAsync(name: DRAW_IMAGE_METHOD,
            isMethodCall: true,
            elementReference,
            dx,
            dy,
            dWidth,
            dHeight);
    }

    public async Task DrawImageAsync(ElementReference elementReference, double sx, double sy, double sWidth,
        double sHeight, double dx, double dy, double dWidth, double dHeight)
    {
        await this.BatchCallAsync(name: DRAW_IMAGE_METHOD,
            isMethodCall: true,
            elementReference,
            sx,
            sy,
            sWidth,
            sHeight,
            dx,
            dy,
            dWidth,
            dHeight);
    }

    public async Task<object> CreatePatternAsync(ElementReference image, RepeatPattern repeat)
    {
        return await this.CallMethodAsync<object>(method: CREATE_PATTERN_METHOD,
            image,
            this._repeatNames[(int) repeat]);
    }

    #endregion Methods
}