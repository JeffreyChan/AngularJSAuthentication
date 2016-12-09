using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AngularJSAuthentication.API.Logger;
using Autofac.Integration.WebApi;

namespace AngularJSAuthentication.API.Filter
{
    public class CustomActionFilter : IAutofacActionFilter
    {
        private readonly IMyLogger _logger;

        public CustomActionFilter(IMyLogger logger)
        {
            this._logger = logger;
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            this._logger.Write("Inside the 'OnActionExecutedAsync' method of the custom action filter.");
            return Task.FromResult(0);
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            this._logger.Write("Inside the 'OnActionExecutingAsync' method of the custom action filter.");
            return Task.FromResult(0);
        }
    }
}