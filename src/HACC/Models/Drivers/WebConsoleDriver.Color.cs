using HACC.Configuration;

namespace HACC.Models.Drivers;

//
// Summary:
//     Represents the standard input, output, and error streams for console applications.
//     This class cannot be inherited.
public partial class WebConsoleDriver
{
    //
    // Summary:
    //     Gets or sets the background color of the console.
    //
    // Returns:
    //     A value that specifies the background color of the console; that is, the color
    //     that appears behind each character. The default is black.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The color specified in a set operation is not a valid member of System.ConsoleColor.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public ConsoleColor BackgroundColor
    {
        get => this.TerminalSettings.TerminalBackground;
        set => this.TerminalSettings.TerminalBackground = value;
    }

    //
    // Summary:
    //     Gets or sets the foreground color of the console.
    //
    // Returns:
    //     A System.ConsoleColor that specifies the foreground color of the console; that
    //     is, the color of each character that is displayed. The default is gray.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     The color specified in a set operation is not a valid member of System.ConsoleColor.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public ConsoleColor ForegroundColor
    {
        get => this.TerminalSettings.TerminalForeground;
        set => this.TerminalSettings.TerminalForeground = value;
    }

    //
    // Summary:
    //     Sets the foreground and background console colors to their defaults.
    //
    // Exceptions:
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void ResetColor()
    {
        this.ForegroundColor = Defaults.ForegroundColor;
        this.BackgroundColor = Defaults.BackgroundColor;
    }
}