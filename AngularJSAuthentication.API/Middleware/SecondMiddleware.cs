using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AngularJSAuthentication.API.Logger;
using Microsoft.Owin;

namespace AngularJSAuthentication.API.Middleware
{
    public class SecondMiddleware : OwinMiddleware
    {
        private readonly IMyLogger _logger;

        public SecondMiddleware(OwinMiddleware next, IMyLogger logger)
            : base(next)
        {
            this._logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            this._logger.Write("Inside the 'Invoke' method of the '{0}' middleware.", GetType().Name);

            await Next.Invoke(context);
        }
    }
}