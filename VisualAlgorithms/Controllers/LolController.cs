using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LolController : ControllerBase
    {
        [HttpGet]
        public void Get(int id)
        {
            Redirect("https://localhost:5001/Tests");
        }

        [HttpPost]
        public IActionResult Post(int a)
        {
            
            return RedirectToPage("Index");
        }
    }
}