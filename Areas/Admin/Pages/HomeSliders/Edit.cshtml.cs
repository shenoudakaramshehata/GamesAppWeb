using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gameapp.Data;
using Gameapp.Models;

namespace Gameapp.Areas.Admin.Pages.HomeSliders
{
    public class EditModel : PageModel
    {
        private GamesContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EditModel(GamesContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        

        [BindProperty]
        public Slider slider { get; set; }
        [BindProperty]
        public int EntityId { get; set; }
        [BindProperty]
        public int? ChampionId { get; set; }
        public int? ShopId { get; set; }

        public int? ItemId { get; set; }
        public string Externallink { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            slider = _context.Slider.Where(c => c.SliderId == id).FirstOrDefault();
            if (slider == null)
            {
                return NotFound();
            }
            if (slider.SliderTypeId == 5)
            {
                Externallink = slider.EntityId;
                //EntityId = 0;
            }
            if (slider.SliderTypeId == 1)
            {
                if (slider.EntityId != null && slider.EntityId != "")
                {
                    ItemId = int.Parse(slider.EntityId);
                }

            }
            if (slider.SliderTypeId == 2)
            {
                if (slider.EntityId != null && slider.EntityId != "")
                {
                    ShopId = int.Parse(slider.EntityId);
                }
            }
            if (slider.SliderTypeId == 3)
            {
                if (slider.EntityId != null && slider.EntityId != "")
                {
                ChampionId = int.Parse(slider.EntityId);
                }
            }

                if (slider.SliderTypeId > 3)
            {
                EntityId = 0;

            }
            

            return Page();
        }
        public IActionResult OnPost(int? id)
        
        {
            

            try
            {
                var model = _context.Slider.Where(c => c.SliderId == id).FirstOrDefault();
                if (model==null)
                {
                    return NotFound();
                }
                if (slider.SliderTypeId == 1)
                {
                    model.EntityId = Request.Form["ItemId"];
                }
                if (slider.SliderTypeId == 2)
                {
                    model.EntityId = Request.Form["ShopId"];
                }
               if (slider.SliderTypeId == 3)
                {
                    model.EntityId = Request.Form["ChampionId"];
                }
                if (slider.SliderTypeId == 4)
                {
                    model.EntityId = null;
                }

                if (slider.SliderTypeId == 5)
                {
                    if (slider.EntityId == null)
                    {
                        ModelState.AddModelError("Validation", "enter link");
                        return Page();
                    }
                    model.EntityId = slider.EntityId;

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
                    var ImagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images/" + model.Pic);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
                    model.Pic = uniqeFileName;
                }
                model.SliderTypeId = slider.SliderTypeId;
                model.IsActive = slider.IsActive;
                model.OrderIndex= slider.OrderIndex;
                model.CountryId = slider.CountryId;

                _context.Attach(model).State = EntityState.Modified;
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
