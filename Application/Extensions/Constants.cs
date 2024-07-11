using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class Constants
    {
        public const string AuthenticationType = "JwtAuth";
        public const string BrowserStorageKey = "x-key";
        public const string HttpClientName = "WebUIClient";
        public const string HttpClientHeaderScheme = "Bearer";
        public static class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }
    }
}
