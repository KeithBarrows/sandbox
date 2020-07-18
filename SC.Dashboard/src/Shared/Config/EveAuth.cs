using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Dashboard.Shared.Config
{
    public class EveAuth
    {
        private IConfiguration _configuration;

        public EveAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string AuthorizationEndpoint => _configuration["EveAuth:AuthorizationEndpoint"];
        public string TokenEndpoint => _configuration["EveAuth:TokenEndpoint"];
        public string UserInformationEndpoint => _configuration["EveAuth:UserInformationEndpoint"];
        public string CallbackPath => _configuration["EveAuth:CallbackPath"];
        public string Scopes => _configuration["EveAuth:Scopes"];
    }
}
