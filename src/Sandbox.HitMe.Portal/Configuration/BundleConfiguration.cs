using System.Web.Optimization;

namespace Sandbox.HitMe.Portal.Configuration
{
    public static class BundleConfiguration
    {
        public static void Configure(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle("~/bundles/modernizr")
                    .Include("~/Scripts/modernizr-*")
                );

            bundles
                .Add(new ScriptBundle("~/bundles/scripts")
                    .Include("~/Scripts/jquery.signalr-{version}.js")
                    .Include("~/Scripts/angular.js")
                    .Include("~/Scripts/angular-resource.js")
                    .Include("~/Scripts/angular-touch.js")
                    .Include("~/Scripts/angular-cookies.js")
                    .Include("~/Scripts/angular-animate.js")
                    .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                    .Include("~/Scripts/angular-ui-router.js")
                    .IncludeDirectory("~/Scripts/antix/", "*.js", true)
                    .IncludeDirectory("~/Client/", "*.js", true)
                );

            bundles
                .Add(new StyleBundle("~/bundles/styles")
                    .Include("~/Content/animate.css")
                    .Include("~/Content/index.css")
                );
        }
    }
}