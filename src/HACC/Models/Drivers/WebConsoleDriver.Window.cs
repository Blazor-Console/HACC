using HACC.Configuration;
using HACC.Models.EventArgs;

namespace HACC.Models.Drivers;

//
// Summary:
//     Represents the standard input, output, and error streams for console applications.
//     This class cannot be inherited.
public partial class WebConsoleDriver
{
    public delegate void NewFrameHandler(object sender, NewFrameEventArgs e);

    //
    // Summary:
    //     Gets or sets the height of the buffer area.
    //
    // Returns:
    //     The current height, in rows, of the buffer area.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     The value in a set operation is less than or equal to zero. -or- The value in
    //     a set operation is greater than or equal to System.Int16.MaxValue. -or- The value
    //     in a set operation is less than System.ConsoleDriver.WindowTop + System.ConsoleDriver.WindowHeight.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int BufferRows
    {
        get => this.TerminalSettings.WindowRows;
        set
        {
            this.TerminalSettings.WindowRows = value;
            this.TerminalResized?.Invoke();
        }
    }

    //
    // Summary:
    //     Gets or sets the width of the buffer area.
    //
    // Returns:
    //     The current width, in columns, of the buffer area.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     The value in a set operation is less than or equal to zero. -or- The value in
    //     a set operation is greater than or equal to System.Int16.MaxValue. -or- The value
    //     in a set operation is less than System.ConsoleDriver.WindowLeft + System.ConsoleDriver.WindowWidth.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int BufferColumns
    {
        get => this.TerminalSettings.WindowColumns;
        set
        {
            this.TerminalSettings.WindowColumns = value;
            this.TerminalResized?.Invoke();
        }
    }

    //
    // Summary:
    //     Gets the largest possible number of console window rows, based on the current
    //     font and screen resolution.
    //
    // Returns:
    //     The height of the largest possible console window measured in rows.
    public int LargestWindowHeight => Defaults.MaximumRows;

    //
    // Summary:
    //     Gets the largest possible number of console window columns, based on the current
    //     font and screen resolution.
    //
    // Returns:
    //     The width of the largest possible console window measured in columns.
    public int LargestWindowWidth => Defaults.MaximumColumns;

    //
    // Summary:
    //     Gets or sets the title to display in the console title bar.
    //
    // Returns:
    //     The string to be displayed in the title bar of the console. The maximum length
    //     of the title string is 24500 characters.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     In a get operation, the retrieved title is longer than 24500 characters.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     In a set operation, the specified title is longer than 24500 characters.
    //
    //   T:System.ArgumentNullException:
    //     In a set operation, the specified title is null.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The get operation is invoked on an operating system other than Windows.
    public string Title
    {
        get => this.TerminalSettings.Title;
        set
        {
            this.TerminalSettings.Title = value;

            throw new NotImplementedException();
        }
    }

    //
    // Summary:
    //     Gets or sets the height of the console window area.
    //
    // Returns:
    //     The height of the console window measured in rows.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     The value of the System.ConsoleDriver.WindowWidth property or the value of the System.ConsoleDriver.WindowHeight
    //     property is less than or equal to 0. -or- The value of the System.ConsoleDriver.WindowHeight
    //     property plus the value of the System.ConsoleDriver.WindowTop property is greater than
    //     or equal to System.Int16.MaxValue. -or- The value of the System.ConsoleDriver.WindowWidth
    //     property or the value of the System.ConsoleDriver.WindowHeight property is greater
    //     than the largest possible window width or height for the current screen resolution
    //     and console font.
    //
    //   T:System.IO.IOException:
    //     Error reading or writing information.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int WindowRows
    {
        get => this.TerminalSettings.WindowRows;
        set
        {
            if (value > this.LargestWindowHeight)
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    message: "The value of the WindowRows property is greater than the largest possible window height");
            this.TerminalSettings.WindowRows = value;
        }
    }

    //
    // Summary:
    //     Gets or sets the leftmost position of the console window area relative to the
    //     screen buffer.
    //
    // Returns:
    //     The leftmost console window position measured in columns.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     In a set operation, the value to be assigned is less than zero. -or- As a result
    //     of the assignment, System.ConsoleDriver.WindowLeft plus System.ConsoleDriver.WindowWidth
    //     would exceed System.ConsoleDriver.BufferWidth.
    //
    //   T:System.IO.IOException:
    //     Error reading or writing information.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int WindowLeft { get; set; }

    //
    // Summary:
    //     Gets or sets the top position of the console window area relative to the screen
    //     buffer.
    //
    // Returns:
    //     The uppermost console window position measured in rows.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     In a set operation, the value to be assigned is less than zero. -or- As a result
    //     of the assignment, System.ConsoleDriver.WindowTop plus System.ConsoleDriver.WindowHeight
    //     would exceed System.ConsoleDriver.BufferHeight.
    //
    //   T:System.IO.IOException:
    //     Error reading or writing information.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int WindowTop { get; set; }

    //
    // Summary:
    //     Gets or sets the width of the console window.
    //
    // Returns:
    //     The width of the console window measured in columns.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     The value of the System.ConsoleDriver.WindowWidth property or the value of the System.ConsoleDriver.WindowHeight
    //     property is less than or equal to 0. -or- The value of the System.ConsoleDriver.WindowHeight
    //     property plus the value of the System.ConsoleDriver.WindowTop property is greater than
    //     or equal to System.Int16.MaxValue. -or- The value of the System.ConsoleDriver.WindowWidth
    //     property or the value of the System.ConsoleDriver.WindowHeight property is greater
    //     than the largest possible window width or height for the current screen resolution
    //     and console font.
    //
    //   T:System.IO.IOException:
    //     Error reading or writing information.
    //
    //   T:System.PlatformNotSupportedException:
    //     The set operation is invoked on an operating system other than Windows.
    public int WindowColumns
    {
        get => this.TerminalSettings.WindowColumns;
        set
        {
            if (value > this.LargestWindowWidth)
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    message:
                    "The value of the WindowColumns property is greater than the largest possible window width");
            this.TerminalSettings.WindowColumns = value;
        }
    }

    public int WindowHeightPixels
    {
        get => this.TerminalSettings.WindowHeightPixels;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    message: "The value of the WindowHeightPixels property is less than zero");
            this.TerminalSettings.WindowHeightPixels = value;
        }
    }

    public int WindowWidthPixels
    {
        get => this.TerminalSettings.WindowWidthPixels;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(value),
                    message: "The value of the WindowWidthPixels property is less than zero");
            this.TerminalSettings.WindowWidthPixels = value;
        }
    }

    public void Clear(bool home)
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        this.Contents = new int[this.BufferRows, this.BufferColumns, 3];
        this._dirtyLine = new bool[this.BufferRows];
        this.SetCursorPosition(left: 0,
            top: 0);
    }

    public event NewFrameHandler? NewFrame;

    //
    // Parameters:
    //   sourceLeft:
    //     The leftmost column of the source area.
    //
    //   sourceTop:
    //     The topmost row of the source area.
    //
    //   sourceWidth:
    //     The number of columns in the source area.
    //
    //   sourceHeight:
    //     The number of rows in the source area.
    //
    //   targetLeft:
    //     The leftmost column of the destination area.
    //
    //   targetTop:
    //     The topmost row of the destination area.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     One or more of the parameters is less than zero. -or- sourceLeft or targetLeft
    //     is greater than or equal to System.ConsoleDriver.BufferWidth. -or- sourceTop or targetTop
    //     is greater than or equal to System.ConsoleDriver.BufferHeight. -or- sourceTop + sourceHeight
    //     is greater than or equal to System.ConsoleDriver.BufferHeight. -or- sourceLeft + sourceWidth
    //     is greater than or equal to System.ConsoleDriver.BufferWidth.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
        int targetTop)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Copies a specified source area of the screen buffer to a specified destination
    //     area.
    //
    // Parameters:
    //   sourceLeft:
    //     The leftmost column of the source area.
    //
    //   sourceTop:
    //     The topmost row of the source area.
    //
    //   sourceWidth:
    //     The number of columns in the source area.
    //
    //   sourceHeight:
    //     The number of rows in the source area.
    //
    //   targetLeft:
    //     The leftmost column of the destination area.
    //
    //   targetTop:
    //     The topmost row of the destination area.
    //
    //   sourceChar:
    //     The character used to fill the source area.
    //
    //   sourceForeColor:
    //     The foreground color used to fill the source area.
    //
    //   sourceBackColor:
    //     The background color used to fill the source area.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     One or more of the parameters is less than zero. -or- sourceLeft or targetLeft
    //     is greater than or equal to System.ConsoleDriver.BufferWidth. -or- sourceTop or targetTop
    //     is greater than or equal to System.ConsoleDriver.BufferHeight. -or- sourceTop + sourceHeight
    //     is greater than or equal to System.ConsoleDriver.BufferHeight. -or- sourceLeft + sourceWidth
    //     is greater than or equal to System.ConsoleDriver.BufferWidth.
    //
    //   T:System.ArgumentException:
    //     One or both of the color parameters is not a member of the System.ConsoleColor
    //     enumeration.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
        int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the height and width of the screen buffer area to the specified values.
    //
    // Parameters:
    //   width:
    //     The width of the buffer area measured in columns.
    //
    //   height:
    //     The height of the buffer area measured in rows.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     height or width is less than or equal to zero. -or- height or width is greater
    //     than or equal to System.Int16.MaxValue. -or- width is less than System.ConsoleDriver.WindowLeft
    //     + System.ConsoleDriver.WindowWidth. -or- height is less than System.ConsoleDriver.WindowTop
    //     + System.ConsoleDriver.WindowHeight.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public void SetInternalBufferSize(int width, int height)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the position of the console window relative to the screen buffer.
    //
    // Parameters:
    //   left:
    //     The column position of the upper left corner of the console window.
    //
    //   top:
    //     The row position of the upper left corner of the console window.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     left or top is less than zero. -or- left + System.ConsoleDriver.WindowWidth is greater
    //     than System.ConsoleDriver.BufferWidth. -or- top + System.ConsoleDriver.WindowHeight is greater
    //     than System.ConsoleDriver.BufferHeight.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public void SetInternalWindowPosition(int left, int top)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the height and width of the console window to the specified values.
    //
    // Parameters:
    //   width:
    //     The width of the console window measured in columns.
    //
    //   height:
    //     The height of the console window measured in rows.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     width or height is less than or equal to zero. -or- width plus System.ConsoleDriver.WindowLeft
    //     or height plus System.ConsoleDriver.WindowTop is greater than or equal to System.Int16.MaxValue.
    //     -or- width or height is greater than the largest possible window width or height
    //     for the current screen resolution and console font.
    //
    //   T:System.Security.SecurityException:
    //     The user does not have permission to perform this action.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public void SetInternalWindowSize(int width, int height)
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        this.Contents = new int[height, width, 3];
    }

    //
    // Summary:
    //     Clears the console buffer and corresponding console window of display information.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void Clear()
    {
        this.Clear(home: false);
    }

    private void OnNewFrame(NewFrameEventArgs e)
    {
        this.NewFrame?.Invoke(sender: this,
            e: e);
    }
}