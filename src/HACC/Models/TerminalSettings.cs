using HACC.Configuration;
using HACC.Enumerations;
using System.Drawing;

namespace HACC.Models;

public record TerminalSettings
{
    /// <summary>
    ///     Terminal window width in characters
    /// </summary>
    public int BufferColumns;

    /// <summary>
    ///     Terminal window height in characters
    /// </summary>
    public int BufferRows;

    /// <summary>
    ///     Cursor height in percentage of character
    /// </summary>
    public int CursorHeight;

    /// <summary>
    ///     Cursor position in characters
    /// </summary>
    public Point CursorPosition;

    /// <summary>
    ///     Summary:
    ///     Gets or sets the height of the cursor within a character cell.
    ///     Returns:
    ///     The size of the cursor expressed as a percentage of the height of a character
    ///     cell. The property value ranges from 1 to 100.
    /// </summary>
    public int CursorSize;

    /// <summary>
    ///     Cursor display shape/type
    /// </summary>
    public CursorType CursorType;

    /// <summary>
    ///     Whether cursor is visible
    /// </summary>
    public bool CursorVisible;

    /// <summary>
    ///     Terminal font size in pixels
    /// </summary>
    public int FontSizePixels;

    /// <summary>
    ///     Terminal font type
    /// </summary>
    public string FontType;

    /// <summary>
    ///     Whether the status bar is visible
    /// </summary>
    public bool StatusVisible;

    /// <summary>
    ///     Terminal default background color
    /// </summary>
    public ConsoleColor TerminalBackground;

    /// <summary>
    ///     Terminal default foreground color
    /// </summary>
    public ConsoleColor TerminalForeground;

    /// <summary>
    ///     Window/Terminal title
    /// </summary>
    public string Title;

    /// <summary>
    ///     Whether the title bar is visible
    /// </summary>
    public bool TitleVisible;

    /// <summary>
    ///     Terminal window width in characters
    /// </summary>
    public int WindowColumns;

    /// <summary>
    ///     Terminal window height in pixels
    /// </summary>
    public int WindowHeightPixels;

    /// <summary>
    ///     Terminal window height in characters
    /// </summary>
    public int WindowRows;

    /// <summary>
    ///     Terminal window width in pixels
    /// </summary>
    public int WindowWidthPixels;

    public TerminalSettings(
        string title = "",
        int windowWidthPixels = Defaults.InitialTerminalWidth,
        int windowHeightPixels = Defaults.InitialTerminalHeight,
        int bufferColumns = Defaults.InitialBufferColumns,
        int bufferRows = Defaults.InitialBufferRows,
        int windowColumns = Defaults.InitialColumns,
        int windowRows = Defaults.InitialRows,
        bool cursorVisible = Defaults.CursorVisibility,
        bool statusVisible = Defaults.StatusVisibility,
        bool titleVisible = Defaults.TitleVisibility,
        Point? cursorPosition = null,
        CursorType cursorType = Defaults.CursorShape,
        int cursorHeight = Defaults.CursorHeight,
        int cursorSize = Defaults.CursorSize,
        ConsoleColor terminalBackground = Defaults.BackgroundColor,
        ConsoleColor terminalForeground = Defaults.ForegroundColor,
        int fontSizePixels = Defaults.FontSize,
        string fontType = Defaults.FontType)
    {
        this.Title = title;
        this.WindowWidthPixels = windowWidthPixels;
        this.WindowHeightPixels = windowHeightPixels;
        this.BufferColumns = bufferColumns;
        this.BufferRows = bufferRows;
        this.WindowColumns = windowColumns;
        this.WindowRows = windowRows;
        this.CursorVisible = cursorVisible;
        this.StatusVisible = statusVisible;
        this.TitleVisible = titleVisible;
        this.CursorPosition = cursorPosition ?? new Point(
            x: 0,
            y: 0);
        this.CursorType = cursorType;
        this.CursorHeight = cursorHeight;
        this.CursorSize = cursorSize;
        this.TerminalBackground = terminalBackground;
        this.TerminalForeground = terminalForeground;
        this.FontSizePixels = fontSizePixels;
        this.FontType = fontType;
    }

    public void SetCursorPosition(int x, int y)
    {
        this.CursorPosition = new Point(x: x,
            y: y);
    }
}