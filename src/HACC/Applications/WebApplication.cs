using HACC.Components;
using HACC.Models;
using HACC.Models.Drivers;
using Terminal.Gui;

namespace HACC.Applications;

public class WebApplication
{
    private readonly bool wait = true;
    public readonly WebConsole WebConsole;

    public readonly WebConsoleDriver WebConsoleDriver;
    public readonly WebMainLoopDriver WebMainLoopDriver;
    private bool _initialized;
    private Application.RunState? state;

    public WebApplication(WebConsoleDriver webConsoleDriver,
        WebMainLoopDriver webMainLoopDriver, WebConsole webConsole)
    {
        this.WebConsoleDriver = webConsoleDriver;
        // TODO: we should be able to implement something that reads from the actual key events set up in WebConsole.razor for key press events on the console
        // Maybe from the Canvas2DContext StdIn
        this.WebMainLoopDriver = webMainLoopDriver;
        this.WebConsole = webConsole;
        this.WebConsole.RunIterationNeeded += this.WebConsole_RunIterationNeeded;
    }

    private void WebConsole_RunIterationNeeded()
    {
        var firstIteration = false;
        Application.RunIteration(state: ref this.state,
            wait: this.wait,
            firstIteration: ref firstIteration);
    }

    public virtual void Init()
    {
        if (this._initialized) return;

        Application.Init(
            driver: this.WebConsoleDriver,
            mainLoopDriver: this.WebMainLoopDriver);
        this._initialized = true;
    }

    public virtual void Run(Func<Exception, bool>? errorHandler = null)
    {
        this.Run(view: Application.Top,
            errorHandler: errorHandler);
    }

    public virtual void Run<T>(Func<Exception, bool>? errorHandler = null)
        where T : Toplevel, new()
    {
        if (this._initialized && Application.Driver != null)
        {
            var top = new T();
            if (top.GetType().BaseType != typeof(Toplevel))
                throw new ArgumentException(message: top.GetType().BaseType?.Name);
            this.Run(view: top,
                errorHandler: errorHandler);
        }
        else
        {
            this.Init();
            this.Run(view: Application.Top,
                errorHandler: errorHandler);
        }
    }

    public virtual void Run(Toplevel view, Func<Exception, bool>? errorHandler = null)
    {
        try
        {
            if (!this._initialized) this.Init();

            _ = Task.Run(function: () => this.state = Application.Begin(toplevel: view ?? Application.Top));
            _ = Task.Run(function: () => this.WebConsoleDriver.firstRender = false);
            _ = Task.Run(action: Application.Refresh);
            var firstIteration = true;
            _ = Task.Run(action: () => Application.RunIteration(state: ref this.state,
                wait: this.wait,
                firstIteration: ref firstIteration));
        }
        catch (Exception error) when (errorHandler != null)
        {
            if (!errorHandler(arg: error)) this.Shutdown();
        }
    }

    public virtual void End(Application.RunState? runState = null)
    {
        if (!this._initialized) return;

        if (runState != null || this.state != null)
            Application.End(runState: runState ?? this.state);
    }

    public virtual void Shutdown()
    {
        Application.Shutdown();
        this._initialized = false;
    }
}