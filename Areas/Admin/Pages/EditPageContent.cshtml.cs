using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameapp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gameapp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]

    public class EditPageContentModel : PageModel
    {

        private GamesContext _context;


        public EditPageContentModel(GamesContext context)
        {
            _context = context;

        }

        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public int Id { get; set; }
        public void OnGet(int id)
        {
            var pageContent = _context.PageContent.FirstOrDefault(p => p.Id == id);


            if(pageContent != null)
            {
                Content = _context.PageContent.FirstOrDefault(p => p.Id == id).Content;
                Id = id;
            }

        }
    }
}
