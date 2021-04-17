using System;
using System.Collections.Generic;
using FluentValidation;

namespace Accepted.FluentValidation
{
    public class FluentValidationOptions
    {
        private List<Type> _types = new List<Type>();
        internal IEnumerable<Type> Validators { get => _types; }

        public FluentValidationOptions Add<T>() where T : IValidator
        {
            _types.Add(typeof(T));
            return this;
        }
    }
}
