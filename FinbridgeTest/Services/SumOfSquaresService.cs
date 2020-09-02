﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finbridge.Test.Configuration;
using Finbridge.Test.Exceptions;
using Microsoft.Extensions.Options;

namespace Finbridge.Test.Services
{
    public class SumOfSquaresService : ISumOfSquares
    {
        private readonly CalculationConfiguration _configuration;

        public SumOfSquaresService(IOptions<CalculationConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        private void Validate(int[] sequence)
        {
            var count = sequence.Count();

            if (count == 0)
            {
                throw new ValidationException($"Sequence should not be empty");
            }

            if (count > _configuration.MaxCount)
            {
                throw new ValidationException($"Invalid count values in sequence, max count: {_configuration.MaxCount}");
            }

            if (!sequence.All(x => x >= _configuration.MinValue && x <= _configuration.MaxValue))
            {
                throw new ValidationException(
                    $"Invalid value in sequence, max value: {_configuration.MaxValue}, min value: {_configuration.MinValue}");
            }
        }

        public Task<int> CalculatingAsync(IEnumerable<int> sequence)
        {
            return Task.Run(() =>
            {
                var random = new Random();

                var values = sequence as int[] ?? sequence.ToArray();

                Validate(values);

                var result = values.Aggregate((x, y) => x + y * y);

                Thread.Sleep(random.Next(_configuration.MinInterval, _configuration.MaxInterval));

                return result;
            });
        } 
    }
}