function canvasHasFocus() {
    if (!window.console.canvas)
        return false;

    const elem = document.querySelector('canvas');

    if (elem === document.activeElement) {
        return true;
    } else {
        return false;
    }
}

function onResize() {
    if (!window.console.canvas)
        return;

    var canvasContainer = document.getElementById('_divCanvas');
    if (canvasContainer) {
        console.canvas.width = canvasContainer.offsetWidth - canvasContainer.offsetLeft;
        console.canvas.height = canvasContainer.offsetHeight - canvasContainer.offsetTop;

        console.instance.invokeMethodAsync('OnResize', console.canvas.width, console.canvas.height);
    }
}

function onFocus() {
    if (!window.console.canvas)
        return;

    console.instance.invokeMethodAsync('OnFocus');
}

function onBeforeUnload() {
    if (!window.console.canvas)
        return;

    console.instance.invokeMethodAsync('OnBeforeUnload');
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
    return window.console.canvas.toDataURL("image/png");
};

window.initConsole = (instance) => {
    var canvasContainer = document.getElementById('_divCanvas'),
        canvases = canvasContainer.getElementsByTagName('canvas') || [];
    window.console = {
        instance: instance,
        canvas: canvases.length ? canvases[0] : null
    };

    if (window.console.canvas) {
        window.console.canvas.onmousemove = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            console.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.console.canvas.onmousedown = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            console.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.console.canvas.onmouseup = (e) => {
            if (!canvasHasFocus)
                return;
            var me = getMouseEvent(e);
            console.instance.invokeMethodAsync('OnCanvasMouse', me);
        };
        window.console.canvas.onmousewheel = (e) => {
            if (!canvasHasFocus)
                return;
            var we = getWheelEvent(e);
            console.instance.invokeMethodAsync('OnCanvasWheel', we);
        };

        window.console.canvas.onkeydown = (e) => {
            if (!canvasHasFocus)
                return;
            var ke = getKeyEvent(e);
            console.instance.invokeMethodAsync('OnCanvasKey', ke);
        };
        window.console.canvas.onkeyup = (e) => {
            if (!canvasHasFocus)
                return;
            var ke = getKeyEvent(e);
            console.instance.invokeMethodAsync('OnCanvasKey', ke);
        };
        window.console.canvas.onblur = (e) => {
            if (!canvasHasFocus)
                return;
            window.console.canvas.focus();
        };
        window.console.canvas.tabIndex = 0;
        window.console.canvas.focus();
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
