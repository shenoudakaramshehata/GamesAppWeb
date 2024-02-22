using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class AddChampionShipModel : PageModel
    {
        private readonly IWebHostEnvironment _host;
        private GamesContext _context;


        public AddChampionShipModel(GamesContext context,
            ApplicationDbContext db,
            IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        [BindProperty]
        public Models.ViewModels.ChampionVm Champion { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Gameapp.Models.ViewModels.ChampionVm Champion)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

                var uniqeFileName = "";

            if (Champion.ChampionPic != null)
            {

                string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                uniqeFileName = Guid.NewGuid() + "_" + Champion.ChampionPic.FileName;

                string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                {
                    Champion.ChampionPic.CopyTo(fileStream);
                }

            }

            Gameapp.Models.Champion champion = new Models.Champion()
            {
                EndDate = Champion.EndDate,
                ChampionTlEn = Champion.ChampionTlEn,
                ChampionTlAR = Champion.ChampionTlAR,
                StartDate = Champion.StartDate,
                ChampionContent = Champion.ChampionContent,
                ChampionPic = uniqeFileName,
                ChampTypeId = Champion.ChampTypeId,
                GameModeId = Champion.GameModeId,
                JoinEnd = Champion.JoinEnd,
                JoinStart = Champion.JoinStart,
            };

            _context.Champions.Add(champion);
            _context.SaveChanges();
            return Redirect("/admin/ChampionShip");
        }
    }

}
