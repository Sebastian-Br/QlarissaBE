using System;
using System.Collections.Generic;
using System.Text;

namespace Qlarissa.Application.Interfaces.ExternalAPI;

public sealed class SubscriptionLock : IAsyncDisposable
{
    private readonly Func<IReadOnlyCollection<string>, ValueTask> _release;
    private readonly IReadOnlyCollection<string> _symbols;

    private int _disposed;

    public SubscriptionLock(IReadOnlyCollection<string> symbols, Func<IReadOnlyCollection<string>, ValueTask> release)
    {
        _symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
        _release = release ?? throw new ArgumentNullException(nameof(release));
    }

    public IReadOnlyCollection<string> Symbols => _symbols;

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        await _release(_symbols);
    }
}