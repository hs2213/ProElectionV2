using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ProElectionV2.Controllers;

public class CultureController : Controller
{
    /// <summary>
    /// Causes culture change by modifying cookie and redirects to the specified URL.
    /// </summary>
    /// <param name="culture">Culture code to change to</param>
    /// <param name="returnUrl">URL to return to</param>
    /// <returns>Redirects to given URL.</returns>
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

        Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;

        return Redirect(returnUrl);
    }
}