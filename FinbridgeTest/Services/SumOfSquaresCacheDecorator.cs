using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Finbridge.Test.Services
{
    public class SumOfSquaresCacheDecorator : ISumOfSquares
    {
        private readonly ISumOfSquares _next;
        private readonly IMemoryCache _memoryCache;

        public SumOfSquaresCacheDecorator(ISumOfSquares next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }

        private string GetKey(IEnumerable<int> sequence) => string.Join(",", sequence);

        public async Task<int> CalculatingAsync(IEnumerable<int> sequence)
        {
            var cacheKey = GetKey(sequence);
            if (_memoryCache.TryGetValue(cacheKey, out int cacheValue))
            {
                return cacheValue;
            }
            var value = await _next.CalculatingAsync(sequence);
            _memoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return value;
        }
    }
}