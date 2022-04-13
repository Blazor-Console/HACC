using System.Runtime.InteropServices;
using Blazor.Extensions.Canvas.Components;
using HACC.Components;
using HACC.Enumerations;

namespace HACC.Models.Canvas.WebGL;

public class WebGLContext : RenderingContext
{
    public WebGLContext(BECanvas reference, WebGLContextAttributes? attributes = null) : base(reference: reference,
        contextName: CONTEXT_NAME,
        parameters: attributes)
    {
    }

    protected override async Task ExtendedInitializeAsync()
    {
        this.DrawingBufferWidth = await this.GetDrawingBufferWidthAsync();
        this.DrawingBufferHeight = await this.GetDrawingBufferHeightAsync();
    }

    #region Constants

    private const string CONTEXT_NAME = "WebGL";
    private const string CLEAR_COLOR = "clearColor";
    private const string CLEAR = "clear";
    private const string DRAWING_BUFFER_WIDTH = "drawingBufferWidth";
    private const string DRAWING_BUFFER_HEIGHT = "drawingBufferHeight";
    private const string GET_CONTEXT_ATTRIBUTES = "getContextAttributes";
    private const string IS_CONTEXT_LOST = "isContextLost";
    private const string SCISSOR = "scissor";
    private const string VIEWPORT = "viewport";
    private const string ACTIVE_TEXTURE = "activeTexture";
    private const string BLEND_COLOR = "blendColor";
    private const string BLEND_EQUATION = "blendEquation";
    private const string BLEND_EQUATION_SEPARATE = "blendEquationSeparate";
    private const string BLEND_FUNC = "blendFunc";
    private const string BLEND_FUNC_SEPARATE = "blendFuncSeparate";
    private const string CLEAR_DEPTH = "clearDepth";
    private const string CLEAR_STENCIL = "clearStencil";
    private const string COLOR_MASK = "colorMask";
    private const string CULL_FACE = "cullFace";
    private const string DEPTH_FUNC = "depthFunc";
    private const string DEPTH_MASK = "depthMask";
    private const string DEPTH_RANGE = "depthRange";
    private const string DISABLE = "disable";
    private const string ENABLE = "enable";
    private const string FRONT_FACE = "frontFace";
    private const string GET_PARAMETER = "getParameter";
    private const string GET_ERROR = "getError";
    private const string HINT = "hint";
    private const string IS_ENABLED = "isEnabled";
    private const string LINE_WIDTH = "lineWidth";
    private const string PIXEL_STORE_I = "pixelStorei";
    private const string POLYGON_OFFSET = "polygonOffset";
    private const string SAMPLE_COVERAGE = "sampleCoverage";
    private const string STENCIL_FUNC = "stencilFunc";
    private const string STENCIL_FUNC_SEPARATE = "stencilFuncSeparate";
    private const string STENCIL_MASK = "stencilMask";
    private const string STENCIL_MASK_SEPARATE = "stencilMaskSeparate";
    private const string STENCIL_OP = "stencilOp";
    private const string STENCIL_OP_SEPARATE = "stencilOpSeparate";
    private const string BIND_BUFFER = "bindBuffer";
    private const string BUFFER_DATA = "bufferData";
    private const string BUFFER_SUB_DATA = "bufferSubData";
    private const string CREATE_BUFFER = "createBuffer";
    private const string DELETE_BUFFER = "deleteBuffer";
    private const string GET_BUFFER_PARAMETER = "getBufferParameter";
    private const string IS_BUFFER = "isBuffer";
    private const string BIND_FRAMEBUFFER = "bindFramebuffer";
    private const string CHECK_FRAMEBUFFER_STATUS = "checkFramebufferStatus";
    private const string CREATE_FRAMEBUFFER = "createFramebuffer";
    private const string DELETE_FRAMEBUFFER = "deleteFramebuffer";
    private const string FRAMEBUFFER_RENDERBUFFER = "framebufferRenderbuffer";
    private const string FRAMEBUFFER_TEXTURE_2D = "framebufferTexture2D";
    private const string GET_FRAMEBUFFER_ATTACHMENT_PARAMETER = "getFramebufferAttachmentParameter";
    private const string IS_FRAMEBUFFER = "isFramebuffer";
    private const string READ_PIXELS = "readPixels";
    private const string BIND_RENDERBUFFER = "bindRenderbuffer";
    private const string CREATE_RENDERBUFFER = "createRenderbuffer";
    private const string DELETE_RENDERBUFFER = "deleteRenderbuffer";
    private const string GET_RENDERBUFFER_PARAMETER = "getRenderbufferParameter";
    private const string IS_RENDERBUFFER = "isRenderbuffer";
    private const string RENDERBUFFER_STORAGE = "renderbufferStorage";
    private const string BIND_TEXTURE = "bindTexture";
    private const string COPY_TEX_IMAGE_2D = "copyTexImage2D";
    private const string COPY_TEX_SUB_IMAGE_2D = "copyTexSubImage2D";
    private const string CREATE_TEXTURE = "createTexture";
    private const string DELETE_TEXTURE = "deleteTexture";
    private const string GENERATE_MIPMAP = "generateMipmap";
    private const string GET_TEX_PARAMETER = "getTexParameter";
    private const string IS_TEXTURE = "isTexture";
    private const string TEX_IMAGE_2D = "texImage2D";
    private const string TEX_SUB_IMAGE_2D = "texSubImage2D";
    private const string TEX_PARAMETER_F = "texParameterf";
    private const string TEX_PARAMETER_I = "texParameteri";
    private const string ATTACH_SHADER = "attachShader";
    private const string BIND_ATTRIB_LOCATION = "bindAttribLocation";
    private const string COMPILE_SHADER = "compileShader";
    private const string CREATE_PROGRAM = "createProgram";
    private const string CREATE_SHADER = "createShader";
    private const string DELETE_PROGRAM = "deleteProgram";
    private const string DELETE_SHADER = "deleteShader";
    private const string DETACH_SHADER = "detachShader";
    private const string GET_ATTACHED_SHADERS = "getAttachedShaders";
    private const string GET_PROGRAM_PARAMETER = "getProgramParameter";
    private const string GET_PROGRAM_INFO_LOG = "getProgramInfoLog";
    private const string GET_SHADER_PARAMETER = "getShaderParameter";
    private const string GET_SHADER_PRECISION_FORMAT = "getShaderPrecisionFormat";
    private const string GET_SHADER_INFO_LOG = "getShaderInfoLog";
    private const string GET_SHADER_SOURCE = "getShaderSource";
    private const string IS_PROGRAM = "isProgram";
    private const string IS_SHADER = "isShader";
    private const string LINK_PROGRAM = "linkProgram";
    private const string SHADER_SOURCE = "shaderSource";
    private const string USE_PROGRAM = "useProgram";
    private const string VALIDATE_PROGRAM = "validateProgram";
    private const string DISABLE_VERTEX_ATTRIB_ARRAY = "disableVertexAttribArray";
    private const string ENABLE_VERTEX_ATTRIB_ARRAY = "enableVertexAttribArray";
    private const string GET_ACTIVE_ATTRIB = "getActiveAttrib";
    private const string GET_ACTIVE_UNIFORM = "getActiveUniform";
    private const string GET_ATTRIB_LOCATION = "getAttribLocation";
    private const string GET_UNIFORM = "getUniform";
    private const string GET_UNIFORM_LOCATION = "getUniformLocation";
    private const string GET_VERTEX_ATTRIB = "getVertexAttrib";
    private const string GET_VERTEX_ATTRIB_OFFSET = "getVertexAttribOffset";
    private const string UNIFORM = "uniform";
    private const string UNIFORM_MATRIX = "uniformMatrix";
    private const string VERTEX_ATTRIB = "vertexAttrib";
    private const string VERTEX_ATTRIB_POINTER = "vertexAttribPointer";
    private const string DRAW_ARRAYS = "drawArrays";
    private const string DRAW_ELEMENTS = "drawElements";
    private const string FINISH = "finish";
    private const string FLUSH = "flush";

    #endregion

    #region Properties

    public int DrawingBufferWidth { get; private set; }
    public int DrawingBufferHeight { get; private set; }

    #endregion

    #region Methods

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ClearColor(float red, float green, float blue, float alpha)
    {
        this.CallMethod<object>(method: CLEAR_COLOR,
            red,
            green,
            blue,
            alpha);
    }

    public async Task ClearColorAsync(float red, float green, float blue, float alpha)
    {
        await this.BatchCallAsync(name: CLEAR_COLOR,
            isMethodCall: true,
            red,
            green,
            blue,
            alpha);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Clear(BufferBits mask)
    {
        this.CallMethod<object>(method: CLEAR,
            mask);
    }

    public async Task ClearAsync(BufferBits mask)
    {
        await this.BatchCallAsync(name: CLEAR,
            isMethodCall: true,
            mask);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLContextAttributes GetContextAttributes()
    {
        return this.CallMethod<WebGLContextAttributes>(method: GET_CONTEXT_ATTRIBUTES);
    }

    public async Task<WebGLContextAttributes> GetContextAttributesAsync()
    {
        return await this.CallMethodAsync<WebGLContextAttributes>(method: GET_CONTEXT_ATTRIBUTES);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsContextLost()
    {
        return this.CallMethod<bool>(method: IS_CONTEXT_LOST);
    }

    public async Task<bool> IsContextLostAsync()
    {
        return await this.CallMethodAsync<bool>(method: IS_CONTEXT_LOST);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Scissor(int x, int y, int width, int height)
    {
        this.CallMethod<object>(method: SCISSOR,
            x,
            y,
            width,
            height);
    }

    public async Task ScissorAsync(int x, int y, int width, int height)
    {
        await this.BatchCallAsync(name: SCISSOR,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Viewport(int x, int y, int width, int height)
    {
        this.CallMethod<object>(method: VIEWPORT,
            x,
            y,
            width,
            height);
    }

    public async Task ViewportAsync(int x, int y, int width, int height)
    {
        await this.BatchCallAsync(name: VIEWPORT,
            isMethodCall: true,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ActiveTexture(Texture texture)
    {
        this.CallMethod<object>(method: ACTIVE_TEXTURE,
            texture);
    }

    public async Task ActiveTextureAsync(Texture texture)
    {
        await this.BatchCallAsync(name: ACTIVE_TEXTURE,
            isMethodCall: true,
            texture);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BlendColor(float red, float green, float blue, float alpha)
    {
        this.CallMethod<object>(method: BLEND_COLOR,
            red,
            green,
            blue,
            alpha);
    }

    public async Task BlendColorAsync(float red, float green, float blue, float alpha)
    {
        await this.BatchCallAsync(name: BLEND_COLOR,
            isMethodCall: true,
            red,
            green,
            blue,
            alpha);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BlendEquation(BlendingEquation equation)
    {
        this.CallMethod<object>(method: BLEND_EQUATION,
            equation);
    }

    public async Task BlendEquationAsync(BlendingEquation equation)
    {
        await this.BatchCallAsync(name: BLEND_EQUATION,
            isMethodCall: true,
            equation);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BlendEquationSeparate(BlendingEquation modeRGB, BlendingEquation modeAlpha)
    {
        this.CallMethod<object>(method: BLEND_EQUATION_SEPARATE,
            modeRGB,
            modeAlpha);
    }

    public async Task BlendEquationSeparateAsync(BlendingEquation modeRGB, BlendingEquation modeAlpha)
    {
        await this.BatchCallAsync(name: BLEND_EQUATION_SEPARATE,
            isMethodCall: true,
            modeRGB,
            modeAlpha);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BlendFunc(BlendingMode sfactor, BlendingMode dfactor)
    {
        this.CallMethod<object>(method: BLEND_FUNC,
            sfactor,
            dfactor);
    }

    public async Task BlendFuncAsync(BlendingMode sfactor, BlendingMode dfactor)
    {
        await this.BatchCallAsync(name: BLEND_FUNC,
            isMethodCall: true,
            sfactor,
            dfactor);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BlendFuncSeparate(BlendingMode srcRGB, BlendingMode dstRGB, BlendingMode srcAlpha,
        BlendingMode dstAlpha)
    {
        this.CallMethod<object>(method: BLEND_FUNC_SEPARATE,
            srcRGB,
            dstRGB,
            srcAlpha,
            dstAlpha);
    }

    public async Task BlendFuncSeparateAsync(BlendingMode srcRGB, BlendingMode dstRGB, BlendingMode srcAlpha,
        BlendingMode dstAlpha)
    {
        await this.BatchCallAsync(name: BLEND_FUNC_SEPARATE,
            isMethodCall: true,
            srcRGB,
            dstRGB,
            srcAlpha,
            dstAlpha);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ClearDepth(float depth)
    {
        this.CallMethod<object>(method: CLEAR_DEPTH,
            depth);
    }

    public async Task ClearDepthAsync(float depth)
    {
        await this.BatchCallAsync(name: CLEAR_DEPTH,
            isMethodCall: true,
            depth);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ClearStencil(int stencil)
    {
        this.CallMethod<object>(method: CLEAR_STENCIL,
            stencil);
    }

    public async Task ClearStencilAsync(int stencil)
    {
        await this.BatchCallAsync(name: CLEAR_STENCIL,
            isMethodCall: true,
            stencil);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ColorMask(bool red, bool green, bool blue, bool alpha)
    {
        this.CallMethod<object>(method: COLOR_MASK,
            red,
            green,
            blue,
            alpha);
    }

    public async Task ColorMaskAsync(bool red, bool green, bool blue, bool alpha)
    {
        await this.BatchCallAsync(name: COLOR_MASK,
            isMethodCall: true,
            red,
            green,
            blue,
            alpha);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void CullFace(Face mode)
    {
        this.CallMethod<object>(method: CULL_FACE,
            mode);
    }

    public async Task CullFaceAsync(Face mode)
    {
        await this.BatchCallAsync(name: CULL_FACE,
            isMethodCall: true,
            mode);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DepthFunc(CompareFunction func)
    {
        this.CallMethod<object>(method: DEPTH_FUNC,
            func);
    }

    public async Task DepthFuncAsync(CompareFunction func)
    {
        await this.BatchCallAsync(name: DEPTH_FUNC,
            isMethodCall: true,
            func);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DepthMask(bool flag)
    {
        this.CallMethod<object>(method: DEPTH_MASK,
            flag);
    }

    public async Task DepthMaskAsync(bool flag)
    {
        await this.BatchCallAsync(name: DEPTH_MASK,
            isMethodCall: true,
            flag);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DepthRange(float zNear, float zFar)
    {
        this.CallMethod<object>(method: DEPTH_RANGE,
            zNear,
            zFar);
    }

    public async Task DepthRangeAsync(float zNear, float zFar)
    {
        await this.BatchCallAsync(name: DEPTH_RANGE,
            isMethodCall: true,
            zNear,
            zFar);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Disable(EnableCap cap)
    {
        this.CallMethod<object>(method: DISABLE,
            cap);
    }

    public async Task DisableAsync(EnableCap cap)
    {
        await this.BatchCallAsync(name: DISABLE,
            isMethodCall: true,
            cap);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Enable(EnableCap cap)
    {
        this.CallMethod<object>(method: ENABLE,
            cap);
    }

    public async Task EnableAsync(EnableCap cap)
    {
        await this.BatchCallAsync(name: ENABLE,
            isMethodCall: true,
            cap);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void FrontFace(FrontFaceDirection mode)
    {
        this.CallMethod<object>(method: FRONT_FACE,
            mode);
    }

    public async Task FrontFaceAsync(FrontFaceDirection mode)
    {
        await this.BatchCallAsync(name: FRONT_FACE,
            isMethodCall: true,
            mode);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetParameter<T>(Parameter parameter)
    {
        return this.CallMethod<T>(method: GET_PARAMETER,
            parameter);
    }

    public async Task<T> GetParameterAsync<T>(Parameter parameter)
    {
        return await this.CallMethodAsync<T>(method: GET_PARAMETER,
            parameter);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public Error GetError()
    {
        return this.CallMethod<Error>(method: GET_ERROR);
    }

    public async Task<Error> GetErrorAsync()
    {
        return await this.CallMethodAsync<Error>(method: GET_ERROR);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Hint(HintTarget target, HintMode mode)
    {
        this.CallMethod<object>(method: HINT,
            target,
            mode);
    }

    public async Task HintAsync(HintTarget target, HintMode mode)
    {
        await this.BatchCallAsync(name: HINT,
            isMethodCall: true,
            target,
            mode);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsEnabled(EnableCap cap)
    {
        return this.CallMethod<bool>(method: IS_ENABLED,
            cap);
    }

    public async Task<bool> IsEnabledAsync(EnableCap cap)
    {
        return await this.CallMethodAsync<bool>(method: IS_ENABLED,
            cap);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void LineWidth(float width)
    {
        this.CallMethod<object>(method: LINE_WIDTH,
            width);
    }

    public async Task LineWidthAsync(float width)
    {
        await this.CallMethodAsync<object>(method: LINE_WIDTH,
            width);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool PixelStoreI(PixelStorageMode pname, int param)
    {
        return this.CallMethod<bool>(method: PIXEL_STORE_I,
            pname,
            param);
    }

    public async Task<bool> PixelStoreIAsync(PixelStorageMode pname, int param)
    {
        return await this.CallMethodAsync<bool>(method: PIXEL_STORE_I,
            pname,
            param);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void PolygonOffset(float factor, float units)
    {
        this.CallMethod<object>(method: POLYGON_OFFSET,
            factor,
            units);
    }

    public async Task PolygonOffsetAsync(float factor, float units)
    {
        await this.BatchCallAsync(name: POLYGON_OFFSET,
            isMethodCall: true,
            factor,
            units);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void SampleCoverage(float value, bool invert)
    {
        this.CallMethod<object>(method: SAMPLE_COVERAGE,
            value,
            invert);
    }

    public async Task SampleCoverageAsync(float value, bool invert)
    {
        await this.BatchCallAsync(name: SAMPLE_COVERAGE,
            isMethodCall: true,
            value,
            invert);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilFunc(CompareFunction func, int reference, uint mask)
    {
        this.CallMethod<object>(method: STENCIL_FUNC,
            func,
            reference,
            mask);
    }

    public async Task StencilFuncAsync(CompareFunction func, int reference, uint mask)
    {
        await this.BatchCallAsync(name: STENCIL_FUNC,
            isMethodCall: true,
            func,
            reference,
            mask);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilFuncSeparate(Face face, CompareFunction func, int reference, uint mask)
    {
        this.CallMethod<object>(method: STENCIL_FUNC_SEPARATE,
            face,
            func,
            reference,
            mask);
    }

    public async Task StencilFuncSeparateAsync(Face face, CompareFunction func, int reference, uint mask)
    {
        await this.BatchCallAsync(name: STENCIL_FUNC_SEPARATE,
            isMethodCall: true,
            face,
            func,
            reference,
            mask);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilMask(uint mask)
    {
        this.CallMethod<object>(method: STENCIL_MASK,
            mask);
    }

    public async Task StencilMaskAsync(uint mask)
    {
        await this.BatchCallAsync(name: STENCIL_MASK,
            isMethodCall: true,
            mask);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilMaskSeparate(Face face, uint mask)
    {
        this.CallMethod<object>(method: STENCIL_MASK_SEPARATE,
            face,
            mask);
    }

    public async Task StencilMaskSeparateAsync(Face face, uint mask)
    {
        await this.BatchCallAsync(name: STENCIL_MASK_SEPARATE,
            isMethodCall: true,
            face,
            mask);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilOp(StencilFunction fail, StencilFunction zfail, StencilFunction zpass)
    {
        this.CallMethod<object>(method: STENCIL_OP,
            fail,
            zfail,
            zpass);
    }

    public async Task StencilOpAsync(StencilFunction fail, StencilFunction zfail, StencilFunction zpass)
    {
        await this.BatchCallAsync(name: STENCIL_OP,
            isMethodCall: true,
            fail,
            zfail,
            zpass);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void StencilOpSeparate(Face face, StencilFunction fail, StencilFunction zfail, StencilFunction zpass)
    {
        this.CallMethod<object>(method: STENCIL_OP_SEPARATE,
            face,
            fail,
            zfail,
            zpass);
    }

    public async Task StencilOpSeparateAsync(Face face, StencilFunction fail, StencilFunction zfail,
        StencilFunction zpass)
    {
        await this.BatchCallAsync(name: STENCIL_OP_SEPARATE,
            isMethodCall: true,
            face,
            fail,
            zfail,
            zpass);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BindBuffer(BufferType target, WebGLBuffer buffer)
    {
        this.CallMethod<object>(method: BIND_BUFFER,
            target,
            buffer);
    }

    public async Task BindBufferAsync(BufferType target, WebGLBuffer buffer)
    {
        await this.BatchCallAsync(name: BIND_BUFFER,
            isMethodCall: true,
            target,
            buffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BufferData(BufferType target, int size, BufferUsageHint usage)
    {
        this.CallMethod<object>(method: BUFFER_DATA,
            target,
            size,
            usage);
    }

    public async Task BufferDataAsync(BufferType target, int size, BufferUsageHint usage)
    {
        await this.BatchCallAsync(name: BUFFER_DATA,
            isMethodCall: true,
            target,
            size,
            usage);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BufferData<T>(BufferType target, T[] data, BufferUsageHint usage)
    {
        this.CallMethod<object>(method: BUFFER_DATA,
            target,
            this.ConvertToByteArray(arr: data),
            usage);
    }

    public async Task BufferDataAsync<T>(BufferType target, T[] data, BufferUsageHint usage)
    {
        await this.BatchCallAsync(name: BUFFER_DATA,
            isMethodCall: true,
            target,
            this.ConvertToByteArray(arr: data),
            usage);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BufferSubData<T>(BufferType target, uint offset, T[] data)
    {
        this.CallMethod<object>(method: BUFFER_SUB_DATA,
            target,
            offset,
            this.ConvertToByteArray(arr: data));
    }

    public async Task BufferSubDataAsync<T>(BufferType target, uint offset, T[] data)
    {
        await this.BatchCallAsync(name: BUFFER_SUB_DATA,
            isMethodCall: true,
            target,
            offset,
            this.ConvertToByteArray(arr: data));
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLBuffer CreateBuffer()
    {
        return this.CallMethod<WebGLBuffer>(method: CREATE_BUFFER);
    }

    public async Task<WebGLBuffer> CreateBufferAsync()
    {
        return await this.CallMethodAsync<WebGLBuffer>(method: CREATE_BUFFER);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteBuffer(WebGLBuffer buffer)
    {
        this.CallMethod<WebGLBuffer>(method: DELETE_BUFFER,
            buffer);
    }

    public async Task DeleteBufferAsync(WebGLBuffer buffer)
    {
        await this.BatchCallAsync(name: DELETE_BUFFER,
            isMethodCall: true,
            buffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetBufferParameter<T>(BufferType target, BufferParameter pname)
    {
        return this.CallMethod<T>(method: GET_BUFFER_PARAMETER,
            target,
            pname);
    }

    public async Task<T> GetBufferParameterAsync<T>(BufferType target, BufferParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_BUFFER_PARAMETER,
            target,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsBuffer(WebGLBuffer buffer)
    {
        return this.CallMethod<bool>(method: IS_BUFFER,
            buffer);
    }

    public async Task<bool> IsBufferAsync(WebGLBuffer buffer)
    {
        return await this.CallMethodAsync<bool>(method: IS_BUFFER,
            buffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BindFramebuffer(FramebufferType target, WebGLFramebuffer framebuffer)
    {
        this.CallMethod<object>(method: BIND_FRAMEBUFFER,
            target,
            framebuffer);
    }

    public async Task BindFramebufferAsync(FramebufferType target, WebGLFramebuffer framebuffer)
    {
        await this.BatchCallAsync(name: BIND_FRAMEBUFFER,
            isMethodCall: true,
            target,
            framebuffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public FramebufferStatus CheckFramebufferStatus(FramebufferType target)
    {
        return this.CallMethod<FramebufferStatus>(method: CHECK_FRAMEBUFFER_STATUS,
            target);
    }

    public async Task<FramebufferStatus> CheckFramebufferStatusAsync(FramebufferType target)
    {
        return await this.CallMethodAsync<FramebufferStatus>(method: CHECK_FRAMEBUFFER_STATUS,
            target);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLFramebuffer CreateFramebuffer()
    {
        return this.CallMethod<WebGLFramebuffer>(method: CREATE_FRAMEBUFFER);
    }

    public async Task<WebGLFramebuffer> CreateFramebufferAsync()
    {
        return await this.CallMethodAsync<WebGLFramebuffer>(method: CREATE_FRAMEBUFFER);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteFramebuffer(WebGLFramebuffer buffer)
    {
        this.CallMethod<object>(method: DELETE_FRAMEBUFFER,
            buffer);
    }

    public async Task DeleteFramebufferAsync(WebGLFramebuffer buffer)
    {
        await this.BatchCallAsync(name: DELETE_FRAMEBUFFER,
            isMethodCall: true,
            buffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void FramebufferRenderbuffer(FramebufferType target, FramebufferAttachment attachment,
        RenderbufferType renderbuffertarget, WebGLRenderbuffer renderbuffer)
    {
        this.CallMethod<object>(method: FRAMEBUFFER_RENDERBUFFER,
            target,
            attachment,
            renderbuffertarget,
            renderbuffer);
    }

    public async Task FramebufferRenderbufferAsync(FramebufferType target, FramebufferAttachment attachment,
        RenderbufferType renderbuffertarget, WebGLRenderbuffer renderbuffer)
    {
        await this.BatchCallAsync(name: FRAMEBUFFER_RENDERBUFFER,
            isMethodCall: true,
            target,
            attachment,
            renderbuffertarget,
            renderbuffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void FramebufferTexture2D(FramebufferType target, FramebufferAttachment attachment, Texture2DType textarget,
        WebGLTexture texture, int level)
    {
        this.CallMethod<object>(method: FRAMEBUFFER_TEXTURE_2D,
            target,
            attachment,
            textarget,
            texture,
            level);
    }

    public async Task FramebufferTexture2DAsync(FramebufferType target, FramebufferAttachment attachment,
        Texture2DType textarget, WebGLTexture texture, int level)
    {
        await this.BatchCallAsync(name: FRAMEBUFFER_TEXTURE_2D,
            isMethodCall: true,
            target,
            attachment,
            textarget,
            texture,
            level);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetFramebufferAttachmentParameter<T>(FramebufferType target, FramebufferAttachment attachment,
        FramebufferAttachmentParameter pname)
    {
        return this.CallMethod<T>(method: GET_FRAMEBUFFER_ATTACHMENT_PARAMETER,
            target,
            attachment,
            pname);
    }

    public async Task<T> GetFramebufferAttachmentParameterAsync<T>(FramebufferType target,
        FramebufferAttachment attachment, FramebufferAttachmentParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_FRAMEBUFFER_ATTACHMENT_PARAMETER,
            target,
            attachment,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsFramebuffer(WebGLFramebuffer framebuffer)
    {
        return this.CallMethod<bool>(method: IS_FRAMEBUFFER,
            framebuffer);
    }

    public async Task<bool> IsFramebufferAsync(WebGLFramebuffer framebuffer)
    {
        return await this.CallMethodAsync<bool>(method: IS_FRAMEBUFFER,
            framebuffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, byte[] pixels)
    {
        this.CallMethod<object>(method: READ_PIXELS,
            x,
            y,
            width,
            height,
            format,
            type,
            pixels);
        //pixels should be an ArrayBufferView which the data gets read into
    }

    public async Task ReadPixelsAsync(int x, int y, int width, int height, PixelFormat format, PixelType type,
        byte[] pixels)
    {
        await this.BatchCallAsync(name: READ_PIXELS,
            isMethodCall: true,
            x,
            y,
            width,
            height,
            format,
            type,
            pixels);
        //pixels should be an ArrayBufferView which the data gets read into
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BindRenderbuffer(RenderbufferType target, WebGLRenderbuffer renderbuffer)
    {
        this.CallMethod<object>(method: BIND_RENDERBUFFER,
            target,
            renderbuffer);
    }

    public async Task BindRenderbufferAsync(RenderbufferType target, WebGLRenderbuffer renderbuffer)
    {
        await this.BatchCallAsync(name: BIND_RENDERBUFFER,
            isMethodCall: true,
            target,
            renderbuffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLRenderbuffer CreateRenderbuffer()
    {
        return this.CallMethod<WebGLRenderbuffer>(method: CREATE_RENDERBUFFER);
    }

    public async Task<WebGLRenderbuffer> CreateRenderbufferAsync()
    {
        return await this.CallMethodAsync<WebGLRenderbuffer>(method: CREATE_RENDERBUFFER);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteRenderbuffer(WebGLRenderbuffer buffer)
    {
        this.CallMethod<object>(method: DELETE_RENDERBUFFER,
            buffer);
    }

    public async Task DeleteRenderbufferAsync(WebGLRenderbuffer buffer)
    {
        await this.BatchCallAsync(name: DELETE_RENDERBUFFER,
            isMethodCall: true,
            buffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetRenderbufferParameter<T>(RenderbufferType target, RenderbufferParameter pname)
    {
        return this.CallMethod<T>(method: GET_RENDERBUFFER_PARAMETER,
            target,
            pname);
    }

    public async Task<T> GetRenderbufferParameterAsync<T>(RenderbufferType target, RenderbufferParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_RENDERBUFFER_PARAMETER,
            target,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsRenderbuffer(WebGLRenderbuffer renderbuffer)
    {
        return this.CallMethod<bool>(method: IS_RENDERBUFFER,
            renderbuffer);
    }

    public async Task<bool> IsRenderbufferAsync(WebGLRenderbuffer renderbuffer)
    {
        return await this.CallMethodAsync<bool>(method: IS_RENDERBUFFER,
            renderbuffer);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void RenderbufferStorage(RenderbufferType type, RenderbufferFormat internalFormat, int width, int height)
    {
        this.CallMethod<object>(method: RENDERBUFFER_STORAGE,
            type,
            internalFormat,
            width,
            height);
    }

    public async Task RenderbufferStorageAsync(RenderbufferType type, RenderbufferFormat internalFormat, int width,
        int height)
    {
        await this.BatchCallAsync(name: RENDERBUFFER_STORAGE,
            isMethodCall: true,
            type,
            internalFormat,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BindTexture(TextureType type, WebGLTexture texture)
    {
        this.CallMethod<object>(method: BIND_TEXTURE,
            type,
            texture);
    }

    public async Task BindTextureAsync(TextureType type, WebGLTexture texture)
    {
        await this.BatchCallAsync(name: BIND_TEXTURE,
            isMethodCall: true,
            type,
            texture);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void CopyTexImage2D(Texture2DType target, int level, PixelFormat format, int x, int y, int width, int height,
        int border)
    {
        this.CallMethod<object>(method: COPY_TEX_IMAGE_2D,
            target,
            level,
            format,
            x,
            y,
            width,
            height,
            border);
    }

    public async Task CopyTexImage2DAsync(Texture2DType target, int level, PixelFormat format, int x, int y, int width,
        int height, int border)
    {
        await this.BatchCallAsync(name: COPY_TEX_IMAGE_2D,
            isMethodCall: true,
            target,
            level,
            format,
            x,
            y,
            width,
            height,
            border);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void CopyTexSubImage2D(Texture2DType target, int level, int xoffset, int yoffset, int x, int y, int width,
        int height)
    {
        this.CallMethod<object>(method: COPY_TEX_SUB_IMAGE_2D,
            target,
            level,
            xoffset,
            yoffset,
            x,
            y,
            width,
            height);
    }

    public async Task CopyTexSubImage2DAsync(Texture2DType target, int level, int xoffset, int yoffset, int x, int y,
        int width, int height)
    {
        await this.BatchCallAsync(name: COPY_TEX_SUB_IMAGE_2D,
            isMethodCall: true,
            target,
            level,
            xoffset,
            yoffset,
            x,
            y,
            width,
            height);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLTexture CreateTexture()
    {
        return this.CallMethod<WebGLTexture>(method: CREATE_TEXTURE);
    }

    public async Task<WebGLTexture> CreateTextureAsync()
    {
        return await this.CallMethodAsync<WebGLTexture>(method: CREATE_TEXTURE);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteTexture(WebGLTexture texture)
    {
        this.CallMethod<object>(method: DELETE_TEXTURE,
            texture);
    }

    public async Task DeleteTextureAsync(WebGLTexture texture)
    {
        await this.BatchCallAsync(name: DELETE_TEXTURE,
            isMethodCall: true,
            texture);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void GenerateMipmap(TextureType target)
    {
        this.CallMethod<object>(method: GENERATE_MIPMAP,
            target);
    }

    public async Task GenerateMipmapAsync(TextureType target)
    {
        await this.BatchCallAsync(name: GENERATE_MIPMAP,
            isMethodCall: true,
            target);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetTexParameter<T>(TextureType target, TextureParameter pname)
    {
        return this.CallMethod<T>(method: GET_TEX_PARAMETER,
            target,
            pname);
    }

    public async Task<T> GetTexParameterAsync<T>(TextureType target, TextureParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_TEX_PARAMETER,
            target,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsTexture(WebGLTexture texture)
    {
        return this.CallMethod<bool>(method: IS_TEXTURE,
            texture);
    }

    public async Task<bool> IsTextureAsync(WebGLTexture texture)
    {
        return await this.CallMethodAsync<bool>(method: IS_TEXTURE,
            texture);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void TexImage2D<T>(Texture2DType target, int level, PixelFormat internalFormat, int width, int height,
        PixelFormat format, PixelType type, T[] pixels)
        where T : struct
    {
        this.CallMethod<object>(method: TEX_IMAGE_2D,
            target,
            level,
            internalFormat,
            width,
            height,
            format,
            type,
            pixels);
    }

    public async Task TexImage2DAsync<T>(Texture2DType target, int level, PixelFormat internalFormat, int width,
        int height, PixelFormat format, PixelType type, T[] pixels)
        where T : struct
    {
        await this.BatchCallAsync(name: TEX_IMAGE_2D,
            isMethodCall: true,
            target,
            level,
            internalFormat,
            width,
            height,
            format,
            type,
            pixels);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void TexSubImage2D<T>(Texture2DType target, int level, int xoffset, int yoffset, int width, int height,
        PixelFormat format, PixelType type, T[] pixels)
        where T : struct
    {
        this.CallMethod<object>(method: TEX_SUB_IMAGE_2D,
            target,
            level,
            xoffset,
            yoffset,
            width,
            height,
            format,
            type,
            pixels);
    }

    public async Task TexSubImage2DAsync<T>(Texture2DType target, int level, int xoffset, int yoffset, int width,
        int height, PixelFormat format, PixelType type, T[] pixels)
        where T : struct
    {
        await this.BatchCallAsync(name: TEX_SUB_IMAGE_2D,
            isMethodCall: true,
            target,
            level,
            xoffset,
            yoffset,
            width,
            height,
            format,
            type,
            pixels);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void TexParameter(TextureType target, TextureParameter pname, float param)
    {
        this.CallMethod<object>(method: TEX_PARAMETER_F,
            target,
            pname,
            param);
    }

    public async Task TexParameterAsync(TextureType target, TextureParameter pname, float param)
    {
        await this.BatchCallAsync(name: TEX_PARAMETER_F,
            isMethodCall: true,
            target,
            pname,
            param);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void TexParameter(TextureType target, TextureParameter pname, int param)
    {
        this.CallMethod<object>(method: TEX_PARAMETER_I,
            target,
            pname,
            param);
    }

    public async Task TexParameterAsync(TextureType target, TextureParameter pname, int param)
    {
        await this.BatchCallAsync(name: TEX_PARAMETER_I,
            isMethodCall: true,
            target,
            pname,
            param);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void AttachShader(WebGLProgram program, WebGLShader shader)
    {
        this.CallMethod<object>(method: ATTACH_SHADER,
            program,
            shader);
    }

    public async Task AttachShaderAsync(WebGLProgram program, WebGLShader shader)
    {
        await this.BatchCallAsync(name: ATTACH_SHADER,
            isMethodCall: true,
            program,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void BindAttribLocation(WebGLProgram program, uint index, string name)
    {
        this.CallMethod<object>(method: BIND_ATTRIB_LOCATION,
            program,
            index,
            name);
    }

    public async Task BindAttribLocationAsync(WebGLProgram program, uint index, string name)
    {
        await this.BatchCallAsync(name: BIND_ATTRIB_LOCATION,
            isMethodCall: true,
            program,
            index,
            name);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void CompileShader(WebGLShader shader)
    {
        this.CallMethod<object>(method: COMPILE_SHADER,
            shader);
    }

    public async Task CompileShaderAsync(WebGLShader shader)
    {
        await this.BatchCallAsync(name: COMPILE_SHADER,
            isMethodCall: true,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLProgram CreateProgram()
    {
        return this.CallMethod<WebGLProgram>(method: CREATE_PROGRAM);
    }

    public async Task<WebGLProgram> CreateProgramAsync()
    {
        return await this.CallMethodAsync<WebGLProgram>(method: CREATE_PROGRAM);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLShader CreateShader(ShaderType type)
    {
        return this.CallMethod<WebGLShader>(method: CREATE_SHADER,
            type);
    }

    public async Task<WebGLShader> CreateShaderAsync(ShaderType type)
    {
        return await this.CallMethodAsync<WebGLShader>(method: CREATE_SHADER,
            type);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteProgram(WebGLProgram program)
    {
        this.CallMethod<object>(method: DELETE_PROGRAM,
            program);
    }

    public async Task DeleteProgramAsync(WebGLProgram program)
    {
        await this.BatchCallAsync(name: DELETE_PROGRAM,
            isMethodCall: true,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DeleteShader(WebGLShader shader)
    {
        this.CallMethod<object>(method: DELETE_SHADER,
            shader);
    }

    public async Task DeleteShaderAsync(WebGLShader shader)
    {
        await this.BatchCallAsync(name: DELETE_SHADER,
            isMethodCall: true,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DetachShader(WebGLProgram program, WebGLShader shader)
    {
        this.CallMethod<object>(method: DETACH_SHADER,
            program,
            shader);
    }

    public async Task DetachShaderAsync(WebGLProgram program, WebGLShader shader)
    {
        await this.BatchCallAsync(name: DETACH_SHADER,
            isMethodCall: true,
            program,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLShader[] GetAttachedShaders(WebGLProgram program)
    {
        return this.CallMethod<WebGLShader[]>(method: GET_ATTACHED_SHADERS,
            program);
    }

    public async Task<WebGLShader[]> GetAttachedShadersAsync(WebGLProgram program)
    {
        return await this.CallMethodAsync<WebGLShader[]>(method: GET_ATTACHED_SHADERS,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetProgramParameter<T>(WebGLProgram program, ProgramParameter pname)
    {
        return this.CallMethod<T>(method: GET_PROGRAM_PARAMETER,
            program,
            pname);
    }

    public async Task<T> GetProgramParameterAsync<T>(WebGLProgram program, ProgramParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_PROGRAM_PARAMETER,
            program,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public string GetProgramInfoLog(WebGLProgram program)
    {
        return this.CallMethod<string>(method: GET_PROGRAM_INFO_LOG,
            program);
    }

    public async Task<string> GetProgramInfoLogAsync(WebGLProgram program)
    {
        return await this.CallMethodAsync<string>(method: GET_PROGRAM_INFO_LOG,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetShaderParameter<T>(WebGLShader shader, ShaderParameter pname)
    {
        return this.CallMethod<T>(method: GET_SHADER_PARAMETER,
            shader,
            pname);
    }

    public async Task<T> GetShaderParameterAsync<T>(WebGLShader shader, ShaderParameter pname)
    {
        return await this.CallMethodAsync<T>(method: GET_SHADER_PARAMETER,
            shader,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLShaderPrecisionFormat GetShaderPrecisionFormat(ShaderType shaderType, ShaderPrecision precisionType)
    {
        return this.CallMethod<WebGLShaderPrecisionFormat>(method: GET_SHADER_PRECISION_FORMAT,
            shaderType,
            precisionType);
    }

    public async Task<WebGLShaderPrecisionFormat> GetShaderPrecisionFormatAsync(ShaderType shaderType,
        ShaderPrecision precisionType)
    {
        return await this.CallMethodAsync<WebGLShaderPrecisionFormat>(method: GET_SHADER_PRECISION_FORMAT,
            shaderType,
            precisionType);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public string GetShaderInfoLog(WebGLShader shader)
    {
        return this.CallMethod<string>(method: GET_SHADER_INFO_LOG,
            shader);
    }

    public async Task<string> GetShaderInfoLogAsync(WebGLShader shader)
    {
        return await this.CallMethodAsync<string>(method: GET_SHADER_INFO_LOG,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public string GetShaderSource(WebGLShader shader)
    {
        return this.CallMethod<string>(method: GET_SHADER_SOURCE,
            shader);
    }

    public async Task<string> GetShaderSourceAsync(WebGLShader shader)
    {
        return await this.CallMethodAsync<string>(method: GET_SHADER_SOURCE,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsProgram(WebGLProgram program)
    {
        return this.CallMethod<bool>(method: IS_PROGRAM,
            program);
    }

    public async Task<bool> IsProgramAsync(WebGLProgram program)
    {
        return await this.CallMethodAsync<bool>(method: IS_PROGRAM,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public bool IsShader(WebGLShader shader)
    {
        return this.CallMethod<bool>(method: IS_SHADER,
            shader);
    }

    public async Task<bool> IsShaderAsync(WebGLShader shader)
    {
        return await this.CallMethodAsync<bool>(method: IS_SHADER,
            shader);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void LinkProgram(WebGLProgram program)
    {
        this.CallMethod<object>(method: LINK_PROGRAM,
            program);
    }

    public async Task LinkProgramAsync(WebGLProgram program)
    {
        await this.BatchCallAsync(name: LINK_PROGRAM,
            isMethodCall: true,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ShaderSource(WebGLShader shader, string source)
    {
        this.CallMethod<object>(method: SHADER_SOURCE,
            shader,
            source);
    }

    public async Task ShaderSourceAsync(WebGLShader shader, string source)
    {
        await this.BatchCallAsync(name: SHADER_SOURCE,
            isMethodCall: true,
            shader,
            source);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void UseProgram(WebGLProgram program)
    {
        this.CallMethod<object>(method: USE_PROGRAM,
            program);
    }

    public async Task UseProgramAsync(WebGLProgram program)
    {
        await this.BatchCallAsync(name: USE_PROGRAM,
            isMethodCall: true,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void ValidateProgram(WebGLProgram program)
    {
        this.CallMethod<object>(method: VALIDATE_PROGRAM,
            program);
    }

    public async Task ValidateProgramAsync(WebGLProgram program)
    {
        await this.BatchCallAsync(name: VALIDATE_PROGRAM,
            isMethodCall: true,
            program);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DisableVertexAttribArray(uint index)
    {
        this.CallMethod<object>(method: DISABLE_VERTEX_ATTRIB_ARRAY,
            index);
    }

    public async Task DisableVertexAttribArrayAsync(uint index)
    {
        await this.BatchCallAsync(name: DISABLE_VERTEX_ATTRIB_ARRAY,
            isMethodCall: true,
            index);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void EnableVertexAttribArray(uint index)
    {
        this.CallMethod<object>(method: ENABLE_VERTEX_ATTRIB_ARRAY,
            index);
    }

    public async Task EnableVertexAttribArrayAsync(uint index)
    {
        await this.BatchCallAsync(name: ENABLE_VERTEX_ATTRIB_ARRAY,
            isMethodCall: true,
            index);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLActiveInfo GetActiveAttrib(WebGLProgram program, uint index)
    {
        return this.CallMethod<WebGLActiveInfo>(method: GET_ACTIVE_ATTRIB,
            program,
            index);
    }

    public async Task<WebGLActiveInfo> GetActiveAttribAsync(WebGLProgram program, uint index)
    {
        return await this.CallMethodAsync<WebGLActiveInfo>(method: GET_ACTIVE_ATTRIB,
            program,
            index);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLActiveInfo GetActiveUniform(WebGLProgram program, uint index)
    {
        return this.CallMethod<WebGLActiveInfo>(method: GET_ACTIVE_UNIFORM,
            program,
            index);
    }

    public async Task<WebGLActiveInfo> GetActiveUniformAsync(WebGLProgram program, uint index)
    {
        return await this.CallMethodAsync<WebGLActiveInfo>(method: GET_ACTIVE_UNIFORM,
            program,
            index);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public int GetAttribLocation(WebGLProgram program, string name)
    {
        return this.CallMethod<int>(method: GET_ATTRIB_LOCATION,
            program,
            name);
    }

    public async Task<int> GetAttribLocationAsync(WebGLProgram program, string name)
    {
        return await this.CallMethodAsync<int>(method: GET_ATTRIB_LOCATION,
            program,
            name);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetUniform<T>(WebGLProgram program, WebGLUniformLocation location)
    {
        return this.CallMethod<T>(method: GET_UNIFORM,
            program,
            location);
    }

    public async Task<T> GetUniformAsync<T>(WebGLProgram program, WebGLUniformLocation location)
    {
        return await this.CallMethodAsync<T>(method: GET_UNIFORM,
            program,
            location);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public WebGLUniformLocation GetUniformLocation(WebGLProgram program, string name)
    {
        return this.CallMethod<WebGLUniformLocation>(method: GET_UNIFORM_LOCATION,
            program,
            name);
    }

    public async Task<WebGLUniformLocation> GetUniformLocationAsync(WebGLProgram program, string name)
    {
        return await this.CallMethodAsync<WebGLUniformLocation>(method: GET_UNIFORM_LOCATION,
            program,
            name);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public T GetVertexAttrib<T>(uint index, VertexAttribute pname)
    {
        return this.CallMethod<T>(method: GET_VERTEX_ATTRIB,
            index,
            pname);
    }

    public async Task<T> GetVertexAttribAsync<T>(uint index, VertexAttribute pname)
    {
        return await this.CallMethodAsync<T>(method: GET_VERTEX_ATTRIB,
            index,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public long GetVertexAttribOffset(uint index, VertexAttributePointer pname)
    {
        return this.CallMethod<long>(method: GET_VERTEX_ATTRIB_OFFSET,
            index,
            pname);
    }

    public async Task<long> GetVertexAttribOffsetAsync(uint index, VertexAttributePointer pname)
    {
        return await this.CallMethodAsync<long>(method: GET_VERTEX_ATTRIB_OFFSET,
            index,
            pname);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, long offset)
    {
        this.CallMethod<object>(method: VERTEX_ATTRIB_POINTER,
            index,
            size,
            type,
            normalized,
            stride,
            offset);
    }

    public async Task VertexAttribPointerAsync(uint index, int size, DataType type, bool normalized, int stride,
        long offset)
    {
        await this.BatchCallAsync(name: VERTEX_ATTRIB_POINTER,
            isMethodCall: true,
            index,
            size,
            type,
            normalized,
            stride,
            offset);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Uniform(WebGLUniformLocation location, params float[] value)
    {
        switch (value.Length)
        {
            case 1:
                this.CallMethod<object>(method: UNIFORM + "1fv",
                    location,
                    value);
                break;
            case 2:
                this.CallMethod<object>(method: UNIFORM + "2fv",
                    location,
                    value);
                break;
            case 3:
                this.CallMethod<object>(method: UNIFORM + "3fv",
                    location,
                    value);
                break;
            case 4:
                this.CallMethod<object>(method: UNIFORM + "4fv",
                    location,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    public async Task UniformAsync(WebGLUniformLocation location, params float[] value)
    {
        switch (value.Length)
        {
            case 1:
                await this.BatchCallAsync(name: UNIFORM + "1fv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 2:
                await this.BatchCallAsync(name: UNIFORM + "2fv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 3:
                await this.BatchCallAsync(name: UNIFORM + "3fv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 4:
                await this.BatchCallAsync(name: UNIFORM + "4fv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Uniform(WebGLUniformLocation location, params int[] value)
    {
        switch (value.Length)
        {
            case 1:
                this.CallMethod<object>(method: UNIFORM + "1iv",
                    location,
                    value);
                break;
            case 2:
                this.CallMethod<object>(method: UNIFORM + "2iv",
                    location,
                    value);
                break;
            case 3:
                this.CallMethod<object>(method: UNIFORM + "3iv",
                    location,
                    value);
                break;
            case 4:
                this.CallMethod<object>(method: UNIFORM + "4iv",
                    location,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    public async Task UniformAsync(WebGLUniformLocation location, params int[] value)
    {
        switch (value.Length)
        {
            case 1:
                await this.BatchCallAsync(name: UNIFORM + "1iv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 2:
                await this.BatchCallAsync(name: UNIFORM + "2iv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 3:
                await this.BatchCallAsync(name: UNIFORM + "3iv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            case 4:
                await this.BatchCallAsync(name: UNIFORM + "4iv",
                    isMethodCall: true,
                    location,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void UniformMatrix(WebGLUniformLocation location, bool transpose, float[] value)
    {
        switch (value.Length)
        {
            case 2 * 2:
                this.CallMethod<object>(method: UNIFORM_MATRIX + "2fv",
                    location,
                    transpose,
                    value);
                break;
            case 3 * 3:
                this.CallMethod<object>(method: UNIFORM_MATRIX + "3fv",
                    location,
                    transpose,
                    value);
                break;
            case 4 * 4:
                this.CallMethod<object>(method: UNIFORM_MATRIX + "4fv",
                    location,
                    transpose,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array has incorrect size");
        }
    }

    public async Task UniformMatrixAsync(WebGLUniformLocation location, bool transpose, float[] value)
    {
        switch (value.Length)
        {
            case 2 * 2:
                await this.BatchCallAsync(name: UNIFORM_MATRIX + "2fv",
                    isMethodCall: true,
                    location,
                    transpose,
                    value);
                break;
            case 3 * 3:
                await this.BatchCallAsync(name: UNIFORM_MATRIX + "3fv",
                    isMethodCall: true,
                    location,
                    transpose,
                    value);
                break;
            case 4 * 4:
                await this.BatchCallAsync(name: UNIFORM_MATRIX + "4fv",
                    isMethodCall: true,
                    location,
                    transpose,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array has incorrect size");
        }
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void VertexAttrib(uint index, params float[] value)
    {
        switch (value.Length)
        {
            case 1:
                this.CallMethod<object>(method: VERTEX_ATTRIB + "1fv",
                    index,
                    value);
                break;
            case 2:
                this.CallMethod<object>(method: VERTEX_ATTRIB + "2fv",
                    index,
                    value);
                break;
            case 3:
                this.CallMethod<object>(method: VERTEX_ATTRIB + "3fv",
                    index,
                    value);
                break;
            case 4:
                this.CallMethod<object>(method: VERTEX_ATTRIB + "4fv",
                    index,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    public async Task VertexAttribAsync(uint index, params float[] value)
    {
        switch (value.Length)
        {
            case 1:
                await this.BatchCallAsync(name: VERTEX_ATTRIB + "1fv",
                    isMethodCall: true,
                    index,
                    value);
                break;
            case 2:
                await this.BatchCallAsync(name: VERTEX_ATTRIB + "2fv",
                    isMethodCall: true,
                    index,
                    value);
                break;
            case 3:
                await this.BatchCallAsync(name: VERTEX_ATTRIB + "3fv",
                    isMethodCall: true,
                    index,
                    value);
                break;
            case 4:
                await this.BatchCallAsync(name: VERTEX_ATTRIB + "4fv",
                    isMethodCall: true,
                    index,
                    value);
                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    actualValue: value.Length,
                    message: "Value array is empty or too long");
        }
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DrawArrays(Primitive mode, int first, int count)
    {
        this.CallMethod<object>(method: DRAW_ARRAYS,
            mode,
            first,
            count);
    }

    public async Task DrawArraysAsync(Primitive mode, int first, int count)
    {
        await this.BatchCallAsync(name: DRAW_ARRAYS,
            isMethodCall: true,
            mode,
            first,
            count);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void DrawElements(Primitive mode, int count, DataType type, long offset)
    {
        this.CallMethod<object>(method: DRAW_ELEMENTS,
            mode,
            count,
            type,
            offset);
    }

    public async Task DrawElementsAsync(Primitive mode, int count, DataType type, long offset)
    {
        await this.BatchCallAsync(name: DRAW_ELEMENTS,
            isMethodCall: true,
            mode,
            count,
            type,
            offset);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Finish()
    {
        this.CallMethod<object>(method: FINISH);
    }

    public async Task FinishAsync()
    {
        await this.BatchCallAsync(name: FINISH,
            isMethodCall: true);
    }

    [Obsolete(message: "Use the async version instead, which is already called internally.")]
    public void Flush()
    {
        this.CallMethod<object>(method: FLUSH);
    }

    public async Task FlushAsync()
    {
        await this.BatchCallAsync(name: FLUSH,
            isMethodCall: true);
    }

    private byte[] ConvertToByteArray<T>(T[] arr)
    {
        var byteArr = new byte[arr.Length * Marshal.SizeOf<T>()];
        Buffer.BlockCopy(src: arr,
            srcOffset: 0,
            dst: byteArr,
            dstOffset: 0,
            count: byteArr.Length);
        return byteArr;
    }

    private async Task<int> GetDrawingBufferWidthAsync()
    {
        return await this.GetPropertyAsync<int>(property: DRAWING_BUFFER_WIDTH);
    }

    private async Task<int> GetDrawingBufferHeightAsync()
    {
        return await this.GetPropertyAsync<int>(property: DRAWING_BUFFER_HEIGHT);
    }

    #endregion
}