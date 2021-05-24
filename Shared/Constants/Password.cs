using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Constants
{
    public partial class Constants
    {
        public class Password
        {
            public const string VALID = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            public const string PASSWORD_VALID = ".@abcdefghijklmnopqrstuvwxyz1234567890";
            public const int LENGTH_PASSWORD = 10;
            public const int MIN_LENGTH_PASSWORD = 5;
        }
    }
    
}
