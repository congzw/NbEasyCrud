using System;
using System.Web.Mvc;
using ZQNB.Web.Models;

namespace ZQNB.Web.Controllers
{
    public abstract class BaseCrudController<TViewModel> : Controller where TViewModel : ICrudViewModel, new()
    {
        public ActionResult GetAll()
        {
            return View();
        }

        public ActionResult Get(Guid id)
        {
            return View();
        }

        public ActionResult Add()
        {
            var addVo = new TViewModel();
            return View(addVo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(TViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            //var assignedToUser = _context.Users.Single(u => u.Id == form.AssignedToUserID);

            //_context.Issues.Add(new Issue(_currentUser.User, assignedToUser, form.IssueType, form.Subject, form.Body));

            //_context.SaveChanges();

            //return RedirectToAction<HomeController>(c => c.Index())
            //    .WithSuccess("Issue created!");
            return View();
        }

        public ActionResult Delete(Guid id)
        {
            return View();
        }

        public ActionResult Edit(object editVo)
        {
            return View();
        }
    }
}