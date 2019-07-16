﻿using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PingDong.CleanArchitect.Core;
using PingDong.Linq;

namespace PingDong.CleanArchitect.Service
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.IsNullOrEmpty())
            {
                var failures = _validators
                    .Select(v => v.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(error => error != null)
                    .ToList();

                if (failures.Any())
                {
                    throw new DomainException(
                        $"Command Validation Errors for type {typeof(TRequest).Name}",
                        new ValidationException("Validation exception", failures));
                }
            }

            var response = await next();

            return response;
        }
    }
}