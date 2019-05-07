using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlockChainClient.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BlockChainClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MakeTransaction()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult MakeTransaction1()
        {
            ViewData["Message"] = "";
            return View();
        }

        [HttpPost]
        public IActionResult ViewTransaction(string node_url)
        {
            var url = new Uri(node_url + "/chain");
            ViewBag.Blocks = GetChain(url);
            return View();
        }

        private List<Block> GetChain(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var model = new
                {
                    chain = new List<Block>(),
                    length = 0
                };
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var data = JsonConvert.DeserializeAnonymousType(json, model);

                return data.chain;
            }
            return null;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
