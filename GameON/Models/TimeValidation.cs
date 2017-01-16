using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class TimeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var term = (Term)validationContext.ObjectInstance;

            if (term.StartTime == null)
                return new ValidationResult("Start time is required");
            if (term.EndTime == null)
                return new ValidationResult("End time is required");
            if (term.Deadline == null)
                return new ValidationResult("Deadline is required");

            if (term.StartTime >= term.EndTime)
            {
                return new ValidationResult("End time can't begin before/at the same as start time!");
            }
            else if (term.Deadline >= term.EndTime)
            {
                return new ValidationResult("End time can't begin before/at the same as deadline!");
            }
            else if (term.StartTime <= term.Deadline)
            {
                return new ValidationResult("Start time can't begin before/at the same as deadline!");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}