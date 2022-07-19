using HACC.Components;
using HACC.Models;
using HACC.Models.Drivers;
using Terminal.Gui;

namespace HACC.Applications;

public class WebApplication : IAsyncDisposable
{
    private readonly bool _wait = true;
    private bool _initialized;
    private Application.RunState? _currentRunState;
    private List<Application.RunState> _runStates = new();
    private Timer? _timer;
    private Application.RunState? _stoppingRunState;
    private bool _running;
    private CancellationTokenSource? _cancellationTokenSource;
    private  CancellationToken _cancellationToken;
    private readonly List<IDisposable> _disposables = new();

    public WebConsole? WebConsole { get; private set; }
    public WebConsoleDriver? WebConsoleDriver { get; private set; }
    public WebMainLoopDriver? WebMainLoopDriver { get; private set; }

    public event Action<Toplevel>? RunStateEnding;

    public virtual async Task Init(ConsoleDriver? webConsoleDriver = null,
        IMainLoopDriver? webMainLoopDriver = null, WebConsole? webConsole = null)
    {
        if (this._initialized) return;

        if (webConsoleDriver == null)
            throw new ArgumentNullException(nameof(webConsoleDriver));
        if (webMainLoopDriver == null)
            throw new ArgumentNullException(nameof(webMainLoopDriver));
        if (webConsole == null)
            throw new ArgumentNullException(nameof(webConsole));
        this.WebConsoleDriver = (WebConsoleDriver?) webConsoleDriver;
        this.WebMainLoopDriver = (WebMainLoopDriver?) webMainLoopDriver;
        this.WebConsole = webConsole;
        Application.ExitRunLoopAfterFirstIteration = true;
        await Task.Run(() => Application.Init(
            driver: this.WebConsoleDriver,
            mainLoopDriver: this.WebMainLoopDriver));

        this.WebConsole.RunIterationNeeded += this.WebConsole_RunIterationNeeded;
        this._cancellationTokenSource = new CancellationTokenSource();
        this._cancellationToken = this._cancellationTokenSource.Token;
        this._cancellationToken.ThrowIfCancellationRequested();
        Application.NotifyNewRunState += this.Application_NotifyNewRunState;
        Application.NotifyStopRunState += this.Application_NotifyStopRunState;
        //Application.MainLoop.TimeoutAdded += this.MainLoop_TimeoutAdded;
        this._initialized = true;
        this._timer = new Timer((_) => this.ProcessTimer(), null, 1000, 1000);
    }

    private void ProcessTimer()
    {
        if (this._running)
            return;

        try
        {
            if (Application.MainLoop.Timeouts.Count > 0)
            {
                var now = DateTime.UtcNow.Ticks;
                var waitTimeout = (int) ((Application.MainLoop.Timeouts.Keys[index: 0] - now) / TimeSpan.TicksPerMillisecond);
                if (waitTimeout < 0)
                    this.WebConsole!.OnTimeout();
            }
            else
            {
                this.WebConsoleDriver!.UpdateCursor();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProcessTimer: {ex.Message}");
        }
    }

    //private void MainLoop_TimeoutAdded(long obj)
    //{
    //    var now = DateTime.UtcNow.Ticks;
    //    var waitTimeout = Math.Max((int) ((obj - now) / TimeSpan.TicksPerMillisecond), 0);
    //_timer = new Timer(_ => this.WebConsole.OnTimeout(),
    //    null, waitTimeout, Timeout.Infinite);
    //}

    private async Task WebConsole_RunIterationNeeded()
    {
        if (this._currentRunState == null) return;

        this._running = true;
        var firstIteration = false;
        await Task.Run(() => Application.RunMainLoopIteration(state: ref this._currentRunState,
            wait: this._wait,
            firstIteration: ref firstIteration));
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
    }

    private void Application_NotifyStopRunState(Toplevel obj)
    {
        this._stoppingRunState = new Application.RunState(obj);
        this.RunStateEnding?.Invoke(obj);
    }

    private void Application_NotifyNewRunState(Application.RunState obj)
    {
        this._runStates.Add(new Application.RunState(obj.Toplevel));
        View view = obj.Toplevel;
        var task = Task.Run(async () => await this.RunLoop(obj));
        task.ContinueWith(async _ =>
        {
            this._disposables.Remove(task);
            await this.WebConsole_RunIterationNeeded();
        });
        this._disposables.Add(task);
        task.Start();
    } // this line is never hit

    private async Task RunLoop(Application.RunState runToken)
    {
        if (!this._initialized) await this.Init();

        var view = runToken.Toplevel;
        var firstIteration = true;
        this._currentRunState = runToken;
        this.WebConsole!.OnReadConsoleInput();

        try
        {
            for (view.Running = true; view.Running;)
            {
                if (firstIteration)
                    Application.RunMainLoopIteration(state: ref this._currentRunState,
                        wait: this._wait,
                        firstIteration: ref firstIteration);

                await Task.Delay(1000);
            }
        }
        catch (OperationCanceledException)
        {
            await this.Shutdown();
            return;
        }
    }

    public virtual async Task Run(Func<Exception, bool>? errorHandler = null)
    {
        await this.Run(view: Application.Top,
            errorHandler: errorHandler);
    }

    public virtual async Task Run<T>(Func<Exception, bool>? errorHandler = null)
        where T : Toplevel, new()
    {
        if (this._initialized && Application.Driver != null)
        {
            var top = new T();
            if (top.GetType().BaseType != typeof(Toplevel))
                throw new ArgumentException(message: top.GetType().BaseType?.Name);
            await this.Run(view: top,
                errorHandler: errorHandler);
        }
        else
        {
            await this.Init();
            await this.Run(view: Application.Top,
                errorHandler: errorHandler);
        }
    }

    public virtual async Task Run(Toplevel view, Func<Exception, bool>? errorHandler = null)
    {
        try
        {
            if (!this._initialized) await this.Init();

            var runToken = Application.Begin(toplevel: view ?? Application.Top);
            this._runStates.Add(new Application.RunState(runToken.Toplevel));
            var firstIteration = true;
            this._currentRunState = runToken;
            this.WebConsole!.OnReadConsoleInput();

            try
            {
                for (view!.Running = true; view.Running;)
                {
                    if (firstIteration)
                    {
                        Application.RunMainLoopIteration(state: ref this._currentRunState,
                            wait: this._wait,
                            firstIteration: ref firstIteration);
                    }

                    await Task.Delay(1000, this._cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            if (!this._cancellationToken.IsCancellationRequested)
            {
                if (view == Application.Top)
                {
                    await this.End(new Application.RunState(view));
                    await this.Shutdown();
                    await this.DisposeAsync();
                }
                return;
            }
        }
        catch (Exception error) when (errorHandler != null)
        {
            if (!errorHandler(arg: error)) await this.Shutdown();
        }
    }

    public virtual async Task End(Application.RunState? runState = null)
    {
        if (!this._initialized) return;

        if (runState != null || this._currentRunState != null)
            await Task.Run(() => Application.End(runState: runState ?? this._currentRunState));
    }

    public virtual Task Shutdown()
    {
        return Task.Run(() =>
        {
            if (this._timer != null)
                _ = this._timer!.DisposeAsync();
            Application.Shutdown();
            this._initialized = false;
        });
    }

    public async ValueTask DisposeAsync()
    {
        await this._timer!.DisposeAsync();
        this._timer = null;
        this._cancellationTokenSource!.Cancel();
        this._cancellationTokenSource.Dispose();
        this._cancellationTokenSource = null;
        this.RunStateEnding = null;
        this._currentRunState = null;
        this._runStates = new();
        this._stoppingRunState = null;
        GC.SuppressFinalize(this);
    }
}