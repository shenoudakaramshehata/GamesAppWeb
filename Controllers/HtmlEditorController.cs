using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gameapp.Controllers
{
    public class HtmlEditorController : Controller
    {
        public IActionResult OutputFormats()
        {
            return View();
        }
    }
}