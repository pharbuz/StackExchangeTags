using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchangeTags.Models;
using StackExchangeTags.Services;

namespace StackExchangeTags.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiCallService _service;

        public HomeController(ILogger<HomeController> logger, IApiCallService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return RedirectToAction("TagList");
        }
        
        public IActionResult IndexPage()
        {
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> TagList(int pageNumber, int pageSize, string sortDirection, string sortType,
            string site)
        {
            PageResult<StackExchangeTagList<StackExchangeTag>> result;
            if (pageNumber == 0
                || pageSize == 0
                || string.IsNullOrEmpty(sortDirection)
                || string.IsNullOrEmpty(sortType)
                || string.IsNullOrEmpty(site))
            {
                result = await _service.GetTags(pageNumber: 1, pageSize: 1000, sortDirection: "desc",
                    sortType: "popular", site: "stackoverflow");
            }
            else
            {
                result = await _service.GetTags(pageNumber: pageNumber, pageSize: pageSize,
                    sortDirection: sortDirection, sortType: sortType, site: site);
            }

            long numberOfTagsInSelectedSample = result.Items.Items.Sum(x => x.Count);

            ViewBag.NumberOfTags = numberOfTagsInSelectedSample;

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}