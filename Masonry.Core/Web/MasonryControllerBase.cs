﻿/*
The MIT License (MIT)
Copyright (c) 2012 Denys Vuika

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Composition;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Masonry.Core.Services;

namespace Masonry.Core.Web
{
  public abstract class MasonryControllerBase : Controller
  {
    [Import]
    public virtual ISecurityService Security { get; set; }

    [Import]
    public virtual ILocalizationService Localization { get; set; }

    [NonAction]
    public virtual bool IsAjaxRequest()
    {
      return Request.IsAjaxRequest();
    }

    protected override void Initialize(RequestContext requestContext)
    {

      string cultureName = null;
      var request = requestContext.HttpContext.Request;

      // Attempt to read the culture cookie from Request
      var cultureCookie = request.Cookies["_culture"];
      if (cultureCookie != null)
        cultureName = cultureCookie.Value;
      else if (request.UserLanguages != null)
        cultureName = request.UserLanguages[0];

      if (Localization != null)
      {
        // Validate culture name
        cultureName = Localization.GetImplementedCulture(cultureName); // This is safe
      }

      if (!string.IsNullOrWhiteSpace(cultureName))
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
      }

      base.Initialize(requestContext);
    }
  }
}
