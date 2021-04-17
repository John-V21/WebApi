using System.Collections.Generic;
using FluentValidation;
using System.Linq;

namespace Accepted.FluentValidation
{
    public class FluentValidator
    {
        private readonly IFluentValidatorList _fluentValidatorList;

        private FluentValidator()
        {
        }

        public FluentValidator(IFluentValidatorList fluentValidatorList)
        {
            _fluentValidatorList = fluentValidatorList;
        }

        public IEnumerable<KeyValuePair<string, string>> Validate<T>(T obj, string[] rulesets = null)
        {
            ValidationContext<T> validationContext = ValidationContext<T>.CreateWithOptions(obj, o =>
            {
                if (rulesets != null)
                {
                    o.IncludeRuleSets(rulesets);
                }
            });
            var objType = obj.GetType();
            if (_fluentValidatorList.Validators.ContainsKey(objType))
            {
                foreach( var vv in _fluentValidatorList.Validators[objType])
                {
                    foreach(var ve in vv.Validate(validationContext).Errors)
                    {
                        yield return new KeyValuePair<string, string>(ve.PropertyName, ve.ErrorMessage);
                    };
                }
            }
        }

        public void ThrowIfInvalid(object o, string[] rulesets = null)
        {
            var validationResult = Validate(o, rulesets);
            if (validationResult.Any())
            {
                throw new FluentValidationException (validationResult.ToList());
            }
        }
    }
}
