﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HelloMicroservice.Models;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase;
using System.IO;

namespace HelloMicroservice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBucket _bucket;

        public HomeController(ILogger<HomeController> logger, IBucketProvider bucketProvider)
        {
            _logger = logger;
            _bucket = bucketProvider.GetBucket("hellomicroservice");
        }

        public IActionResult Index()
        {
            var id = Guid.NewGuid().ToString();
            _bucket.Insert(new Document<dynamic>
            {
                Id = id,
                Content = new
                {
                    hello = "microservice",
                    foo = Path.GetRandomFileName()
                }
            });
            var doc = _bucket.Get<dynamic>(id);
            return View(doc);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
