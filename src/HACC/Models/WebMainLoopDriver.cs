using HACC.Components;
using HACC.Models.Structs;
using Terminal.Gui;

namespace HACC.Models;

//
// MainLoop.cs: IMainLoopDriver and MainLoop for Terminal.Gui
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//

/// <summary>
///     Simple main loop implementation that can be used to monitor
///     file descriptor, run timers and idle handlers.
/// </summary>
/// <remarks>
///     Monitoring of file descriptors is only available on Unix, there
///     does not seem to be a way of supporting this on Windows.
/// </remarks>
public class WebMainLoopDriver : IMainLoopDriver
{
    private readonly Queue<WebInputResult> _inputResult = new();
    private readonly WebConsole _webConsole;
    private MainLoop? _mainLoop;

    /// <summary>
    ///     Invoked when a Key is pressed, mouse is clicked or on resizing.
    /// </summary>
    public Action<WebInputResult>? ProcessInput;


    /// <summary>
    ///     Creates a new Mainloop.
    /// </summary>
    /// <param name="webConsole"></param>
    public WebMainLoopDriver(WebConsole webConsole)
    {
        this._webConsole = webConsole ??
                           throw new ArgumentNullException(paramName: "Console driver instance must be provided.");
    }

    void IMainLoopDriver.Setup(MainLoop mainLoop)
    {
        this._mainLoop = mainLoop ?? throw new ArgumentException(message: "MainLoop must be provided");
        this._webConsole.ReadConsoleInput += this.WebConsole_ReadConsoleInput;
    }

    void IMainLoopDriver.Wakeup()
    {
        this._webConsole.OnWakeup();
    }

    bool IMainLoopDriver.EventsPending(bool wait)
    {
        //return this._inputResult.Count > 0 || this.CheckTimers(wait: wait,
        //    waitTimeout: out _);
        return true;
    }

    void IMainLoopDriver.MainIteration()
    {
        while (this._inputResult.Count > 0) this.ProcessInput?.Invoke(obj: this._inputResult.Dequeue());
    }

    private Task WebConsole_ReadConsoleInput(WebInputResult obj)
    {
        this._inputResult.Enqueue(item: obj);
        return Task.CompletedTask;
    }

    private bool CheckTimers(bool wait, out int waitTimeout)
    {
        var now = DateTime.UtcNow.Ticks;

        if (this._mainLoop!.Timeouts.Count > 0)
        {
            waitTimeout = (int) ((this._mainLoop.Timeouts.Keys[index: 0] - now) / TimeSpan.TicksPerMillisecond);
            if (waitTimeout < 0)
                return true;
        }
        else
        {
            waitTimeout = -1;
        }

        if (!wait)
            waitTimeout = 0;

        int ic;
        lock (this._mainLoop.IdleHandlers)
        {
            ic = this._mainLoop.IdleHandlers.Count;
        }

        return ic > 0;
    }
}