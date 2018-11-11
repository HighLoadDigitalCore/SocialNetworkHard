using System.Web;
using System.Web.Optimization;

namespace sexivirt.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/fineuploader")
                    .Include("~/Scripts/jquery.fineuploader-4.1.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/fineuploader")
                             .Include("~/Content/css/fineuploader-4.1.1.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery2").Include(
                        "~/Scripts/jquery-2.1.0.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                     "~/Scripts/jquery-ui-1.10.4.custom.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js", "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/style.css",
                      "~/Content/css/jquery.formstyler.css"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryui").Include(
                     "~/Content/css/jquery-ui-1.10.0.custom.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                   "~/Content/css/bootstrap.css",
                   "~/Content/css/datepicker.css"));

            bundles.Add(new StyleBundle("~/Content/style_bootstrap").Include(
                   "~/Content/css/style_bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css/admin").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/PagedList.css",
                "~/Content/css/admin.css", "~/Content/css/datepicker.css"));
        }
    }
}
