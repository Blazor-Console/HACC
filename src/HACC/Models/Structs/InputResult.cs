using HACC.Models.Enums;

namespace HACC.Models.Structs;

public struct InputResult
{
    public EventType EventType;
    public WebKeyEvent KeyEvent;
    public WebMouseEvent MouseEvent;
    public ResizeEvent ResizeEvent;
}