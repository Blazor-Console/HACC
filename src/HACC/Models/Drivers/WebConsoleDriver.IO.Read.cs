namespace HACC.Models.Drivers;

//
// Summary:
//     Represents the standard input, output, and error streams for console applications.
//     This class cannot be inherited.
public partial class WebConsoleDriver
{
    //
    // Summary:
    //     Reads the next character from the standard input stream.
    //
    // Returns:
    //     The next character from the input stream, or negative one (-1) if there are currently
    //     no more characters to be read.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public int Read()
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Obtains the next character or function key pressed by the user. The pressed key
    //     is displayed in the console window.
    //
    // Returns:
    //     An object that describes the System.ConsoleKey constant and Unicode character,
    //     if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
    //     object also describes, in a bitwise combination of System.ConsoleModifiers values,
    //     whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
    //     with the console key.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     The System.ConsoleDriver.In property is redirected from some stream other than the
    //     console.
    public ConsoleKeyInfo ReadKey()
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Obtains the next character or function key pressed by the user. The pressed key
    //     is optionally displayed in the console window.
    //
    // Parameters:
    //   intercept:
    //     Determines whether to display the pressed key in the console window. true to
    //     not display the pressed key; otherwise, false.
    //
    // Returns:
    //     An object that describes the System.ConsoleKey constant and Unicode character,
    //     if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
    //     object also describes, in a bitwise combination of System.ConsoleModifiers values,
    //     whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
    //     with the console key.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     The System.ConsoleDriver.In property is redirected from some stream other than the
    //     console.
    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Reads the next line of characters from the standard input stream.
    //
    // Returns:
    //     The next line of characters from the input stream, or null if no more lines are
    //     available.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.OutOfMemoryException:
    //     There is insufficient memory to allocate a buffer for the returned string.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The number of characters in the next line of characters is greater than System.Int32.MaxValue.
    public string? ReadLine()
    {
        throw new NotImplementedException();
    }
}