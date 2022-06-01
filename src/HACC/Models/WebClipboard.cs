using HACC.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Terminal.Gui;

namespace HACC.Models;

/// <summary>
///     Blazor based clipboard
/// </summary>
public class WebClipboard : ClipboardBase
{
    private readonly IJSRuntime _jsRuntime;

    public WebClipboard()
    {
        this._jsRuntime = HaccExtensions.GetService<IJSRuntime>();
    }

    [Parameter] public string? Text { get; set; } = string.Empty;

    public override bool IsSupported => true;

    protected override string GetClipboardDataImpl()
    {
        Task.Run(async () => await this.ReadFromClipboardAsync());
        return this.Text!;
    }

    private async Task ReadFromClipboardAsync()
    {
        // Reading from the clipboard may be denied, so you must handle the exception
        try
        {
            this.Text = await ReadTextAsync();
        }
        catch
        {
            Console.WriteLine("Cannot read from clipboard");
            this.Text = null;
        }
    }

    private ValueTask<string> ReadTextAsync()
    {
        return _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    protected override void SetClipboardDataImpl(string text)
    {
        Task.Run(async () => await this.CopyToClipboardAsync(text));
        this.Text = text;
    }

    private async Task CopyToClipboardAsync(string text)
    {
        // Writing to the clipboard may be denied, so you must handle the exception
        try
        {
            await WriteTextAsync(text);
            this.Text = text;
        }
        catch
        {
            Console.WriteLine("Cannot write text to clipboard");
            this.Text = null;
        }
    }

    private ValueTask WriteTextAsync(string text)
    {
        return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}