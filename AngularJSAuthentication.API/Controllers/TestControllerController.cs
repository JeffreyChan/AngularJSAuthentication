using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJSAuthentication.API.Identity;
using AngularJSAuthentication.API.Logger;
using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Repository;

namespace AngularJSAuthentication.API.Controllers
{
    public class TestController : ApiController
    {
        private readonly IMyLogger _logger;

        private readonly IAuthRepository _authRepository;

        public TestController(IMyLogger logger, IAuthRepository authRepository)
        {
            this._logger = logger;
            this._authRepository = authRepository;
        }

        public string Get()
        {
            this._logger.Write("Inside the 'Get' method of the '{0}' controller.", GetType().Name);

            return "Hello, world!";
        }
    }
}
