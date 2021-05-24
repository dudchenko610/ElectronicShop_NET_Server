using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Constants
{
    public partial class Constants
    {
        public partial class Routes
        {
            public class Order
            {
                public const string ADD_ORDER = "addOrder";
                public const string ADD_NOT_AUTH_ORDER = "addNotauthOrder";
                public const string GET_MY_ORDERS = "getMyOrders";
                public const string GET_AUTH_ORDERS = "getAuthOrders";
                public const string GET_NOT_AUTH_ORDERS = "getNotauthOrders";
            }
        }
    }
}
