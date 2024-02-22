using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Gameapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;

namespace Gameapp.Areas.Admin.Pages
{
    public class ParticipationListModel : PageModel
    {

        [BindProperty]
        public  ChampionParticipate  ChampionP { get; set; }
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;

        public ParticipationListModel(GamesContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        public IActionResult OnGet(int id)
        {
            try
            {
                ChampionP = _context.ChampionParticipates.Where(e => e.ChampionId == id).FirstOrDefault();

                if (ChampionP == null)
                {
                    return Redirect("../NotFound");
                }
            }

            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong");
            }
            return Page();
        }
    }
}
