using HACC.Components;
using HACC.Extensions;
using HACC.Models.Spectre;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;
using Terminal.Gui;

namespace HACC.Models.Drivers;

/// <summary>
///     Represents the standard input, output, and error streams for console applications.
///     This class cannot be inherited.
/// </summary>
public sealed partial class WebConsoleDriver : ConsoleDriver, IAnsiConsole
{
    private readonly WebConsole _webConsole;

    public readonly ILogger<WebConsoleDriver> Logger;

    /// <summary>
    ///     Initializes a web console driver.
    /// </summary>
    /// <param name="webClipboard"></param>
    /// <param name="webConsole"></param>
    public WebConsoleDriver(WebClipboard webClipboard, WebConsole webConsole)
    {
        this.Logger = HaccExtensions.CreateLogger<WebConsoleDriver>();
        this.Clipboard = webClipboard;
        this.ExclusivityMode = new BrowserExclusivityMode();
        this._webConsole = webConsole;
        this.TerminalSettings = new TerminalSettings();
        this.Contents = new int[this.BufferRows, this.BufferColumns, 3];
        this._dirtyLine = new bool[this.BufferRows];
    }

    // TODO: resize, etc if terminal settings updated
    public TerminalSettings TerminalSettings { get; private set; }

    public Profile Profile => throw new NotImplementedException();

    public IExclusivityMode ExclusivityMode { get; }
    public RenderPipeline Pipeline => throw new NotImplementedException();
}