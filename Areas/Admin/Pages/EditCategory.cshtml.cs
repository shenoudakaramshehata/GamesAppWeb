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
using Microsoft.EntityFrameworkCore;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class EditCategoryModel : PageModel
    {
        private readonly GamesContext _context;
        private readonly IWebHostEnvironment _host;

        public EditCategoryModel(GamesContext context,  IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        [BindProperty]
        public Gameapp.Models.ViewModels.CategoryEditVm Category { get; set; }

        public void OnGet(int id)
        {
            var category = _context.Category.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);

            Category = new Models.ViewModels.CategoryEditVm();

            Category.CategoryTlar = category.CategoryTlar;
            Category.CategoryTlen = category.CategoryTlen;
            Category.IsActive = category.IsActive;
            Category.OrderIndex = category.OrderIndex;
            Category.CategoryId = category.CategoryId;

        }



        public async Task<IActionResult> OnPostAsync(Gameapp.Models.ViewModels.CategoryEditVm Category)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {

                Gameapp.Models.Category category = new Models.Category()
                {
                    CategoryTlar = Category.CategoryTlar,
                    CategoryTlen = Category.CategoryTlen,
                    IsActive = Category.IsActive,
                    OrderIndex = Category.OrderIndex,
                    CategoryId = Category.CategoryId
                };

                if (Category.CategoryPic != null)
                {
                    var uniqeFileName = "";

                    string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                    uniqeFileName = Guid.NewGuid() + "_" + Category.CategoryPic.FileName;

                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Category.CategoryPic.CopyTo(fileStream);
                    }

                    category.CategoryPic = uniqeFileName;
                }else
                {
                    category.CategoryPic = _context.Category.AsNoTracking().FirstOrDefault(c => c.CategoryId == Category.CategoryId).CategoryPic;
                }

                if (Category.CategoryIcon != null)
                {
                    var uniqeFileName = "";

                    string uploadFolder = Path.Combine(_host.WebRootPath, "images");

                    uniqeFileName = Guid.NewGuid() + "_" + Category.CategoryIcon.FileName;

                    string uploadedImagePath = Path.Combine(uploadFolder, uniqeFileName);

                    using (FileStream fileStream = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        Category.CategoryIcon.CopyTo(fileStream);
                    }

                    category.CategoryIcon = uniqeFileName;
                }
                else
                {
                    category.CategoryIcon = _context.Category.AsNoTracking().FirstOrDefault(c => c.CategoryId == Category.CategoryId).CategoryIcon;
                }

                _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }


            return Redirect("/Admin/Categories");
        }
    }
}
