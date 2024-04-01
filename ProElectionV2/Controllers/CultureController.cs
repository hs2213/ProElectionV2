using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ProElectionV2.Controllers;

public class CultureController : Controller
{
    [Route("culture/change")]
    public IActionResult Change(string culture, string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(culture))
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return BadRequest();
        }

        string cookieName = CookieRequestCultureProvider.DefaultCookieName;
        string cookieValue = CookieRequestCultureProvider
            .MakeCookieValue(new RequestCulture(culture, culture));

        HttpContext.Response.Cookies.Append(cookieName, cookieValue);

        Console.WriteLine(CultureInfo.CurrentCulture);

        Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;

        return Redirect(returnUrl);
    }
}