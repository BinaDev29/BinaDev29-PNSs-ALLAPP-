// File Path: Application/Behaviours/ValidationBehaviour.cs
// Namespace: Application.Behaviours
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation; // ለ AbstractValidator እና ValidationFailure
// using ValidationException = Application.Exceptions.ValidationException; // የራስህን የምትጠቀም ከሆነ

namespace Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    // እዚህ ጋር ነው ማስተካከያ የሚያስፈልገው!
                    // የራስህ ValidationException class ካለህ እና List<ValidationFailure> የሚቀበል ከሆነ
                    // throw new Application.Exceptions.ValidationException(failures);

                    // ካልሆነ (እና FluentValidation's ValidationException መጠቀም ከፈለክ):
                    throw new FluentValidation.ValidationException(failures);
                }
            }
            return await next();
        }
    }
}