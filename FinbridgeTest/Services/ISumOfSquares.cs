using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finbridge.Test.Services
{
    public interface ISumOfSquares
    {
        Task<int> CalculatingAsync(IEnumerable<int> sequence);
    }
}