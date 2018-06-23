using IdentityDemo.Infrastructure;
using IdentityDemo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentityDemo.Controllers
{
    public class AdminController : Controller
    {
        #region 建構

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion


        #region View

        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        #endregion


        #region API

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel inModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = inModel.Name, Email = inModel.Email };
                IdentityResult result = await UserManager.CreateAsync(user, inModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(inModel);
        }

        #endregion
    }
}