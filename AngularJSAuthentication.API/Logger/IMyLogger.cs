using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Logger
{
    public interface IMyLogger
    {
        void Write(string message, params object[] args);
    }
}