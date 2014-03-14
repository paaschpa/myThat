using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common.Extensions;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using myThat.ServiceModel.Request;

namespace myThat.ServiceModel.Validators
{
    public class MyRegistrationValidator : AbstractValidator<Registration>
    {
        //This will be injected
        public IUserAuthRepository UserAuthRepo { get; set; }

        public MyRegistrationValidator()
        {
            //User must supply a UserName and Password. Username cannot already exist
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().When(x => x.Email.IsNullOrEmpty());
                RuleFor(x => x.Email)
                    .Must(x => UserAuthRepo.GetUserAuthByUserName(x) == null)
                    .WithErrorCode("AlreadyExists")
                    .WithMessage("UserName already exists")
                    .When(x => !x.Email.IsNullOrEmpty());
            });
        }
    }
}
