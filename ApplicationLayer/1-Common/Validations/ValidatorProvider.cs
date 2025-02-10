using FluentValidation;

namespace ApplicationLayer.Common.Validations;

public interface IValidatorProvider
{
    IValidator GetValidator(Type type);
}
public class ValidatorProvider : IValidatorProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ValidatorProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IValidator GetValidator(Type type)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(type);
        return _serviceProvider.GetService(validatorType) as IValidator;
    }
}

