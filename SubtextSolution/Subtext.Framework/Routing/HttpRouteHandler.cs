﻿#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public class HttpRouteHandler<THandler> : IRouteHandler where THandler : IHttpHandler, new()
    {
        public HttpRouteHandler(THandler handler) {
            HttpHandler = handler;
        }

        public HttpRouteHandler()
        {
            HttpHandler = new THandler();
        }

        public IHttpHandler HttpHandler { 
            get; 
            private set; 
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            IRoutableHandler routableHandler = HttpHandler as IRoutableHandler;
            if (routableHandler != null) {
                routableHandler.RequestContext = requestContext;
            }

            return HttpHandler;
        }
    }
}
