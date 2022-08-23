using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reporter.CustomAtributes
{
    public class AllowedEmailExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedEmailExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            string email=value as string;

            if (email != null)
            {
                foreach (var ex in _extensions)
                {
                    if (!email.Contains(ex))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This email extension is not allowed!";
        }
    }

}
