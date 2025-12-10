namespace Core.Framework.Aplication.Common.Enums
{
    public enum ErrorCodeResponse : short
    {

        ModelStateNotValid = 400,
        BadRequest = 400,
        CommandError = 400,
        Unauthorized = 401,
        AccessDenied = 403,
        Forbidden = 403,
        NotFound = 404,
        FieldDataInvalid = 400,/*422,*/
        UnprocessableEntity = 400,//422,
        InternalServerError = 500,
        ErrorInIdentity = 500,
        Exception = 500,
        OutOfMemory = 500,

    }
}
