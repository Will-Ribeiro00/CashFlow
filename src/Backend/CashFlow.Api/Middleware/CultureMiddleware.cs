using System.Globalization;

namespace CashFlow.Api.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        public CultureMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var supportedLenguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            var RequestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var cultureInfo = new CultureInfo("en");
            
            if (!string.IsNullOrWhiteSpace(RequestCulture) && supportedLenguages.Exists(l => l.Name.Equals(RequestCulture)))
            {
                cultureInfo = new CultureInfo(RequestCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
