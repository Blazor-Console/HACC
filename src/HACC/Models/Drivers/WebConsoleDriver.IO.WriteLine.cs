using NStack;
using System.Drawing;
using System.Globalization;

namespace HACC.Models.Drivers;

//
// Summary:
//     Represents the standard input, output, and error streams for console applications.
//     This class cannot be inherited.
public partial class WebConsoleDriver
{
    //
    // Summary:
    //     Writes the current line terminator to the standard output stream.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine()
    {
        var currentPosition = this.CursorPosition;
        this.TerminalSettings.SetCursorPosition(x: currentPosition.X,
            y: currentPosition.Y + 1);
    }

    //
    // Summary:
    //     Writes the text representation of the specified Boolean value, followed by the
    //     current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(bool value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the specified Unicode character, followed by the current line terminator,
    //     value to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(char value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the specified array of Unicode characters, followed by the current line
    //     terminator, to the standard output stream.
    //
    // Parameters:
    //   buffer:
    //     A Unicode character array.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(char[]? buffer)
    {
        this.WriteLine(value: buffer is null ? string.Empty : Convert.ToString(value: buffer));
    }

    //
    // Summary:
    //     Writes the specified subarray of Unicode characters, followed by the current
    //     line terminator, to the standard output stream.
    //
    // Parameters:
    //   buffer:
    //     An array of Unicode characters.
    //
    //   index:
    //     The starting position in buffer.
    //
    //   count:
    //     The number of characters to write.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     buffer is null.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     index or count is less than zero.
    //
    //   T:System.ArgumentException:
    //     index plus count specify a position that is not within buffer.
    //
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(char[] buffer, int index, int count)
    {
        var value = Convert.ToString(value: buffer)
            ?.Substring(
                startIndex: index,
                length: count);
        this.WriteLine(value: value);
    }

    //
    // Summary:
    //     Writes the text representation of the specified System.Decimal value, followed
    //     by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(decimal value)
    {
        this.WriteLine(value: Convert.ToString(value: value,
            provider: CultureInfo.InvariantCulture));
    }

    //
    // Summary:
    //     Writes the text representation of the specified double-precision floating-point
    //     value, followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(double value)
    {
        this.WriteLine(value: Convert.ToString(value: value,
            provider: CultureInfo.InvariantCulture));
    }

    //
    // Summary:
    //     Writes the text representation of the specified 32-bit signed integer value,
    //     followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(int value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the text representation of the specified 64-bit signed integer value,
    //     followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(long value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the text representation of the specified object, followed by the current
    //     line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(object? value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the text representation of the specified single-precision floating-point
    //     value, followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(float value)
    {
        this.WriteLine(value: Convert.ToString(value: value,
            provider: CultureInfo.InvariantCulture));
    }

    //
    // Summary:
    //     Writes the specified string value, followed by the current line terminator, to
    //     the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(string? value)
    {
        this.AddStr(str: value is null ? ustring.Empty : ustring.Make(str: value));
        var currentPosition = this.CursorPosition;
        this.CursorPosition = new Point(x: 0,
            y: currentPosition.Y + 1);
    }

    //
    // Summary:
    //     Writes the text representation of the specified object, followed by the current
    //     line terminator, to the standard output stream using the specified format information.
    //
    // Parameters:
    //   format:
    //     A composite format string.
    //
    //   arg0:
    //     An object to write using format.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.ArgumentNullException:
    //     format is null.
    //
    //   T:System.FormatException:
    //     The format specification in format is invalid.
    public void WriteLine(string format, object? arg0)
    {
        this.WriteLine(value: string.Format(
            format: format,
            arg0: arg0));
    }

    //
    // Summary:
    //     Writes the text representation of the specified objects, followed by the current
    //     line terminator, to the standard output stream using the specified format information.
    //
    // Parameters:
    //   format:
    //     A composite format string.
    //
    //   arg0:
    //     The first object to write using format.
    //
    //   arg1:
    //     The second object to write using format.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.ArgumentNullException:
    //     format is null.
    //
    //   T:System.FormatException:
    //     The format specification in format is invalid.
    public void WriteLine(string format, object? arg0, object? arg1)
    {
        this.WriteLine(value: string.Format(
            format: format,
            arg0: arg0,
            arg1: arg1));
    }

    //
    // Summary:
    //     Writes the text representation of the specified objects, followed by the current
    //     line terminator, to the standard output stream using the specified format information.
    //
    // Parameters:
    //   format:
    //     A composite format string.
    //
    //   arg0:
    //     The first object to write using format.
    //
    //   arg1:
    //     The second object to write using format.
    //
    //   arg2:
    //     The third object to write using format.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.ArgumentNullException:
    //     format is null.
    //
    //   T:System.FormatException:
    //     The format specification in format is invalid.
    public void WriteLine(string format, object? arg0, object? arg1, object? arg2)
    {
        this.WriteLine(value: string.Format(
            format: format,
            arg0: arg0,
            arg1: arg1,
            arg2: arg2));
    }

    //
    // Summary:
    //     Writes the text representation of the specified array of objects, followed by
    //     the current line terminator, to the standard output stream using the specified
    //     format information.
    //
    // Parameters:
    //   format:
    //     A composite format string.
    //
    //   arg:
    //     An array of objects to write using format.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.ArgumentNullException:
    //     format or arg is null.
    //
    //   T:System.FormatException:
    //     The format specification in format is invalid.
    public void WriteLine(string format, params object?[]? arg)
    {
        this.WriteLine(value: string.Format(
            format: format,
            arg0: arg));
    }

    //
    // Summary:
    //     Writes the text representation of the specified 32-bit unsigned integer value,
    //     followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(uint value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }

    //
    // Summary:
    //     Writes the text representation of the specified 64-bit unsigned integer value,
    //     followed by the current line terminator, to the standard output stream.
    //
    // Parameters:
    //   value:
    //     The value to write.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    public void WriteLine(ulong value)
    {
        this.WriteLine(value: Convert.ToString(value: value));
    }
}