using System.Linq;
using FluentValidation;

namespace AjNetCore.Modules.Core.Validators
{
    public static class AbstractValidatorExtension
    {
        public static Result ValidateResult<TInstance>(this AbstractValidator<TInstance> validator, TInstance instance)
        {
            var results = validator.Validate(instance);

            if (results.IsValid) return new Result().SetSuccess();

            //var result = new Result { Success = false };
            var result = new Result().SetError("Marked red fields are mandatory.");

            foreach (var validationFailure in results.Errors)
                if (!result.Errors.ContainsKey(validationFailure.PropertyName))
                    result.Errors.Add(validationFailure.PropertyName, validationFailure.ErrorMessage);

            return result;
        }
    }
}