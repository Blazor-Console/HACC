using HACC.Enumerations;
using HACC.Models.Enums;
using HACC.Models.Structs;
using NStack;
using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;
using MouseEvent = Terminal.Gui.MouseEvent;

namespace HACC.Models.Drivers;

public partial class WebConsoleDriver
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    ///     Shortcut to <see cref="WebConsoleDriver.WindowColumns" />
    /// </summary>
    public override int Cols => this.WindowColumns;

    /// <summary>
    ///     Shortcut to <see cref="WebConsoleDriver.WindowRows" />
    /// </summary>
    public override int Rows => this.WindowRows;

    /// <summary>
    ///     Shortcut to <see cref="WebConsoleDriver.WindowLeft" />
    ///     Only handling left here because not all terminals has a horizontal scroll bar.
    /// </summary>
    public override int Left => this.WindowLeft;

    /// <summary>
    ///     Shortcut to <see cref="WebConsoleDriver.WindowTop" />
    /// </summary>
    public override int Top => this.WindowTop;

    /// <summary>
    ///     If false height is measured by the window height and thus no scrolling.
    ///     If true then height is measured by the buffer height, enabling scrolling.
    ///     The current <see cref="ConsoleDriver.HeightAsBuffer" /> used in the terminal.
    /// </summary>
    public override bool HeightAsBuffer { get; set; }

    public override WebClipboard Clipboard { get; }

    // The format is rows, columns and 3 values on the last column: Rune, Attribute and Dirty Flag
    private int[,,] contents;
    private bool[] _dirtyLine;

    /// <summary>
    ///     Assists with testing, the format is rows, columns and 3 values on the last column: Rune, Attribute and Dirty Flag
    /// </summary>
    public override int[,,] Contents => this.contents;

    internal bool firstRender = true;

    private static readonly bool sync = false;

    // Current row, and current col, tracked by Move/AddRune only
    private int ccol, crow;

    //private bool _needMove;

    public override void Move(int col, int row)
    {
        this.ccol = col;
        this.crow = row;
    }

    public override void AddRune(Rune rune)
    {
        if (this.contents.Length != this.Rows * this.Cols * 3) return;
        rune = MakePrintable(c: rune);
        var runeWidth = Rune.ColumnWidth(rune: rune);
        var validClip = IsValidContent(col: ccol, row: crow, clip: Clip);

        if (validClip)
        {
            if (runeWidth < 2 && this.ccol > 0
                && Rune.ColumnWidth((char) this.contents[this.crow, this.ccol - 1, (int) RuneDataType.Rune]) > 1)
            {

                this.contents[this.crow, this.ccol - 1, (int) RuneDataType.Rune] = (int) (uint) ' ';

            }
            else if (runeWidth < 2 && this.ccol <= this.Clip.Right - 1
              && Rune.ColumnWidth((char) this.contents[this.crow, this.ccol, (int) RuneDataType.Rune]) > 1)
            {

                this.contents[this.crow, this.ccol + 1, (int) RuneDataType.Rune] = (int) (uint) ' ';
                this.contents[this.crow, this.ccol + 1, (int) RuneDataType.DirtyFlag] = 1;
            }
            if (runeWidth > 1 && this.ccol == this.Clip.Right - 1)
                this.contents[this.crow,
                this.ccol,
                (int) RuneDataType.Rune] = (int) (uint) ' ';
            else
                this.contents[this.crow,
                this.ccol,
                (int) RuneDataType.Rune] = (int) (uint) rune;
            this.contents[this.crow,
                this.ccol,
                (int) RuneDataType.Attribute] = this._currentAttribute;
            this.contents[this.crow,
                this.ccol,
                (int) RuneDataType.DirtyFlag] = 1;
            this._dirtyLine[this.crow] = true;
        }

        this.ccol++;
        if (runeWidth > 1)
        {
            if (validClip && this.ccol < this.Clip.Right)
            {
                this.contents[this.crow,
                    this.ccol,
                    (int) RuneDataType.Attribute] = this._currentAttribute;
                this.contents[this.crow,
                    this.ccol,
                    (int) RuneDataType.DirtyFlag] = 0;
            }
            this.ccol++;
        }

        //if (ccol == Cols) {
        //	ccol = 0;
        //	if (crow + 1 < WindowRows)
        //		crow++;
        //}
        if (sync) this.UpdateScreen();
    }

    public override void AddStr(ustring str)
    {
        foreach (var rune in str) this.AddRune(rune: rune);
    }

    public override void End()
    {
        this.ResetColor();
        this.Clear();
    }

    private static Attribute MakeColor(ConsoleColor f, ConsoleColor b)
    {
        // Encode the colors into the int value.
        return new Attribute(
            value: (((int) f & 0xffff) << 16) | ((int) b & 0xffff),
            foreground: (Color) f,
            background: (Color) b
        );
    }

    public override void Init(Action terminalResized)
    {
        this.TerminalSettings = new TerminalSettings();
        this.TerminalResized = terminalResized;
        this.Clear();
        this.ResizeScreen();
        this.UpdateOffScreen();

        Colors.TopLevel = new ColorScheme();
        Colors.Base = new ColorScheme();
        Colors.Dialog = new ColorScheme();
        Colors.Menu = new ColorScheme();
        Colors.Error = new ColorScheme();
        this.Clip = new Rect(x: 0,
            y: 0,
            width: this.Cols,
            height: this.Rows);

        Colors.TopLevel.Normal = MakeColor(f: ConsoleColor.Green,
            b: ConsoleColor.Black);
        Colors.TopLevel.Focus = MakeColor(f: ConsoleColor.White,
            b: ConsoleColor.DarkCyan);
        Colors.TopLevel.HotNormal = MakeColor(f: ConsoleColor.DarkYellow,
            b: ConsoleColor.Black);
        Colors.TopLevel.HotFocus = MakeColor(f: ConsoleColor.DarkBlue,
            b: ConsoleColor.DarkCyan);
        Colors.TopLevel.Disabled = MakeColor(f: ConsoleColor.DarkGray,
            b: ConsoleColor.Black);

        Colors.Base.Normal = MakeColor(f: ConsoleColor.White,
            b: ConsoleColor.Blue);
        Colors.Base.Focus = MakeColor(f: ConsoleColor.Black,
            b: ConsoleColor.Cyan);
        Colors.Base.HotNormal = MakeColor(f: ConsoleColor.Yellow,
            b: ConsoleColor.Blue);
        Colors.Base.HotFocus = MakeColor(f: ConsoleColor.Yellow,
            b: ConsoleColor.Cyan);
        Colors.Base.Disabled = MakeColor(f: ConsoleColor.DarkGray,
            b: ConsoleColor.DarkBlue);

        // Focused,
        //    Selected, Hot: Yellow on Black
        //    Selected, text: white on black
        //    Unselected, hot: yellow on cyan
        //    unselected, text: same as unfocused
        Colors.Menu.HotFocus = MakeColor(f: ConsoleColor.Yellow,
            b: ConsoleColor.Black);
        Colors.Menu.Focus = MakeColor(f: ConsoleColor.White,
            b: ConsoleColor.Black);
        Colors.Menu.HotNormal = MakeColor(f: ConsoleColor.Yellow,
            b: ConsoleColor.Cyan);
        Colors.Menu.Normal = MakeColor(f: ConsoleColor.White,
            b: ConsoleColor.Cyan);
        Colors.Menu.Disabled = MakeColor(f: ConsoleColor.DarkGray,
            b: ConsoleColor.Cyan);

        Colors.Dialog.Normal = MakeColor(f: ConsoleColor.Black,
            b: ConsoleColor.Gray);
        Colors.Dialog.Focus = MakeColor(f: ConsoleColor.Black,
            b: ConsoleColor.Cyan);
        Colors.Dialog.HotNormal = MakeColor(f: ConsoleColor.Blue,
            b: ConsoleColor.Gray);
        Colors.Dialog.HotFocus = MakeColor(f: ConsoleColor.Blue,
            b: ConsoleColor.Cyan);
        Colors.Dialog.Disabled = MakeColor(f: ConsoleColor.DarkGray,
            b: ConsoleColor.Gray);

        Colors.Error.Normal = MakeColor(f: ConsoleColor.White,
            b: ConsoleColor.Red);
        Colors.Error.Focus = MakeColor(f: ConsoleColor.Black,
            b: ConsoleColor.Gray);
        Colors.Error.HotNormal = MakeColor(f: ConsoleColor.Yellow,
            b: ConsoleColor.Red);
        Colors.Error.HotFocus = Colors.Error.HotNormal;
        Colors.Error.Disabled = MakeColor(f: ConsoleColor.DarkGray,
            b: ConsoleColor.White);

        //MockConsole.Clear ();
    }

    public override Attribute MakeAttribute(Color fore, Color back)
    {
        return MakeColor(f: (ConsoleColor) fore,
            b: (ConsoleColor) back);
    }

    private int _redrawColor = -1;

    private void SetColor(int color)
    {
        this._redrawColor = color;
        var values = Enum.GetValues(enumType: typeof(ConsoleColor))
            .OfType<ConsoleColor>()
            .Select(selector: s => (int) s);
        var enumerable = values as int[] ?? values.ToArray();
        if (enumerable.Contains(value: color & 0xffff)) this.BackgroundColor = (ConsoleColor) (color & 0xffff);

        if (enumerable.Contains(value: (color >> 16) & 0xffff))
            this.ForegroundColor = (ConsoleColor) ((color >> 16) & 0xffff);
    }

    public override void UpdateScreen()
    {
        if (this.firstRender) return;

        lock (this.contents)
        {
            var dirtySegments = new List<DirtySegment>();
            var output = new System.Text.StringBuilder();
            var top = this.Top;
            var left = this.Left;
            var rows = Math.Min(val1: this.WindowRows + top,
                val2: this.Rows);
            var cols = Math.Min(val1: this.WindowColumns + left,
                val2: this.Cols);
            for (var row = top; row < rows; row++)
            {
                if (!this._dirtyLine[row]) continue;
                this._dirtyLine[row] = false;
                var segmentStart = -1;
                var outputWidth = 0;
                for (var col = left; col < cols; col++)
                {
                    // no dirty flag here continue
                    if (col > 0 && this.contents[row,
                            col,
                            (int) RuneDataType.DirtyFlag] == 0)
                    {
                        if (output.Length > 0)
                        {
                            dirtySegments.Add(item: new DirtySegment(
                                BackgroundColor: this.TerminalSettings.TerminalBackground,
                                ForegroundColor: this.TerminalSettings.TerminalForeground,
                                Row: row,
                                Column: segmentStart,
                                Text: output.ToString()));
                            output.Clear();
                            segmentStart += outputWidth;
                            outputWidth = 0;
                        }
                        else if (segmentStart == -1)
                        {
                            segmentStart = col;
                        }
                        if (segmentStart + 1 < cols)
                            segmentStart++;
                        continue;
                    }

                    if (segmentStart == -1)
                        segmentStart = col;

                    // get color at current position
                    var color = this.contents[
                        row,
                        col,
                        (int) RuneDataType.Attribute];

                    if (this._redrawColor != color)
                    {
                        // we've reached a new color, add the segment and start a new one
                        if (output.Length > 0)
                            dirtySegments.Add(item: new DirtySegment(
                                BackgroundColor: this.TerminalSettings.TerminalBackground,
                                ForegroundColor: this.TerminalSettings.TerminalForeground,
                                Row: row,
                                Column: segmentStart,
                                Text: output.ToString()));
                        output.Clear();
                        segmentStart += outputWidth;
                        outputWidth = 0;
                        this.SetColor(color: color);
                    }
                    outputWidth++;
                    // append to buffer
                    output.Append(value: (char) this.contents[row, col, (int) RuneDataType.Rune]);

                    // clear the flag
                    this.contents[row,
                        col,
                        (int) RuneDataType.DirtyFlag] = 0;
                } // col

                // in case the segment ends at the end of the line, add it
                if (output.Length > 0)
                {
                    dirtySegments.Add(item: new DirtySegment(
                        BackgroundColor: this.TerminalSettings.TerminalBackground,
                        ForegroundColor: this.TerminalSettings.TerminalForeground,
                        Row: row,
                        Column: segmentStart,
                        Text: output.ToString()));
                    output.Clear();
                }
            } // row

            if (dirtySegments.Count > 0)
                _ = Task.Run(function: () => this._webConsole.DrawDirtySegmentToCanvas(
                    segments: dirtySegments,
                    terminalSettings: this.TerminalSettings));
        }
    }

    public override void Refresh()
    {
        this.UpdateScreen();

        this.UpdateCursor();
    }

    private Attribute _currentAttribute;

    public override void SetAttribute(Attribute c)
    {
        this._currentAttribute = c;
    }

    private static Key MapKey(ConsoleKeyInfo keyInfo)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (keyInfo.Key)
        {
            case ConsoleKey.Escape:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.Esc);
            case ConsoleKey.Tab:
                return keyInfo.Modifiers == ConsoleModifiers.Shift ? Key.BackTab : Key.Tab;
            case ConsoleKey.Home:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.Home);
            case ConsoleKey.End:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.End);
            case ConsoleKey.LeftArrow:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.CursorLeft);
            case ConsoleKey.RightArrow:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.CursorRight);
            case ConsoleKey.UpArrow:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.CursorUp);
            case ConsoleKey.DownArrow:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.CursorDown);
            case ConsoleKey.PageUp:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.PageUp);
            case ConsoleKey.PageDown:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.PageDown);
            case ConsoleKey.Enter:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.Enter);
            case ConsoleKey.Spacebar:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: keyInfo.KeyChar == 0 ? Key.Space : (Key) keyInfo.KeyChar);
            case ConsoleKey.Backspace:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.Backspace);
            case ConsoleKey.Delete:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.DeleteChar);
            case ConsoleKey.Insert:
                return MapKeyModifiers(keyInfo: keyInfo,
                    key: Key.InsertChar);

            case ConsoleKey.Oem1:
            case ConsoleKey.Oem2:
            case ConsoleKey.Oem3:
            case ConsoleKey.Oem4:
            case ConsoleKey.Oem5:
            case ConsoleKey.Oem6:
            case ConsoleKey.Oem7:
            case ConsoleKey.Oem8:
            case ConsoleKey.Oem102:
            case ConsoleKey.OemPeriod:
            case ConsoleKey.OemComma:
            case ConsoleKey.OemPlus:
            case ConsoleKey.OemMinus:
                if (keyInfo.KeyChar == 0)
                    return Key.Unknown;

                return (Key) keyInfo.KeyChar;
        }

        var key = keyInfo.Key;
        switch (key)
        {
            case >= ConsoleKey.A and <= ConsoleKey.Z:
                {
                    var delta = key - ConsoleKey.A;
                    switch (keyInfo.Modifiers)
                    {
                        case ConsoleModifiers.Control:
                            return (Key) ((uint) Key.CtrlMask | ((uint) Key.A + delta));
                        case ConsoleModifiers.Alt:
                            return (Key) ((uint) Key.AltMask | ((uint) Key.A + delta));
                    }

                    if ((keyInfo.Modifiers & (ConsoleModifiers.Alt | ConsoleModifiers.Control)) == 0)
                        return (Key) keyInfo.KeyChar;
                    if (keyInfo.KeyChar == 0)
                        return (Key) ((uint) Key.AltMask | (uint) Key.CtrlMask | ((uint) Key.A + delta));
                    return (Key) keyInfo.KeyChar;
                }
            case >= ConsoleKey.D0 and <= ConsoleKey.D9:
                {
                    var delta = key - ConsoleKey.D0;
                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (keyInfo.Modifiers)
                    {
                        case ConsoleModifiers.Alt:
                            return (Key) ((uint) Key.AltMask | ((uint) Key.D0 + delta));
                        case ConsoleModifiers.Control:
                            return (Key) ((uint) Key.CtrlMask | ((uint) Key.D0 + delta));
                    }

                    if (keyInfo.KeyChar == 0 || keyInfo.KeyChar == 30)
                        return MapKeyModifiers(keyInfo: keyInfo,
                            key: (Key) ((uint) Key.D0 + delta));

                    return (Key) keyInfo.KeyChar;
                }
            case >= ConsoleKey.F1 and <= ConsoleKey.F12:
                {
                    var delta = key - ConsoleKey.F1;
                    if ((keyInfo.Modifiers & (ConsoleModifiers.Shift | ConsoleModifiers.Alt | ConsoleModifiers.Control)) !=
                        0)
                        return MapKeyModifiers(keyInfo: keyInfo,
                            key: (Key) ((uint) Key.F1 + delta));

                    return (Key) ((uint) Key.F1 + delta);
                }
        }

        if (keyInfo.KeyChar != 0)
            return MapKeyModifiers(keyInfo: keyInfo,
                key: (Key) keyInfo.KeyChar);

        return (Key) 0xffffffff;
    }

    private KeyModifiers? _keyModifiers;

    private static Key MapKeyModifiers(ConsoleKeyInfo keyInfo, Key key)
    {
        var keyMod = new Key();
        if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
            keyMod = Key.ShiftMask;
        if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
            keyMod |= Key.CtrlMask;
        if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
            keyMod |= Key.AltMask;

        return keyMod != Key.Null ? keyMod | key : key;
    }

    private Action<KeyEvent>? _keyHandler;
    private Action<KeyEvent>? _keyDownHandler;
    private Action<KeyEvent>? _keyUpHandler;
    private Action<MouseEvent>? _mouseHandler;

    public override void PrepareToRun(MainLoop mainLoop, Action<KeyEvent> keyHandler, Action<KeyEvent> keyDownHandler,
        Action<KeyEvent> keyUpHandler, Action<MouseEvent> mouseHandler)
    {
        this._keyHandler = keyHandler;
        this._keyDownHandler = keyDownHandler;
        this._keyUpHandler = keyUpHandler;
        this._mouseHandler = mouseHandler;

        // ReSharper disable once HeapView.DelegateAllocation
        // Note: Net doesn't support keydown/up events and thus any passed keyDown/UpHandlers will never be called
        (mainLoop.Driver as WebMainLoopDriver)!.ProcessInput += this.ProcessInput;
    }

    private void ProcessInput(InputResult inputEvent)
    {
        switch (inputEvent.EventType)
        {
            case EventType.Key:
                {
                    this._keyModifiers = new KeyModifiers();
                    var map = MapKey(keyInfo: inputEvent.KeyEvent.ConsoleKeyInfo);
                    if (map == (Key) 0xffffffff)
                    {
                        var key = new KeyEvent();

                        if (inputEvent.KeyEvent.KeyDown)
                            this._keyDownHandler?.Invoke(obj: key);
                        else
                            this._keyUpHandler?.Invoke(obj: key);
                    }
                    else
                    {
                        if (inputEvent.KeyEvent.KeyDown)
                        {
                            this._keyDownHandler?.Invoke(obj: new KeyEvent(k: map,
                                km: this._keyModifiers));
                            this._keyHandler?.Invoke(obj: new KeyEvent(k: map,
                                km: this._keyModifiers));
                        }
                        else
                        {
                            this._keyUpHandler?.Invoke(obj: new KeyEvent(k: map,
                                km: this._keyModifiers));
                        }

                        if (!inputEvent.KeyEvent.KeyDown) this._keyModifiers = null;
                    }

                    break;
                }
            case EventType.Mouse:
                this._mouseHandler?.Invoke(obj: this.ToDriverMouse(me: inputEvent.MouseEvent));
                break;
            case EventType.Resize:
                this.WindowWidthPixels = inputEvent.ResizeEvent.Size.Width;
                this.WindowHeightPixels = inputEvent.ResizeEvent.Size.Height;
                this.WindowColumns = this.BufferColumns =
                    this.WindowWidthPixels / this.TerminalSettings.FontSpacePixels;
                this.WindowRows = this.BufferRows =
                    this.WindowHeightPixels / this.TerminalSettings.FontSizePixels;
                this.ProcessResize();
                break;
        }
    }

    private MouseEvent ToDriverMouse(WebMouseEvent me)
    {
        MouseFlags mouseFlag = 0;

        if ((me.ButtonState & MouseButtonState.Button1Pressed) != 0) mouseFlag |= MouseFlags.Button1Pressed;
        if ((me.ButtonState & MouseButtonState.Button1Released) != 0) mouseFlag |= MouseFlags.Button1Released;
        if ((me.ButtonState & MouseButtonState.Button1Clicked) != 0) mouseFlag |= MouseFlags.Button1Clicked;
        if ((me.ButtonState & MouseButtonState.Button1DoubleClicked) != 0) mouseFlag |= MouseFlags.Button1DoubleClicked;
        if ((me.ButtonState & MouseButtonState.Button1TripleClicked) != 0) mouseFlag |= MouseFlags.Button1TripleClicked;
        if ((me.ButtonState & MouseButtonState.Button2Pressed) != 0) mouseFlag |= MouseFlags.Button2Pressed;
        if ((me.ButtonState & MouseButtonState.Button2Released) != 0) mouseFlag |= MouseFlags.Button2Released;
        if ((me.ButtonState & MouseButtonState.Button2Clicked) != 0) mouseFlag |= MouseFlags.Button2Clicked;
        if ((me.ButtonState & MouseButtonState.Button2DoubleClicked) != 0) mouseFlag |= MouseFlags.Button2DoubleClicked;
        if ((me.ButtonState & MouseButtonState.Button2TrippleClicked) != 0)
            mouseFlag |= MouseFlags.Button2TripleClicked;
        if ((me.ButtonState & MouseButtonState.Button3Pressed) != 0) mouseFlag |= MouseFlags.Button3Pressed;
        if ((me.ButtonState & MouseButtonState.Button3Released) != 0) mouseFlag |= MouseFlags.Button3Released;
        if ((me.ButtonState & MouseButtonState.Button3Clicked) != 0) mouseFlag |= MouseFlags.Button3Clicked;
        if ((me.ButtonState & MouseButtonState.Button3DoubleClicked) != 0) mouseFlag |= MouseFlags.Button3DoubleClicked;
        if ((me.ButtonState & MouseButtonState.Button3TripleClicked) != 0) mouseFlag |= MouseFlags.Button3TripleClicked;
        if ((me.ButtonState & MouseButtonState.ButtonWheeledUp) != 0) mouseFlag |= MouseFlags.WheeledUp;
        if ((me.ButtonState & MouseButtonState.ButtonWheeledDown) != 0) mouseFlag |= MouseFlags.WheeledDown;
        if ((me.ButtonState & MouseButtonState.ButtonWheeledLeft) != 0) mouseFlag |= MouseFlags.WheeledLeft;
        if ((me.ButtonState & MouseButtonState.ButtonWheeledRight) != 0) mouseFlag |= MouseFlags.WheeledRight;
        if ((me.ButtonState & MouseButtonState.Button4Pressed) != 0) mouseFlag |= MouseFlags.Button4Pressed;
        if ((me.ButtonState & MouseButtonState.Button4Released) != 0) mouseFlag |= MouseFlags.Button4Released;
        if ((me.ButtonState & MouseButtonState.Button4Clicked) != 0) mouseFlag |= MouseFlags.Button4Clicked;
        if ((me.ButtonState & MouseButtonState.Button4DoubleClicked) != 0) mouseFlag |= MouseFlags.Button4DoubleClicked;
        if ((me.ButtonState & MouseButtonState.Button4TripleClicked) != 0) mouseFlag |= MouseFlags.Button4TripleClicked;
        if ((me.ButtonState & MouseButtonState.ReportMousePosition) != 0) mouseFlag |= MouseFlags.ReportMousePosition;
        if ((me.ButtonState & MouseButtonState.ButtonShift) != 0) mouseFlag |= MouseFlags.ButtonShift;
        if ((me.ButtonState & MouseButtonState.ButtonCtrl) != 0) mouseFlag |= MouseFlags.ButtonCtrl;
        if ((me.ButtonState & MouseButtonState.ButtonAlt) != 0) mouseFlag |= MouseFlags.ButtonAlt;

        return new MouseEvent
        {
            X = me.Position.X,
            Y = me.Position.Y,
            Flags = mouseFlag,
        };
    }

    public override Attribute GetAttribute()
    {
        return this._currentAttribute;
    }

    /// <inheritdoc />
    public override bool GetCursorVisibility(out CursorVisibility visibility)
    {
        visibility = this.CursorVisible ? CursorVisibility.Default : CursorVisibility.Invisible;

        return false;
    }

    /// <inheritdoc />
    public override bool SetCursorVisibility(CursorVisibility visibility)
    {
        this.CursorVisible = visibility != CursorVisibility.Invisible;

        return false;
    }

    /// <inheritdoc />
    public override bool EnsureCursorVisibility()
    {
        return false;
    }

    public override void SendKeys(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
    {
        var input = new InputResult();
        ConsoleKey ck;
        if (char.IsLetter(c: keyChar))
            ck = key;
        else
            ck = (ConsoleKey) '\0';
        input.EventType = EventType.Key;
        input.KeyEvent.ConsoleKeyInfo = new ConsoleKeyInfo(keyChar: keyChar,
            key: ck,
            shift: shift,
            alt: alt,
            control: control);

        try
        {
            this.ProcessInput(inputEvent: input);
        }
        catch (OverflowException)
        {
        }
    }

    public void SetBufferSize(int width, int height)
    {
        this.TerminalSettings.BufferRows = height;
        this.TerminalSettings.BufferColumns = width;
        this.WindowRows = height;
        this.WindowColumns = width;
        this.ProcessResize();
    }

    public void SetWindowSize(int width, int height)
    {
        this.WindowColumns = width;
        this.WindowRows = height;
        if (width > this.BufferColumns || !this.HeightAsBuffer) this.BufferColumns = width;

        if (height > this.BufferRows || !this.HeightAsBuffer) this.BufferRows = height;

        this.ProcessResize();
    }

    public void SetWindowPosition(int left, int top)
    {
        if (this.HeightAsBuffer)
        {
            this.WindowLeft = this.WindowLeft = Math.Max(val1: Math.Min(val1: left,
                    val2: this.Cols - this.WindowColumns),
                val2: 0);
            this.WindowTop = this.WindowTop = Math.Max(val1: Math.Min(val1: top,
                    val2: this.Rows - this.WindowRows),
                val2: 0);
        }
        else if (this.WindowLeft > 0 || this.WindowTop > 0)
        {
            this.WindowLeft = 0;
            this.WindowTop = 0;
        }
    }

    private void ProcessResize()
    {
        this.ResizeScreen();
        this.UpdateOffScreen();
        this.TerminalResized?.Invoke();
    }

    private void ResizeScreen()
    {
        if (!this.HeightAsBuffer)
        {
            if (this.WindowRows > 0)
                // Can raise an exception while is still resizing.
                try
                {
#pragma warning disable CA1416
                    this.CursorTop = 0;
                    this.CursorLeft = 0;
                    this.WindowTop = 0;
                    this.WindowLeft = 0;
#pragma warning restore CA1416
                }
                catch (IOException)
                {
                    return;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
        }
        else
        {
            try
            {
#pragma warning disable CA1416
                this.WindowLeft = Math.Max(val1: Math.Min(val1: this.Left,
                        val2: this.Cols - this.WindowColumns),
                    val2: 0);
                this.WindowTop = Math.Max(val1: Math.Min(val1: this.Top,
                        val2: this.Rows - this.WindowRows),
                    val2: 0);
#pragma warning restore CA1416
            }
            catch (Exception)
            {
                return;
            }
        }

        this.Clip = new Rect(x: 0,
            y: 0,
            width: this.Cols,
            height: this.Rows);
    }

    public override void UpdateOffScreen()
    {
        this.contents = new int[this.Rows, this.Cols, 3];
        this._dirtyLine = new bool[this.Rows];

        // Can raise an exception while is still resizing.
        try
        {
            for (var row = 0; row < this.Rows; row++)
                for (var c = 0; c < this.Cols; c++)
                {
                    this.contents[row,
                        c,
                        (int) RuneDataType.Rune] = ' ';
                    this.contents[row,
                        c,
                        (int) RuneDataType.Attribute] = Colors.TopLevel.Normal;
                    this.contents[row,
                        c,
                        (int) RuneDataType.DirtyFlag] = 0;
                    this._dirtyLine[row] = true;
                }
        }
        catch (IndexOutOfRangeException)
        {
        }
    }

    public override bool GetColors(int value, out Color foreground, out Color background)
    {
        var hasColor = false;
        foreground = default;
        background = default;

        var foregroundInt = value & 0xffff;
        var backgroundInt = (value >> 16) & 0xffff;

        var values = Enum.GetValues(
                enumType: typeof(Color))
            .OfType<Color>()
            .Select(selector: color => (int) color);

        var enumerable = values as int[] ?? values.ToArray();
        if (enumerable.Contains(value: foregroundInt))
        {
            hasColor = true;
            background = (Color) foregroundInt;
        }

        if (!enumerable.Contains(value: backgroundInt)) return hasColor;
        hasColor = true;
        foreground = (Color) backgroundInt;

        return hasColor;
    }

    #region Unused

    public override void UpdateCursor()
    {
        //
    }

    public override void StartReportingMouseMoves()
    {
    }

    public override void StopReportingMouseMoves()
    {
    }

    public override void Suspend()
    {
    }

    public override void SetColors(ConsoleColor foreground, ConsoleColor background)
    {
    }

    public override void SetColors(short foregroundColorId, short backgroundColorId)
    {
        throw new NotImplementedException();
    }

    public override void CookMouse()
    {
    }

    public override void UncookMouse()
    {
    }

    #endregion

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}