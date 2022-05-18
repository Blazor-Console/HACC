function canvasHasFocus() {
    if (!window.consoleJs.canvas)
        return false;

    const elem = document.querySelector('canvas');

    if (elem === document.activeElement) {
        return true;
    } else {
        return false;
    }
}

function onResize() {
    if (!window.consoleJs.canvas)
        return;

    var canvasContainer = document.getElementById('_divCanvas');
    if (canvasContainer) {
        window.consoleJs.canvas.width = canvasContainer.offsetWidth - canvasContainer.offsetLeft;
        window.consoleJs.canvas.height = canvasContainer.offsetHeight - canvasContainer.offsetTop;

        window.consoleJs.instance.invokeMethodAsync('OnResize', window.consoleJs.canvas.width, window.consoleJs.canvas.height);
    }
}

function onFocus() {
    if (!window.consoleJs.canvas)
        return;

    window.consoleJs.instance.invokeMethodAsync('OnFocus');
}

function onBeforeUnload() {
    if (!window.consoleJs.canvas)
        return;

    window.consoleJs.instance.invokeMethodAsync('OnBeforeUnload');
}

window.consoleWindowResize = (instance) => {
    onResize();
};

window.consoleWindowFocus = (instance) => {
    onFocus();
}

window.consoleWindowBeforeUnload = (instance) => {
    onBeforeUnload();
}

window.canvasToPng = () => {
    return window.consoleJs.canvas.toDataURL("image/png");
};

window.initConsole = (instance) => {
    var canvasContainer = document.getElementById('_divCanvas'),
        canvases = canvasContainer.getElementsByTagName('canvas') || [];
    canvasContainer.oncontextmenu = function () {
        return false;
    }
    window.consoleJs = {
        instance: instance,
        canvas: canvases.length ? canvases[0] : null
    };

    if (window.consoleJs.canvas) {
        window.consoleJs.canvas.onmousemove = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.consoleJs.canvas.onmousedown = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.consoleJs.canvas.onmouseup = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.consoleJs.canvas.onmousewheel = (e) => {
            if (!canvasHasFocus)
                return;
            var we = getWheelEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasWheel', we);
        };

        window.consoleJs.canvas.onkeydown = (e) => {
            if (!canvasHasFocus)
                return;
            var ke = getKeyEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasKey', ke);
        };
        window.consoleJs.canvas.onkeyup = (e) => {
            if (!canvasHasFocus)
                return;
            var ke = getKeyEvent(e);
            window.consoleJs.instance.invokeMethodAsync('OnCanvasKey', ke);
        };
        window.consoleJs.canvas.onblur = (e) => {
            if (!canvasHasFocus)
                return;
            window.consoleJs.canvas.focus();
        };
        window.consoleJs.canvas.tabIndex = 0;
        window.consoleJs.canvas.focus();
    }

    window.addEventListener("resize", onResize);
    window.addEventListener("focus", onFocus);
    window.addEventListener("beforeunload", onBeforeUnload);
};

function getKeyEvent(e) {
    var ke = {};
    ke.AltKey = e.altKey;
    ke.Code = e.code;
    ke.CtrlKey = e.ctrlKey;
    ke.Key = e.key;
    ke.Location = e.location;
    ke.MetaKey = e.metaKey;
    ke.Repeat = e.repeat;
    ke.ShiftKey = e.shiftKey;
    ke.Type = e.type;
    return ke;
}

function getWheelEvent(e) {
    var we = {};
    we.AltKey = e.altKey;
    we.Button = e.button;
    we.Buttons = e.buttons;
    we.ClientX = e.clientX;
    we.ClientY = e.clientY;
    we.CtrlKey = e.ctrlKey;
    we.DeltaMode = e.deltaMode;
    we.DeltaX = e.deltaX;
    we.DeltaY = e.deltaY;
    we.DeltaZ = e.deltaZ;
    we.Detail = e.detail;
    we.MetaKey = e.metaKey;
    we.OffsetX = e.offsetX;
    we.OffsetY = e.offsetY;
    we.PageX = e.pageX;
    we.PageY = e.pageY;
    we.ScreenX = e.screenX;
    we.ScreenY = e.screenY;
    we.ShiftKey = e.shiftKey;
    we.Type = e.type;
    return we;
}

function getMouseEvent(e) {
    var me = {};
    me.AltKey = e.altKey;
    me.Button = e.button;
    me.Buttons = e.buttons;
    me.ClientX = e.clientX;
    me.ClientY = e.clientY;
    me.CtrlKey = e.ctrlKey;
    me.Detail = e.detail;
    me.MetaKey = e.metaKey;
    me.OffsetX = e.offsetX;
    me.OffsetY = e.offsetY;
    me.PageX = e.pageX;
    me.PageY = e.pageY;
    me.ScreenX = e.screenX;
    me.ScreenY = e.screenY;
    me.ShiftKey = e.shiftKey;
    me.Type = e.type;
    return me;
}
