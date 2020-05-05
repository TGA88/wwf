using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublisherWeb.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

using PublisherWeb.ClientApi;

namespace PublisherWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TodoItemClient _TodoClient;
        public HomeController(ILogger<HomeController> logger, TodoItemClient client)
        {
            _logger = logger;
            _TodoClient = client;


        }

        public async Task<IActionResult> Index()
        {
            _TodoClient.BaseUrl = "http://localhost:5002";
            var res = _TodoClient.GetTodoItemAsync(1);

            TodoItem ret = await res;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("TestClient")]
        public async Task<IActionResult> TestClient()
        {
            // _TodoClient.BaseUrl = "http://localhost:5004";
            _TodoClient.BaseUrl = "http://localhost:5002";
            var res = _TodoClient.GetTodoItemAsync(1);
            // var res = _TodoClient.GetTodoItemsAsync();


            ViewData["Data"] = (TodoItem)await res;
            return View();
        }

        [Route("TestClientWithAuth")]
        [Authorize]
        public async Task<IActionResult> TestClientWithAuth()
        {
            // _TodoClient.BaseUrl = "http://localhost:5004";
            _TodoClient.BaseUrl = "http://localhost:5002";
            var res = _TodoClient.GetTodoItemsAsync();
            // var res = _TodoClient.GetTodoItemsAsync();


            ViewBag.Results = (IEnumerable<TodoItem>)await res;
            return View();
        }

        [Route("SignOut")]
        [Authorize]
        public IActionResult Logout()
        {
             return SignOut(new AuthenticationProperties{ RedirectUri = "/TestAfterLogout"},"Cookies", "oidc");
           // return SignOut("Cookies", "oidc");
        }

        [Route("TestAfterLogout")]
        public  IActionResult TestAfterLogout()
        {
            return  View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
