using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestMasterASPFramework.Models;
using TestMasterInfrastructure.Models;
using TestMasterInfrastructure.Wrappers;

namespace TestMasterASPFramework.Controllers
{
    public class HomeController : Controller
    {
        // private IConfigWrapper _configWrapper;

        // public HomeController(IConfigWrapper configWrapper)
        // {
        //     _configWrapper = configWrapper;
        // }
        
        public ActionResult Index()
        {
            var connstr = ConfigurationManager.ConnectionStrings["notexist"];

            var x = new EasyModel() { somthEasy = "sometheasy" };
            return View(x);
        }

        public ActionResult About(string value)
        {
            ViewBag.Message = "Your application description page.";

            var secret = ConfigurationManagerResolver.GetAppSettings("");
            var secret2 = ConfigurationManagerResolver.GetAppSettings(value);
            var secret3 = ConfigurationManagerResolver.GetAppSettings<ConnectionStringSettings>(value);

            var model = new ConfigModel
            {
                FullSecret = secret,
                SomeParameter = secret2,
                ConnectionString = secret3
            };

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}