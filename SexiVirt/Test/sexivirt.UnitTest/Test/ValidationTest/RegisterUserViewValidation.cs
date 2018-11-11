using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sexivirt.Web.Models.ViewModels.User;
using sexivirt.UnitTest.Tools;

namespace sexivirt.UnitTest
{
    [TestFixture]
    public class RegisterUserViewValidation
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_EmailIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,

                Email = string.Empty,
                Password = "123456",
                ConfirmPassword = "123456",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "ValidEmailAttribute")]
        public void Validate_EmailIsNotCorrect_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "chernikov",
                Password = "123456",
                ConfirmPassword = "123456",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "UserEmailValidationAttribute")]
        public void Validate_EmailIsAlreadyExists_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "chernikov@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [Test]
        public void Validate_EmailIsAlreadyExistsButCanEdit_PassValidate()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 2,
                Email = "chernikov@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
            Assert.IsNotNull(registerUserView);
        }

        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_PasswordIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "",
                ConfirmPassword = "123456",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_ConfirmPasswordIsEmpty_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }

        [ExpectedException(ExpectedException = typeof(Validator.ValidatorException), UserMessage = "RequestAttribute")]
        public void Validate_ConfirmPasswordDoesNotMatch_Fail()
        {
            var registerUserView = new RegisterUserView()
            {
                ID = 0,
                Email = "rollinx@gmail.com",
                Password = "123456",
                ConfirmPassword = "654321",
            };
            Validator.ValidateObject<RegisterUserView>(registerUserView);
        }
    }
}
