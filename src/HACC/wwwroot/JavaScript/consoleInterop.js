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

    console.canvas.width = window.innerWidth;
    console.canvas.height = window.innerHeight;

    console.instance.invokeMethodAsync('OnResize', console.canvas.width, console.canvas.height);
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

window.canvasToPng = () =>{
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
        //window.console.canvas.onmousemove = (e) => {
        //    if (!canvasHasFocus)
        //        return;
        //    console.instance.invokeMethodAsync('OnCanvasClick', e);
        //};
        window.console.canvas.onmousedown = (e) => {
            if (!canvasHasFocus)
                return;
            console.instance.invokeMethodAsync('OnCanvasClick', e);
        };
        window.console.canvas.onmouseup = (e) => {
            if (!canvasHasFocus)
                return;
            console.instance.invokeMethodAsync('OnCanvasClick', e);
        };

        window.console.canvas.onkeydown = (e) => {
            if (!canvasHasFocus)
                return;
            console.instance.invokeMethodAsync('OnCanvasKeyDown', e);
        };
        window.console.canvas.onkeyup = (e) => {
            if (!canvasHasFocus)
                return;
            console.instance.invokeMethodAsync('OnCanvasKeyUp', e);
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
