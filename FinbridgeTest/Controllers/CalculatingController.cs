using System.Threading.Tasks;
using Finbridge.Test.Controllers.Dto;
using Finbridge.Test.Services;
using FinbridgeTest.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Finbridge.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatingController : ControllerBase
    {
        private readonly ISumOfSquares _sumOfSquaresService;

        public CalculatingController(ISumOfSquares sumOfSquaresService)
        {
            _sumOfSquaresService = sumOfSquaresService;
        }

        /// <summary>
        /// Method for calculating sum squares of sequence
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("sumOfSquares")]
        public async Task<ResultOfCalculateDto> SumOfSquares(SequenceDto dto)
        {
            var result = await _sumOfSquaresService.CalculatingAsync(dto.Values);
            return new ResultOfCalculateDto
            {
                Value = result
            };
        }
    }
}