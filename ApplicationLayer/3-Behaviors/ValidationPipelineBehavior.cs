#region Usings

using ApplicationLayer.Common.Validations;
using ApplicationLayer.Extensions.SmartEnums;
using FluentValidation;
using MediatR;

#endregion

namespace ApplicationLayer.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
                where TRequest : IRequest<TResponse>
                where TResponse : HandlerResult
    {
        private readonly IValidatorProvider _validatorProvider;

        public ValidationPipelineBehavior(IValidatorProvider validatorProvider)
        {
            _validatorProvider = validatorProvider;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                var inputDataProperty = typeof(TRequest).GetProperty("InputData");

                if (inputDataProperty != null)
                {
                    var inputData = inputDataProperty.GetValue(request);
                    var validationContext = new ValidationContext<object>(inputData);
                    var inputDataType = inputDataProperty.PropertyType;
                    var validator = _validatorProvider.GetValidator(inputDataType);
                    if (validator != null)
                    {
                        var validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

                        if (!validationResult.IsValid)
                        {
                            HandlerResult result = new(inputDataType);
                            result.RequestStatus = RequestStatus.ValidationFailed;
                            result.Message = validationResult.ToString("~");
                            result.ValidationResult = new()
                            {
                                ErrorMessage = result.Message,
                                IsValid = false,
                                ValidatedValue = request,
                                ErrorCode = string.Join(',', validationResult.Errors.Select(e => e.ErrorCode))
                            };

                            return (TResponse)(object)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }

            return await next();
        }
    }
}