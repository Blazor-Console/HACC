using Spectre.Console;

namespace HACC.Models.Spectre;

internal sealed class BrowserExclusivityMode : IExclusivityMode
{
    public T Run<T>(Func<T> func)
    {
        return func();
    }

    public async Task<T> RunAsync<T>(Func<Task<T>> func)
    {
        return await func().ConfigureAwait(continueOnCapturedContext: false);
    }
}