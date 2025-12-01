using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.PresentationLayer.Validation
{
    // The class tells if the validation passed or failed and stores an error message if it failed
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
        // Creates a successful validation result
        public static ValidationResult Success()
        {
            return new ValidationResult(true, string.Empty);
        }

        // Creates a failed validation result with a given error message
        public static ValidationResult Fail(string message)
        {
            return new ValidationResult(false, message);
        }

    }
}
