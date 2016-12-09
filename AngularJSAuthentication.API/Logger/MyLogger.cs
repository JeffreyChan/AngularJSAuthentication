using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Logger
{
    public class MyLogger : IMyLogger
    {
        public void Write(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }
    }
}