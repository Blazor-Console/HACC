namespace HACC.Models.Enums;

[Flags]
public enum MouseButtonState
{
    Button1Pressed = 0x1,
    Button1Released = 0x2,
    Button1Clicked = 0x4,
    Button1DoubleClicked = 0x8,
    Button1TripleClicked = 0x10,
    Button2Pressed = 0x20,
    Button2Released = 0x40,
    Button2Clicked = 0x80,
    Button2DoubleClicked = 0x100,
    Button2TrippleClicked = 0x200,
    Button3Pressed = 0x400,
    Button3Released = 0x800,
    Button3Clicked = 0x1000,
    Button3DoubleClicked = 0x2000,
    Button3TripleClicked = 0x4000,
    ButtonWheeledUp = 0x8000,
    ButtonWheeledDown = 0x10000,
    ButtonWheeledLeft = 0x20000,
    ButtonWheeledRight = 0x40000,
    Button4Pressed = 0x80000,
    Button4Released = 0x100000,
    Button4Clicked = 0x200000,
    Button4DoubleClicked = 0x400000,
    Button4TripleClicked = 0x800000,
    ButtonShift = 0x1000000,
    ButtonCtrl = 0x2000000,
    ButtonAlt = 0x4000000,
    ReportMousePosition = 0x8000000,

    AllEvents = Button1Pressed | Button1Released | Button1Clicked | Button1DoubleClicked | Button1TripleClicked |
                Button2Pressed | Button2Released | Button2Clicked | Button2DoubleClicked | Button2TrippleClicked |
                Button3Pressed | Button3Released | Button3Clicked | Button3DoubleClicked | Button3TripleClicked |
                ButtonWheeledUp | ButtonWheeledDown | ButtonWheeledLeft | ButtonWheeledRight | Button4Pressed |
                Button4Released | Button4Clicked | Button4DoubleClicked | Button4TripleClicked | ReportMousePosition,
}