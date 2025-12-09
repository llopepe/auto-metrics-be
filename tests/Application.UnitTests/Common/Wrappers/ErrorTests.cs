using Core.Framework.Aplication.Common.Enums;
using Core.Framework.Aplication.Common.Wrappers;

namespace Application.UnitTests.Common.Wrappers
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {

            var errorCode = ErrorCodeResponse.BadRequest;
            var description = "Campo inválido";
            var fieldName = "Email";

            var error = new Error(errorCode, description, fieldName);

            Assert.AreEqual(errorCode, error.ErrorCode);
            Assert.AreEqual(description, error.Description);
            Assert.AreEqual(fieldName, error.FieldName);
        }

        [Test]
        public void Properties_ShouldBeReadableAndWritable()
        {

            var error = new Error(ErrorCodeResponse.NotFound);

            error.ErrorCode = ErrorCodeResponse.InternalServerError;
            error.Description = "Error interno";
            error.FieldName = "Password";


            Assert.AreEqual(ErrorCodeResponse.InternalServerError, error.ErrorCode);
            Assert.AreEqual("Error interno", error.Description);
            Assert.AreEqual("Password", error.FieldName);
        }
    }
}
