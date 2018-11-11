using System.Web.Mvc;

namespace sexivirt.Web.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Error",
                "error",
                new { controller = "Error", action = "Index" },
                new[] { "sexivirt.Web.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "404",
                "not-found-page",
                new { controller = "Error", action = "NotFoundPage" },
                new[] { "sexivirt.Web.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "Default_default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "sexivirt.Web.Areas.Default.Controllers" }
            );
        }
    }
}