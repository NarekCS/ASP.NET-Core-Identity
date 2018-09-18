using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private ProtectedDocument[] docs = new ProtectedDocument[]
        {
            new ProtectedDocument { Title = "Q3 Budget", Author = "Alice", Editor = "Joe" },
            new ProtectedDocument { Title = "Project Plan", Author = "Bob", Editor = "Alice" }
        };

        private IAuthorizationService authService;
        public DocumentController(IAuthorizationService auth) { authService = auth; }

        public ViewResult Index() => View(docs);

        //public ViewResult Edit(string title)
        //{
        //    return View("Index", docs.FirstOrDefault(d => d.Title == title));
        //}

        public async Task<IActionResult> Edit(string title)
        {
            ProtectedDocument doc = docs.FirstOrDefault(d => d.Title == title);
            AuthorizationResult authorized = await authService.AuthorizeAsync(User, doc, "AuthorsAndEditors");
            if (authorized.Succeeded)
            {
                return View("Index", doc);
            }
            else
            {
                return new ChallengeResult();
            }
        }
    }
}
