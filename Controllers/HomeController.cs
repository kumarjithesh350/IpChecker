using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test_Application.Models;
using System.Xml;
using Newtonsoft.Json;

namespace Test_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult UserDetails()
        {
            List<UserDetails> userList = new List<UserDetails>();

            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                userList = JsonConvert.DeserializeObject<List<UserDetails>>(jsonData) ?? new List<UserDetails>();
            }
            UserDetailsModel obj = new UserDetailsModel() { Users= userList };
            return View(userList);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        private static readonly string filePath = "UserDetails.json";
        [HttpPost]
        public ActionResult SaveBrowserData(UserDetails userDetails)
        {
            try
            {
                _logger.LogInformation("userDetails recieved");
                List<UserDetails> userList = new List<UserDetails>();

                if (System.IO.File.Exists(filePath))
                {
                    _logger.LogInformation($"{filePath}");
                    _logger.LogInformation("Reading existing data");
                    string existingData = System.IO.File.ReadAllText(filePath);

                    _logger.LogInformation("Read existing data" + existingData.Length);
                    if (!string.IsNullOrWhiteSpace(existingData))
                    {
                        userList = JsonConvert.DeserializeObject<List<UserDetails>>(existingData) ?? new List<UserDetails>();
                    }
                }

                userList.Add(userDetails);
                _logger.LogInformation("write back to file ");
                System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(userList, Newtonsoft.Json.Formatting.Indented));

                _logger.LogInformation("written successfully ");
                return Ok("User details saved successfully!" ); // HTTP 200
            }
            catch (Exception ex)
            {
                return Ok(ex.Message); // HTTP 500
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
