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

    [Parameter] public string Text { get; set; } = string.Empty;

    public override bool IsSupported => true;

    protected override string GetClipboardDataImpl()
    {
        // ReSharper disable once HeapView.DelegateAllocation
        var task = Task.Run(function: async () =>
        {
            var clipboardData = await this._jsRuntime.InvokeAsync<string>(identifier: "clipboardFunctions.getText");
            if (clipboardData is { } text) return text;
            return null;
        });
        task.Wait();
        var text = task.Result ?? string.Empty;
        this.Text = text;
        return text;
    }

    protected override void SetClipboardDataImpl(string text)
    {
        // ReSharper disable once HeapView.DelegateAllocation
        // ReSharper disable once HeapView.ObjectAllocation
        var task = Task.Run(
            function: async () => await this._jsRuntime
                .InvokeAsync<bool>(
                    identifier: "clipboardFunctions.setText",
                    args: text)
                .ConfigureAwait(continueOnCapturedContext: false));
        task.Wait();
        this.Text = text;
    }
}