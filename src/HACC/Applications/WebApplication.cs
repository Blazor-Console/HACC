using HACC.Components;
using HACC.Models;
using HACC.Models.Drivers;
using Terminal.Gui;

namespace HACC.Applications;

public class WebApplication : IDisposable
{
    private readonly bool _wait = true;
    private bool _initialized;
    private Application.RunState? _currentRunState;
    private List<Application.RunState> _runStates = new List<Application.RunState>();
    private Timer? _timer;
    private Application.RunState? _stoppingRunState;
    private bool _running;

    public readonly WebConsole WebConsole;
    public readonly WebConsoleDriver WebConsoleDriver;
    public readonly WebMainLoopDriver WebMainLoopDriver;

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

    private Task WebConsole_RunIterationNeeded()
    {
        if (this._currentRunState == null) return Task.CompletedTask;

        this._running = true;
        var firstIteration = false;
        Application.RunMainLoopIteration(state: ref this._currentRunState,
            wait: this._wait,
            firstIteration: ref firstIteration);
        if (this._currentRunState.Toplevel != Application.Top && (!this._currentRunState.Toplevel.Running
            || this._stoppingRunState != null))
        {
            Application.RunState? runState;
            if (this._stoppingRunState != null && this._stoppingRunState.Toplevel != this._currentRunState.Toplevel)
            {
                runState = this._runStates.Find(rs => rs.Toplevel == this._stoppingRunState.Toplevel);
            }
            else
            {
                runState = this._currentRunState;
            }
            Application.End(runState);
            this._runStates.Remove(runState!);
            this._stoppingRunState = null;
            if (this._currentRunState.Toplevel != Application.Current)
            {
                this._currentRunState = new Application.RunState(Application.Current);
            }
        }
        this._running = false;

        return Task.CompletedTask;
    }

    public virtual Task Init()
    {
        if (this._initialized) return Task.CompletedTask;

        Application.ExitRunLoopAfterFirstIteration = true;
        Application.Init(
            driver: this.WebConsoleDriver,
            mainLoopDriver: this.WebMainLoopDriver);
        Application.NotifyNewRunState += this.Application_NotifyNewRunState;
        Application.NotifyStopRunState += this.Application_NotifyStopRunState;
        //Application.MainLoop.TimeoutAdded += this.MainLoop_TimeoutAdded;
        this._initialized = true;
        _timer = new Timer((_) => this.ProcessTimer(), null, 1000, 1000);
        return Task.CompletedTask;
    }

    private void ProcessTimer()
    {
        if (this._running)
            return;

        if (Application.MainLoop.Timeouts.Count > 0)
        {
            var now = DateTime.UtcNow.Ticks;
            var waitTimeout = (int) ((Application.MainLoop.Timeouts.Keys[index: 0] - now) / TimeSpan.TicksPerMillisecond);
            if (waitTimeout < 0)
                this.WebConsole.OnTimeout();
        }
        else
        {
            this.WebConsoleDriver.UpdateCursor();
        }
    }

    //private void MainLoop_TimeoutAdded(long obj)
    //{
    //    var now = DateTime.UtcNow.Ticks;
    //    var waitTimeout = Math.Max((int) ((obj - now) / TimeSpan.TicksPerMillisecond), 0);
    //_timer = new Timer(_ => this.WebConsole.OnTimeout(),
    //    null, waitTimeout, Timeout.Infinite);
    //}

    private void Application_NotifyStopRunState(Toplevel obj)
    {
        this._stoppingRunState = new Application.RunState(obj);
    }

    private void Application_NotifyNewRunState(Application.RunState obj)
    {
        this._runStates.Add(new Application.RunState(obj.Toplevel));
    }

    public virtual Task Run(Func<Exception, bool>? errorHandler = null)
    {
        this.Run(view: Application.Top,
            errorHandler: errorHandler);
        return Task.CompletedTask;
    }

    public virtual Task Run<T>(Func<Exception, bool>? errorHandler = null)
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
        return Task.CompletedTask;
    }

    public virtual Task Run(Toplevel view, Func<Exception, bool>? errorHandler = null)
    {
        try
        {
            if (!this._initialized) this.Init();

            this._currentRunState = Application.Begin(toplevel: view ?? Application.Top);
            this._currentRunState.Toplevel.Running = true;
            this._runStates.Add(new Application.RunState(this._currentRunState.Toplevel));
            var firstIteration = true;
            Application.RunMainLoopIteration(state: ref this._currentRunState,
                wait: this._wait,
                firstIteration: ref firstIteration);
        }
        catch (Exception error) when (errorHandler != null)
        {
            if (!errorHandler(arg: error)) this.Shutdown();
        }
        return Task.CompletedTask;
    }

    public virtual Task End(Application.RunState? runState = null)
    {
        if (!this._initialized) return Task.CompletedTask;

        if (runState != null || this._currentRunState != null)
            Application.End(runState: runState ?? this._currentRunState);
        return Task.CompletedTask;
    }

    public virtual Task Shutdown()
    {
        Application.Shutdown();
        this.Dispose();
        this._initialized = false;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        this._timer?.Dispose();
        _timer = null;
    }
}