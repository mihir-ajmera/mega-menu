using System.Collections.Generic;
using FluentValidation.Validators;

namespace AjNetCore.Modules.Core.Validators
{
    public class AjNotNullValidator : PropertyValidator, INotNullValidator
    {

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
                return false;

            if (context.PropertyValue is List<int>)
                if (context.PropertyValue.To<List<int>>().Count == 0)
                    return false;

            return true;
        }

        protected override string GetDefaultMessageTemplate()
        {
            return Localized(nameof(NotNullValidator));
        }
    }

    public interface INotNullValidator : IPropertyValidator
    {
    }
}