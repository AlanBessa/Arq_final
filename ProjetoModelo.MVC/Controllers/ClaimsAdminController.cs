﻿using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjetoModelo.Infra.CrossCutting.Identity;
using ProjetoModelo.Infra.CrossCutting.Identity.Configuration;
using ProjetoModelo.Infra.CrossCutting.Identity.Context;
using ProjetoModelo.Infra.CrossCutting.MvcFilters;
using ProjetoModelo.MVC.ViewModels;

namespace ProjetoModelo.MVC.Controllers
{
    [ClaimsAuthorize("AdmClaims","True")]
    public class ClaimsAdminController : Controller
    {
        public ClaimsAdminController()
        {
        }

        public ClaimsAdminController(ApplicationUserManager userManager, IdentityContext dbContext)
        {
            DbContext = dbContext;
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private IdentityContext _dbContext;

        public IdentityContext DbContext
        {
            get { return _dbContext ?? HttpContext.GetOwinContext().GetUserManager<IdentityContext>(); }
            set { _dbContext = value; }
        }

        // GET: ClaimsAdmin
        public ActionResult Index()
        {
            return View(DbContext.Claims.ToList());
        }

        // GET: ClaimsAdmin/SetUserClaim
        public ActionResult SetUserClaim(string id)
        {
            ViewBag.Type = new SelectList
                (
                    DbContext.Claims.ToList(),
                    "Name",
                    "Name"
                );

            ViewBag.User = UserManager.FindById(id);

            return View();
        }

        // POST: ClaimsAdmin/SetUserClaim
        [HttpPost]
        public ActionResult SetUserClaim(ClaimViewModel claim, string id)
        {
            try
            {
                UserManager.AddClaimAsync(id, new Claim(claim.Type, claim.Value));

                return RedirectToAction("Details", "UsersAdmin", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: ClaimsAdmin/CreateClaim
        public ActionResult CreateClaim()
        {
            return View();
        }

        // POST: ClaimsAdmin/CreateClaim
        [HttpPost]
        public ActionResult CreateClaim(Claims claim)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Claims.Add(claim);
                    DbContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
