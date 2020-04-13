namespace Extended.WebApi.Models
{
    public interface IValidationResultType
    {
        public const string GeneralException = nameof(GeneralException);
        public const string ConflictException = nameof(ConflictException);
        public const string NotFoundException = nameof(NotFoundException);
        public const string ValidationException = nameof(ValidationException);
    }
}