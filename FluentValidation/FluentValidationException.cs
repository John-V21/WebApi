using System;
using System.Collections.Generic;
using System.Linq;

namespace Accepted.FluentValidation
{
    public class FluentValidationFieldError
    {
        public string Field { get; set; }
        public string[] Errors { get; set; }
    }

    public class FluentValidationErrorList : List<FluentValidationFieldError>
    {
    }

    public class FluentValidationException : Exception
    {
        public FluentValidationErrorList Errors { get; private set; } = new FluentValidationErrorList();

        public FluentValidationException (IList<KeyValuePair<string, string>> keyValuePairs)
        {
            Errors.AddRange(keyValuePairs.GroupBy(x => x.Key)
            .Select(g => new FluentValidationFieldError
            {
                Field = g.Key,
                Errors = g.Select(x => x.Value).ToArray()
            })
            .ToList());
        }
    }
}
