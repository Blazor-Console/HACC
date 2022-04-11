using HACC.Enumerations;

namespace HACC.Configuration;

/// <summary>
///     Default values for terminal settings and canvas
/// </summary>
public static class Defaults
{
    public const int MaximumColumns = 1000;
    public const int MaximumRows = 1000;
    public const int InitialTerminalWidth = 640;
    public const int InitialTerminalHeight = 480;
    public const int InitialBufferRows = 25;
    public const int InitialBufferColumns = 80;
    public const int InitialColumns = 80;
    public const int InitialRows = 25;
    public const int CursorSize = 100;
    public const int CursorHeight = 100;
    public const bool CursorVisibility = true;
    public const bool StatusVisibility = true;
    public const bool TitleVisibility = true;
    public const ConsoleColor BackgroundColor = ConsoleColor.Black;
    public const ConsoleColor ForegroundColor = ConsoleColor.White;
    public const CursorType CursorShape = CursorType.Block;
    public const float BeepFrequency = 800.0f;
    public const float BeepDurationMsec = 50.0f;
    public const float BeepVolume = 1.0f;
    public const BeepType BeepType = Enumerations.BeepType.Sine;
    public const int FontSize = 16;
    public const string FontType = "Courier";
}