using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test_Application.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Test_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        const string connectionUri = "mongodb+srv://kumarjithesh350:qwvXy49TcG9R7lql@cluster0.im02ivu.mongodb.net/?retryWrites=true&w=majority";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveBrowserData(UserDetails docs)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            var client = new MongoClient(settings);

            // Send a ping to confirm a successful connection
            try
            {

                // automatically when you first write data.
                var dbName = "test_table";
                var collectionName = "UserDetails";

                var collection = client.GetDatabase(dbName)
                   .GetCollection<UserDetails>(collectionName);


                //var docs = UserDetails.GetUserDetails();

                try
                {
                    collection.InsertMany([docs]);

                    return Content("data Saved successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong trying to insert the new documents." +
                        $" Message: {e.Message}");
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
                return Content("data Save failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); 
                return Content("data Save failed."); 
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
