using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Memberships.Extensions;
using Web.Memberships.Models;

namespace Web.Memberships.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;

        public UserController()
        {

        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = new List<UserViewModel>();
            //use the extension method to get the users
            await users.GetUsers();

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new UserViewModel();
            return View(model);
        }

        //
        // POST: /User/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsActive = true,
                        RegistrationDate = DateTime.Now,
                        EmailConfirmed = true
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    AddErrors(result);
                }
            }
            catch { }

            // If we got this far something failed, re-display the form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Password = user.PasswordHash
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        if (!user.PasswordHash.Equals(model.Password))
                        {
                            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                        }

                        var result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        AddErrors(result);
                    }
                }
            }
            catch { }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Password = "Fake"
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(UserViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        var result = await UserManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {
                            //remove subscriptions as well 
                            var db = new ApplicationDbContext();
                            var subscriptions = db.UserSubscriptions.Where(x => x.UserId.Equals(user.Id));
                            db.UserSubscriptions.RemoveRange(subscriptions);
                            await db.SaveChangesAsync(); //persist 

                            return RedirectToAction("Index");
                        }
                        AddErrors(result);
                    }
                }
            }
            catch { }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}