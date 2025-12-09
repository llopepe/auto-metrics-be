using Core.Framework.Aplication.Common.Enums;
using Core.Framework.Aplication.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Wrappers
{

    [TestFixture]
    public class ResultResponseTests
    {
        // Ok() retorna Success = true y Errors vacíos
        [Test]
        public void Ok_ShouldReturnSuccessTrue_AndEmptyErrors()
        {
            var result = ResultResponse.Ok();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(0, result.Errors.Count);
        }

        // Failure() retorna Success = false y Errors vacíos
        [Test]
        public void Failure_NoParams_ShouldReturnSuccessFalse_AndEmptyErrors()
        {
            var result = ResultResponse.Failure();

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(0, result.Errors.Count);
        }

        //Failure(Error) asigna el error
        [Test]
        public void Failure_WithError_ShouldContainTheError()
        {
            var error = new Error(ErrorCodeResponse.CommandError, "Campo obligatorio", "Name");
            var result = ResultResponse.Failure(error);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreSame(error, result.Errors.First());
        }

        //Failure(IEnumerable<Error>) asigna los errores
        [Test]
        public void Failure_WithErrorsList_ShouldContainAllErrors()
        {
            var errors = new List<Error>
            {
                new(ErrorCodeResponse.NotFound, "No encontrado"),
                new(ErrorCodeResponse.Unauthorized, "Sin permiso")
            };

            var result = ResultResponse.Failure(errors);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(ErrorCodeResponse.NotFound, result.Errors[0].ErrorCode);
            Assert.AreEqual(ErrorCodeResponse.Unauthorized, result.Errors[1].ErrorCode);
        }

        //Operador implícito: Error -> ResultResponse
        [Test]
        public void ImplicitOperator_Error_ShouldCreateFailureResult()
        {
            var error = new Error(ErrorCodeResponse.InternalServerError, "Error implicit");
            ResultResponse result = error;

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreSame(error, result.Errors[0]);
        }

        //Operador implícito: List<Error> -> ResultResponse
        [Test]
        public void ImplicitOperator_ErrorList_ShouldCreateFailureResult()
        {
            var errors = new List<Error>
            {
                new(ErrorCodeResponse.FieldDataInvalid, "E1"),
                new(ErrorCodeResponse.NotFound, "E2")
            };

            ResultResponse result = errors;

            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(ErrorCodeResponse.FieldDataInvalid, result.Errors[0].ErrorCode);
            Assert.AreEqual(ErrorCodeResponse.NotFound, result.Errors[1].ErrorCode);
        }

        //AddError agrega error y Success = false
        [Test]
        public void AddError_ShouldAddError_AndSetSuccessFalse()
        {
            var result = ResultResponse.Ok();
            var error = new Error(ErrorCodeResponse.BadRequest, "Added error");

            result.AddError(error);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreSame(error, result.Errors[0]);
        }

        [Test]
        public void AddError_WhenErrorsIsNull_ShouldInitializeAndAddError()
        {
            var result = new ResultResponse(); // Errors es null por defecto
            Error error = new(ErrorCodeResponse.BadRequest, "Test error");

            var returnedResult = result.AddError(error);

            Assert.IsFalse(returnedResult.Success, "Success should be false after adding an error");
            Assert.IsNotNull(returnedResult.Errors, "Errors should be initialized if it was null");
            Assert.AreEqual(1, returnedResult.Errors.Count, "Errors list should contain the added error");
            Assert.AreEqual(error, returnedResult.Errors[0], "The added error should match the provided error");
        }

        [Test]
        public void AddError_WhenErrorsIsNotNull_ShouldAddErrorToExistingList()
        {
            // Arrange
            var existingError = new Error(ErrorCodeResponse.InternalServerError, "Existing error");
            var result = new ResultResponse
            {
                Errors = new List<Error> { existingError }
            };
            var newError = new Error(ErrorCodeResponse.BadRequest, "New error");


            result.AddError(newError);

            Assert.IsFalse(result.Success, "Success should be false after adding an error");
            Assert.AreEqual(2, result.Errors.Count, "Errors list should contain both existing and new errors");
            Assert.AreEqual(existingError, result.Errors[0]);
            Assert.AreEqual(newError, result.Errors[1]);
        }
    }
}
