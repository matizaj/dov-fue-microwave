namespace Microwave.Infrastructure.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SanitizedInput { get; set; }

        public static ValidationResult Success(string? sanitizedInput = null)
        {
            return new ValidationResult
            {
                IsValid = true,
                SanitizedInput = sanitizedInput
            };
        }

        public static ValidationResult Failure(string errorMessage)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
