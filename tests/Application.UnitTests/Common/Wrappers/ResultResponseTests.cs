using Core.Framework.Aplication.Common.Enums;
using Core.Framework.Aplication.Common.Wrappers;

namespace Application.UnitTests.Common.Wrappers
{

    [TestFixture]
    public class ResultResponseTests
    {
        // OK()

        [Test]
        public void Ok_ShouldReturnSuccessTrue_AndEmptyErrors()
        {
            var result = ResultResponse.Ok();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(0, result.Errors.Count);
        }

        // Failure() sin parámetros

        [Test]
        public void Failure_NoParams_ShouldReturnSuccessFalse_AndEmptyErrors()
        {
            var result = ResultResponse.Failure();

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(0, result.Errors.Count);
        }

        // Failure(Error)

        [Test]
        public void Failure_WithError_ShouldContainTheError()
        {
            var error = new Error(ErrorCodeResponse.CommandError, "Campo obligatorio", "Name");
            var result = ResultResponse.Failure(error);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreSame(error, result.Errors[0]);
        }

        //  Failure(IEnumerable<Error>)

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

        // Operador implícito: List<Error> -> ResultResponse

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


        // Tests del branch Errors ??= []

        [Test]
        public void AddError_WhenErrorsIsNull_ShouldInitializeAndAddError()
        {
            var result = new ResultResponse(); // Errors = null
            var error = new Error(ErrorCodeResponse.BadRequest, "Test error");

            result.AddError(error);

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(error, result.Errors[0]);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void AddError_WhenErrorsIsNotNull_ShouldAddErrorToExistingList()
        {
            var existing = new Error(ErrorCodeResponse.InternalServerError, "Existing");
            var result = new ResultResponse
            {
                Errors = new List<Error> { existing } // Errors != null
            };

            var newError = new Error(ErrorCodeResponse.BadRequest, "New");

            result.AddError(newError);

            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(existing, result.Errors[0]);
            Assert.AreEqual(newError, result.Errors[1]);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ImplicitOperator_NotUsed_ShouldStillAllowNormalConstruction()
        {
            var result = new ResultResponse();

            Assert.IsFalse(result.Success);
            // Assert.IsNull(result.Errors);
        }
    }

}
