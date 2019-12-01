using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Js").Include(
                //jQuery 2.2.3
                "~/Content/plugins/jQuery/jquery-2.2.3.min.js",
                //Bootstrap
                "~/Content/bootstrap/js/bootstrap.min.js",                
                "~/Content/plugins/datepicker/bootstrap-datepicker.js",
                "~/Content/plugins/datepicker/locales/bootstrap-datepicker.es.js",
                "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                //FastClick
                "~/Content/plugins/fastclick/fastclick.js",
                //AdminLTE App
                "~/Content/dist/js/app.min.js",
                //AdminLTE for demo purposes
                "~/Content/dist/js/demo.js",
                //Reloj del sistema
                "~/Scripts/reloj.js",
                //Restricciones para letras, numeros y decimales
                "~/Scripts/js/restricciones/restricciones.js",
                "~/Scripts/sweetalert2.min.js"
                ));

            bundles.Add(new StyleBundle("~/Css").Include(
                //Bootstrap
                "~/Content/bootstrap/css/bootstrap.min.css",
                //Font Awesome
                "~/Content/dist/css/font-awesome.min.css",
                //DataTables
                "~/Content/plugins/datepicker/datepicker3.css",
                //Theme style
                "~/Content/dist/css/AdminLTE.min.css",
                //AdminLTE Skins: Elija una máscara de la carpeta css/skins en lugar de descargar todas para reducir la carga.
                "~/Content/dist/css/skins/_all-skins.min.css",
                //Reloj del sistema
                "~/Content/Reloj.css",
                "~/Content/sweetalert2.min.css"
                ));

            BundleTable.EnableOptimizations = true;

        }
    }
}
