using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reporter.Data;
using Reporter.Models;

namespace Reporter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;


        
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index(int pg = 1)
        {
            var selling = _context.SalesTable.ToList();
            const int pageSize = 30;
            if (pg < 1)

                pg = 1;


            int recsCount = selling.Count();
            var pagination = new Pagination(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;


            var data = selling.Skip(recSkip).OrderByDescending(x => x.Date).Take(pagination.PageSize).ToList();
            this.ViewBag.Pagination = pagination;

            
            return View(data);
        }

    }
}
