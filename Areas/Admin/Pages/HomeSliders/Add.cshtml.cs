using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Areas.Admin.Pages.HomeSliders
{
    public class AddModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AddModel(GamesContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        
        
        public void OnGet()
        {
        }
        public IActionResult OnPost(Models.Slider model)
        {
            if (!ModelState.IsValid)
            {
                //return Page();
            }
            try
            {
                if (model.SliderTypeId == 1)
                {
                    model.EntityId = Request.Form["ItemId"];
                }
                if (model.SliderTypeId == 2)
                {
                    model.EntityId = Request.Form["ShopId"];
                }
               if (model.SliderTypeId == 3)
                {
                    model.EntityId = Request.Form["ChampionId"];
                }
                if (model.SliderTypeId == 4)
                {
                    model.EntityId = null;
                }
                 if (model.SliderTypeId == 5)
                {
                    if (model.EntityId== null)
                    {
                        ModelState.AddModelError("Validation", "enter link");
                        return Page();
                    }
                 
                }
                var uniqeFileName = "";
                if (Response.HttpContext.Request.Form.Files.Count() > 0)
                {
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images");
                    string ext = Path.GetExtension(Response.HttpContext.Request.Form.Files[0].FileName);
                    uniqeFileName = Guid.NewGuid().ToString("N") + ext;
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);
                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Response.HttpContext.Request.Form.Files[0].CopyTo(fileStream);
                    }
                    model.Pic = uniqeFileName;
                }
                _context.Slider.Add(model);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return Redirect("./Index");

        }
    }
}
