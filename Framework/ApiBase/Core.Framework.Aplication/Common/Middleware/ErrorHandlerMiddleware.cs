using Core.Framework.Aplication.Common.Enums;
using Core.Framework.Aplication.Common.Exceptions;
using Core.Framework.Aplication.Common.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Core.Framework.Aplication.Common.Middleware
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = ResultResponse.Failure();

                switch (error)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        foreach (var validationFailure in e.Errors)
                        {
                            responseModel.AddError(new Error(ErrorCodeResponse.ModelStateNotValid, validationFailure.ErrorMessage, validationFailure.PropertyName));
                            _logger.LogError(e, "ValidationException {ErrorMessage} {PropertyName}", validationFailure.ErrorMessage, validationFailure.PropertyName);
                        }
                        break;
                    case CommandException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.AddError(new Error(ErrorCodeResponse.CommandError, e.Message));
                        _logger.LogError(e, "CommandException");
                        break;
                    case RestApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.AddError(new Error(ErrorCodeResponse.CommandError, e.Message));
                        _logger.LogError(e, "RestApiException");
                        break;
                    // 400 - Bad Request
                    case BadRequestException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.AddError(new Error(ErrorCodeResponse.FieldDataInvalid, e.Message));
                        _logger.LogError(e, "BadRequestException");
                        break;
                    // 422 - Unprocessable Entity (validaciones de negocio / campos invalidos)
                    case UnprocessableEntityException e:
                        response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        responseModel.AddError(new Error(ErrorCodeResponse.UnprocessableEntity, e.Message));
                        _logger.LogError(e, "UnprocessableEntityException");
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.AddError(new Error(ErrorCodeResponse.NotFound, e.Message));
                        _logger.LogError(e, "KeyNotFoundException");
                        break;
                    case OutOfMemoryException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.AddError(new Error(
                            ErrorCodeResponse.OutOfMemory,
                            "Lo siento el sistema no pudo completar su solicitud, Intentelo mas tarde o contactese con el administrador."
                        ));
                        _logger.LogCritical(e, "OutOfMemoryException: Falta de memoria al procesar la solicitud. Requiere revisi�n.");
                        break;
                    case InvalidOperationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.AddError(new Error(ErrorCodeResponse.NotFound, e.Message));
                        _logger.LogCritical(e, "InvalidOperationException");
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.AddError(new Error(
                            ErrorCodeResponse.Unauthorized,
                            "Acceso no autorizado. El token es invalido, ha expirado o no fue provisto."
                        ));
                        _logger.LogWarning(e, "UnauthorizedAccessException capturada (middleware)");
                        break;


                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.AddError(new Error(ErrorCodeResponse.Exception, error.Message));
                        _logger.LogError("Excepci�n capturada en default: {Type}", error.GetType().FullName);

                        break;
                }
                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(result);
            }
        }
    }
}
