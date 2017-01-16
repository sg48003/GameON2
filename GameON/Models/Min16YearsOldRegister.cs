using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.Models
{
    public class Min16YearsOldRegister : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (RegisterViewModel)validationContext.ObjectInstance;

            if (user.BirthDate == null)
                return new ValidationResult("Birthdate is required");

            var age = DateTime.Today.Year - user.BirthDate.Year;

            if (age >= 16)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Member should be at least 16 years old to register");
            }
        }
    }
}