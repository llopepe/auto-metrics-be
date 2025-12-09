using Core.Framework.Aplication.Common.Enums;

namespace Core.Framework.Aplication.Common.Wrappers
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Error
    {
        public Error(ErrorCodeResponse errorCode, string? description = null, string? fieldName = null)
        {
            ErrorCode = errorCode;
            Description = description;
            FieldName = fieldName;
        }

        public ErrorCodeResponse ErrorCode { get; set; }
        public string? FieldName { get; set; }
        public string? Description { get; set; }
    }
}
