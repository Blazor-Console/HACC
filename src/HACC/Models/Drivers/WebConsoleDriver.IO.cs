using System.Text;
using HACC.Configuration;
using HACC.Enumerations;
using Spectre.Console;

namespace HACC.Models.Drivers;

//
// Summary:
//     Represents the standard input, output, and error streams for console applications.
//     This class cannot be inherited.
public partial class WebConsoleDriver
{
    //
    // Summary:
    //     Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or
    //     turned off.
    //
    // Returns:
    //     true if CAPS LOCK is turned on; false if CAPS LOCK is turned off.
    //
    // Exceptions:
    //   T:System.PlatformNotSupportedException:
    //     The get operation is invoked on an operating system other than Windows.
    public bool CapsLock => throw new NotImplementedException();

    //
    // Summary:
    //     Gets the standard error output stream.
    //
    // Returns:
    //     A System.IO.TextWriter that represents the standard error output stream.
    public TextWriter Error => throw new NotImplementedException();

    //
    // Summary:
    //     Gets the standard input stream.
    //
    // Returns:
    //     A System.IO.TextReader that represents the standard input stream.
    public TextReader In => throw new NotImplementedException();

    //
    // Summary:
    //     Gets a value that indicates whether the error output stream has been redirected
    //     from the standard error stream.
    //
    // Returns:
    //     true if error output is redirected; otherwise, false.
    public bool IsErrorRedirected => throw new NotImplementedException();

    //
    // Summary:
    //     Gets a value that indicates whether input has been redirected from the standard
    //     input stream.
    //
    // Returns:
    //     true if input is redirected; otherwise, false.
    public bool IsInputRedirected => throw new NotImplementedException();

    //
    // Summary:
    //     Gets a value that indicates whether output has been redirected from the standard
    //     output stream.
    //
    // Returns:
    //     true if output is redirected; otherwise, false.
    public bool IsOutputRedirected => throw new NotImplementedException();

    //
    // Summary:
    //     Gets a value indicating whether a key press is available in the input stream.
    //
    // Returns:
    //     true if a key press is available; otherwise, false.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     An I/O error occurred.
    //
    //   T:System.InvalidOperationException:
    //     Standard input is redirected to a file instead of the keyboard.
    public bool KeyAvailable => throw new NotImplementedException();

    //
    // Summary:
    //     Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or
    //     turned off.
    //
    // Returns:
    //     true if NUM LOCK is turned on; false if NUM LOCK is turned off.
    //
    // Exceptions:
    //   T:System.PlatformNotSupportedException:
    //     The get operation is invoked on an operating system other than Windows.
    public bool NumberLock => throw new NotImplementedException();

    //
    // Summary:
    //     Gets the standard output stream.
    //
    // Returns:
    //     A System.IO.TextWriter that represents the standard output stream.
    public TextWriter Out => throw new NotImplementedException();

    //
    // Summary:
    //     Gets or sets the encoding the console uses to read input.
    //
    // Returns:
    //     The encoding used to read console input.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The property value in a set operation is null.
    //
    //   T:System.IO.IOException:
    //     An error occurred during the execution of this operation.
    //
    //   T:System.Security.SecurityException:
    //     Your application does not have permission to perform this operation.
    public Encoding InputEncoding
    {
        get => throw new NotImplementedException();
        set { }
    }

    //
    // Summary:
    //     Gets or sets a value indicating whether the combination of the System.ConsoleModifiers.Control
    //     modifier key and System.ConsoleKey.C console key (Ctrl+C) is treated as ordinary
    //     input or as an interruption that is handled by the operating system.
    //
    // Returns:
    //     true if Ctrl+C is treated as ordinary input; otherwise, false.
    //
    // Exceptions:
    //   T:System.IO.IOException:
    //     Unable to get or set the input mode of the console input buffer.
    public bool TreatControlCAsInput
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    //
    // Summary:
    //     Gets or sets the encoding the console uses to write output.
    //
    // Returns:
    //     The encoding used to write console output.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The property value in a set operation is null.
    //
    //   T:System.IO.IOException:
    //     An error occurred during the execution of this operation.
    //
    //   T:System.Security.SecurityException:
    //     Your application does not have permission to perform this operation.
    public Encoding OutputEncoding
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public IAnsiConsoleInput Input => throw new NotImplementedException();

    //
    // Summary:
    //     Occurs when the System.ConsoleModifiers.Control modifier key (Ctrl) and either
    //     the System.ConsoleKey.C console key (C) or the Break key are pressed simultaneously
    //     (Ctrl+C or Ctrl+Break).
    public event ConsoleCancelEventHandler? CancelKeyPress
    {
        //[System.Runtime.CompilerServices.NullableContext(2)]
        add => throw new NotImplementedException();
        //[System.Runtime.CompilerServices.NullableContext(2)]
        remove => throw new NotImplementedException();
    }

    //
    // Summary:
    //     Plays the sound of a beep of a specified frequency and duration through the console
    //     speaker.
    //
    // Parameters:
    //   frequency:
    //     The frequency of the beep, ranging from 37 to 32767 hertz.
    //
    //   duration:
    //     The duration of the beep measured in milliseconds.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     frequency is less than 37 or more than 32767 hertz. -or- duration is less than
    //     or equal to zero.
    //
    //   T:System.Security.HostProtectionException:
    //     This method was executed on a server, such as SQL Server, that does not permit
    //     access to the console.
    //
    //   T:System.PlatformNotSupportedException:
    //     The current operating system is not Windows.
    public async Task Beep(float frequency = Defaults.BeepFrequency, float duration = Defaults.BeepDurationMsec,
        float volume = Defaults.BeepVolume, BeepType beepType = Defaults.BeepType)
    {
        // ReSharper disable once HeapView.BoxingAllocation
        var type = Enum.GetName(
            enumType: typeof(BeepType),
            value: beepType)!.ToLowerInvariant();
        await this._webConsole.Beep(
            duration: duration,
            frequency: frequency,
            volume: volume,
            type: type);
    }

    //
    // Summary:
    //     Acquires the standard error stream.
    //
    // Returns:
    //     The standard error stream.
    public Stream OpenStandardError()
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Acquires the standard error stream, which is set to a specified buffer size.
    //
    // Parameters:
    //   bufferSize:
    //     This parameter has no effect, but its value must be greater than or equal to
    //     zero.
    //
    // Returns:
    //     The standard error stream.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     bufferSize is less than or equal to zero.
    public Stream OpenStandardError(int bufferSize)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Acquires the standard input stream.
    //
    // Returns:
    //     The standard input stream.
    public Stream OpenStandardInput()
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Acquires the standard input stream, which is set to a specified buffer size.
    //
    // Parameters:
    //   bufferSize:
    //     This parameter has no effect, but its value must be greater than or equal to
    //     zero.
    //
    // Returns:
    //     The standard input stream.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     bufferSize is less than or equal to zero.
    public Stream OpenStandardInput(int bufferSize)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Acquires the standard output stream.
    //
    // Returns:
    //     The standard output stream.
    public Stream OpenStandardOutput()
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Acquires the standard output stream, which is set to a specified buffer size.
    //
    // Parameters:
    //   bufferSize:
    //     This parameter has no effect, but its value must be greater than or equal to
    //     zero.
    //
    // Returns:
    //     The standard output stream.
    //
    // Exceptions:
    //   T:System.ArgumentOutOfRangeException:
    //     bufferSize is less than or equal to zero.
    public Stream OpenStandardOutput(int bufferSize)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the System.ConsoleDriver.Error property to the specified System.IO.TextWriter
    //     object.
    //
    // Parameters:
    //   newError:
    //     A stream that is the new standard error output.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     newError is null.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public void SetError(TextWriter newError)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the System.ConsoleDriver.In property to the specified System.IO.TextReader object.
    //
    // Parameters:
    //   newIn:
    //     A stream that is the new standard input.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     newIn is null.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public void SetIn(TextReader newIn)
    {
        throw new NotImplementedException();
    }

    //
    // Summary:
    //     Sets the System.ConsoleDriver.Out property to target the System.IO.TextWriter object.
    //
    // Parameters:
    //   newOut:
    //     A text writer to be used as the new standard output.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     newOut is null.
    //
    //   T:System.Security.SecurityException:
    //     The caller does not have the required permission.
    public void SetOut(TextWriter newOut)
    {
        throw new NotImplementedException();
    }
}