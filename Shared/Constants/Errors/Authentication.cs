using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Constants
{

    public partial class Constants
    {
        public partial class Errors
        {
            public class Authentication
            {

                public const string PLEASE_LOGIN = "Login pleae again";


                public const string USER_WAS_NOT_REGISTERED = "User was not registered";
                public const string USER_WAS_NOT_ADDED_TO_ROLE = "User was not added to role";
                public const string TOKEN_OUT_OF_DATE = "Please, login to your account";
                public const string INVALID_PASSWORD = "Password is invalid";

                public const string LOGIN_INCORRECT = "Incorrect login";
                public const string USER_ALREADY_EXISTS = "User already exists";

                public const string PHONE_NUMBER_IS_EMPTY = "Phone number is empty";
                public const string NAME_IS_REQUIRED = "First name is required";
                public const string SURNAME_IS_REQUIRED = "Last name is required";
                public const string AGE_IS_REQUIRED = "Age is required";
                public const string PHONE_NUMBER_IS_REQUIRED = "Phone number is required";
                public const string EMAIL_IS_REQUIRED = "Email is required";
                public const string PASSWORD_IS_REQUIRED = "Password is required";
            }
        }
    }

}
