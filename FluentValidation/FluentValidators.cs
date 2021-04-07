using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;

namespace Accepted.FluentValidation
{
    public class ValidationErrorList : List<ValidationFieldErrors>
    {
    }

    public class ValidationFieldErrors
    {
        public string Name { get; set; }
        public string[] Errors { get; set; }
    }

    public class ValidationErrorsException: Exception
    {
        public ValidationErrorList Errors { get; private set; } = new ValidationErrorList();

        public ValidationErrorsException(IList<KeyValuePair<string, string>> keyValuePairs)
        {
            Errors.AddRange(keyValuePairs.GroupBy(x => x.Key)
            .Select(g => new ValidationFieldErrors
            {
                Name = g.Key,
                Errors = g.Select(x => x.Value).ToArray()
            })
            .ToList());
        }
    }

    public class FluentValidationOptions
    {
        List<Type> _types = new List<Type>();
        public FluentValidationOptions Add<T>() where T : IValidator
        {
            _types.Add(typeof(T));
            return this;
        }
        internal IEnumerable<Type> Validators { get => _types; }
    }

    public class FluentValidator
    {
        private readonly Dictionary<Type, List<IValidator>> _validators = new Dictionary<Type, List<IValidator>>();

        private FluentValidator()
        {
        }

        public FluentValidator(FluentValidationOptions options)
        {
            foreach (var v in options.Validators)
            {
                var validator = Activator.CreateInstance(v) as IValidator
                    ?? throw new Exception("Invalid validator, missing default constuctor");
                Add(validator);
            }
        }

        public FluentValidator Add(IValidator entityValidator)
        {
            var mi = entityValidator.GetType().BaseType.GenericTypeArguments.FirstOrDefault()?.UnderlyingSystemType
                ?? throw new Exception("Invalid validator");

            if (_validators.ContainsKey(mi))
                _validators[mi].Add(entityValidator);
            else
            {
                _validators.Add(mi, new List<IValidator> { entityValidator });
            }
            return this;
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
            if (_validators.ContainsKey(objType))
            {
                foreach( var vv in _validators[objType])
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
                throw new ValidationErrorsException(validationResult.ToList());
            }
        }
    }
}
