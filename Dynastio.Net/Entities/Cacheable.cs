using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dynastio.Net.Entities
{
    /// <summary>
    /// Caches a value fetched asynchronously, refreshing it only after a given interval.
    /// If a fetch fails, the cache will not immediately retry until a configurable cooldown elapses.
    /// </summary>
    internal class Cacheable<T>
    {
        private readonly Func<Task<T>> _factory;
        private readonly TimeSpan _cacheDuration;
        private readonly TimeSpan _errorCooldown;
        private readonly object _sync = new object();

        private DateTime _lastSuccess = DateTime.MinValue;
        private DateTime _lastError = DateTime.MinValue;
        private T _cache = default!;

        /// <summary>
        /// Creates a new Cacheable&lt;T&gt;.
        /// </summary>
        /// <param name="cacheDuration">
        /// How long a successful value remains fresh before a new fetch is attempted.
        /// </param>
        /// <param name="factory">
        /// Async function to fetch the up-to-date value.
        /// </param>
        /// <param name="errorCooldown">
        /// After a fetch error, how long to wait before re-trying. Defaults to cacheDuration.
        /// </param>
        public Cacheable(
            TimeSpan cacheDuration,
            Func<Task<T>> factory,
            TimeSpan? errorCooldown = null)
        {
            _cacheDuration = cacheDuration;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _errorCooldown = errorCooldown ?? cacheDuration;
        }

        /// <summary>
        /// Gets the cached value, fetching a fresh one if:
        /// - we've never fetched yet, OR
        /// - cacheDuration has elapsed since the last success AND
        ///   we're not still within the errorCooldown after a failure.
        /// </summary>
        public T Value => GetValue();

        /// <summary>
        /// Synchronously returns the cached value, performing a blocking fetch if needed.
        /// </summary>
        public T GetValue()
        {
            var now = DateTime.UtcNow;
            bool shouldFetch;

            var expired = (now - _lastSuccess) >= _cacheDuration;
            var canRetry = (now - _lastError) >= _errorCooldown;
            var neverFetched = _lastSuccess == DateTime.MinValue;

            shouldFetch = (neverFetched || expired) && canRetry;


            if (shouldFetch)
            {
                try
                {
                    // Perform the fetch outside the lock to avoid deadlocks
                    var result = _factory().GetAwaiter().GetResult();

                    _cache = result;
                    _lastSuccess = DateTime.UtcNow;
                }
                catch
                {
                    _lastError = DateTime.UtcNow;

                }
            }

            return _cache;

        }

        /// <summary>
        /// Asynchronous version of GetValue: avoids blocking threads on I/O.
        /// </summary>
        public async Task<T> GetValueAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            bool shouldFetch;

            var expired = (now - _lastSuccess) >= _cacheDuration;
            var canRetry = (now - _lastError) >= _errorCooldown;
            var neverFetched = _lastSuccess == DateTime.MinValue;

            shouldFetch = (neverFetched || expired) && canRetry;


            if (shouldFetch)
            {
                try
                {
                    var result = await _factory().ConfigureAwait(false);

                    _cache = result;
                    _lastSuccess = DateTime.UtcNow;
                }
                catch
                {
                    _lastError = DateTime.UtcNow;
                }
            }

            return _cache;
        }
        public void Update(T data)
        {
            _cache = data;
            _lastSuccess = DateTime.UtcNow;
        }
        /// <summary>
        /// Resets the cache, forcing the next Value/GetValueAsync call to fetch anew.
        /// </summary>
        public void Invalidate()
        {
            lock (_sync)
            {
                _lastSuccess = DateTime.MinValue;
                _lastError = DateTime.MinValue;
                _cache = default!;
            }
        }
    }
}
