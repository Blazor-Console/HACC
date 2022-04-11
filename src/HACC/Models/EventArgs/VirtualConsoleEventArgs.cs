using HACC.Models.Drivers;

namespace HACC.Models.EventArgs;

public record VirtualConsoleEventArgs
{
    public readonly WebConsoleDriver ConsoleDriver;

    public VirtualConsoleEventArgs(WebConsoleDriver sender)
    {
        this.ConsoleDriver = sender;
    }
}