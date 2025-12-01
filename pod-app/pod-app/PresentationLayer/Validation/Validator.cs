using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pod_app.BusinessLogicLayer;

    namespace pod_app.PresentationLayer.Validation
    {
        // A own class for the formal validation in the UI
        public static class Validator
        {
            // Validator for the format of the podcast
            public static ValidationResult ValidatePodcastForm(string? title, string? category)
            {
                if (string.IsNullOrWhiteSpace(title))
                    return ValidationResult.Fail("Du måste ange ett namn på podden.");

                if (title!.Length > 30)
                    return ValidationResult.Fail("Namnet är för långt (max 30 tecken).");

                if (string.IsNullOrWhiteSpace(category))
                    return ValidationResult.Fail("Du måste ange en kategori.");

                if (category!.Length > 30)
                    return ValidationResult.Fail("Kategorin är för lång (max 30 tecken).");

                return ValidationResult.Success();
            }

            // Validator for the URL-Search
            public static ValidationResult ValidateSearchQuery(string? query)
            {
                if (string.IsNullOrWhiteSpace(query))
                    return ValidationResult.Fail("Du måste ange en RSS-adress att söka på.");

                if (!RssUtilHelpers.IsvalidXmlUrl(query))
                    return ValidationResult.Fail("Ogiltig RSS-adress. Kontrollera URL:en.");

                return ValidationResult.Success();
            }

            // Validator for the connectionString
            public static ValidationResult ValidateConnectionString(string? connStr)
            {
                if (string.IsNullOrWhiteSpace(connStr))
                    return ValidationResult.Fail("Du måste ange en connection string.");

                return ValidationResult.Success();
            }
        }
    }



