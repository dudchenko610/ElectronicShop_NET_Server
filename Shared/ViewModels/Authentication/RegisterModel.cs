using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Authentication
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Age { get; set; }
        public string Password { get; set; }
    }
}
