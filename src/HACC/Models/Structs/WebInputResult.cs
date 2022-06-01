using HACC.Models.Enums;

namespace HACC.Models.Structs;

public struct WebInputResult
{
    public WebEventType EventType;
    public WebKeyEvent KeyEvent;
    public WebMouseEvent MouseEvent;
    public WebResizeEvent ResizeEvent;
}