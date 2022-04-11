using HACC.Models.Drivers;

namespace HACC.Models.EventArgs;

public record NewFrameEventArgs : VirtualConsoleEventArgs
{
    public NewFrameEventArgs(WebConsoleDriver sender) : base(sender: sender)
    {
        throw new NotImplementedException();
    }
}