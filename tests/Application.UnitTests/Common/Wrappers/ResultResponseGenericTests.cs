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
    public class ResultResponseGenericTests
    {
        private class TestData
        {
            public string Name { get; set; } = string.Empty;
        }

        [Test]
        public void Ok_ShouldSetSuccessAndData()
        {
            var data = new TestData { Name = "Test" };
            var result = ResultResponse<TestData>.Ok(data);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(data, result.Data);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void Failure_NoParameters_ShouldSetSuccessFalse()
        {
            var result = ResultResponse<TestData>.Failure();

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void Failure_WithSingleError_ShouldSetErrorAndSuccessFalse()
        {
            var error = new Error(ErrorCodeResponse.BadRequest, "Error message");
            var result = ResultResponse<TestData>.Failure(error);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(error, result.Errors[0]);
        }

        [Test]
        public void Failure_WithMultipleErrors_ShouldSetErrorsAndSuccessFalse()
        {
            var errors = new List<Error>
            {
                new(ErrorCodeResponse.BadRequest, "Error 1"),
                new(ErrorCodeResponse.InternalServerError, "Error 2")
            };

            var result = ResultResponse<TestData>.Failure(errors);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(errors[0], result.Errors[0]);
            Assert.AreEqual(errors[1], result.Errors[1]);
        }

        [Test]
        public void ImplicitOperator_FromData_ShouldReturnSuccessResult()
        {
            var data = new TestData { Name = "Implicit" };
            ResultResponse<TestData> result = data;

            Assert.IsTrue(result.Success);
            Assert.AreEqual(data, result.Data);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void ImplicitOperator_FromError_ShouldReturnFailureResult()
        {
            var error = new Error(ErrorCodeResponse.Unauthorized, "No access");
            ResultResponse<TestData> result = error;

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(error, result.Errors[0]);
        }

        [Test]
        public void ImplicitOperator_FromListOfErrors_ShouldReturnFailureResult()
        {
            var errors = new List<Error>
            {
                new(ErrorCodeResponse.NotFound, "Not found"),
                new(ErrorCodeResponse.Forbidden, "Forbidden")
            };

            ResultResponse<TestData> result = errors;

            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual(errors[0], result.Errors[0]);
            Assert.AreEqual(errors[1], result.Errors[1]);
        }
    }
}
