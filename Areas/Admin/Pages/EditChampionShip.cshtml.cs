using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages
{
    public class EditChampionShipModel : PageModel
    {


        private readonly IWebHostEnvironment _host;
        private GamesContext _context;


        public EditChampionShipModel(GamesContext context,
            ApplicationDbContext db,
            IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }


        [BindProperty]
        public Models.Champion Champion { get; set; }

        public IActionResult OnGet(int id)
        {

            Champion = _context.Champions.FirstOrDefault(s => s.ChampionId == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Gameapp.Models.Champion Champion)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var uniqeFileName = "";


            if (Response.HttpContext.Request.Form.Files.Count() > 0)
            {
                string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                uniqeFileName = Guid.NewGuid() + "_" + Response.HttpContext.Request.Form.Files[0].FileName;

                string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                {
                    Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                }

                Champion.ChampionPic = uniqeFileName;
            }else
            {
                Champion.ChampionPic = _context.Champions.AsNoTracking().FirstOrDefault(s => s.ChampionId == Champion.ChampionId).ChampionPic;
            }


            _context.Entry(Champion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Redirect("/admin/ChampionShip");
        }

    }
}
