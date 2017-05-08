using PranzoDoodle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PranzoDoodle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            carpooltoolEntities entities = new carpooltoolEntities();

            var sortedOptions = entities.PranzoOptions.ToList().OrderBy(o => o.defaultOption);

            return View("PranzoView", new UsersAndOptions(entities.PranzoUsers, sortedOptions));
        }


        [HttpPost]
        public ActionResult EditSchedule(IEnumerable<PranzoUsers> users, IEnumerable<PranzoUsers> oldModel)
        {
            carpooltoolEntities entities = new carpooltoolEntities();

            foreach (var user in users)
            {
                var oldUser = (from o in oldModel where o.id == user.id select o).FirstOrDefault();
                if(oldUser.nome.Equals(user.nome) && oldUser.preferenza == user.preferenza)
                {
                    continue;
                }

                entities.PranzoUsers.Attach(user);
                var entry = entities.Entry(user);
                entry.Property(e => e.nome).IsModified = true;
                entry.Property(e => e.preferenza).IsModified = true;
            }

            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUser(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                carpooltoolEntities entities = new carpooltoolEntities();
                try
                {
                    var option = (from o in entities.PranzoOptions where o.defaultOption == 1 select o).FirstOrDefault();

                    entities.PranzoUsers.Add(new PranzoUsers() { nome = userName.Trim(), preferenza = option.id });
                    entities.SaveChanges();
                }
                catch(Exception ex) {
                    ex.ToString();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteUser(int userId)
        {
            carpooltoolEntities entities = new carpooltoolEntities();
            var toRemove = (from u in entities.PranzoUsers where u.id == userId select u).FirstOrDefault();
            if(toRemove != null)
            {
                try
                {
                    entities.PranzoUsers.Remove(toRemove);
                    entities.SaveChanges();
                }
                catch { }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddOption(PranzoOptions option)
        {
            if (option != null && !string.IsNullOrWhiteSpace(option.descrizione))
            {
                carpooltoolEntities entities = new carpooltoolEntities();
                try
                {
                    option.defaultOption = 0;
                    entities.PranzoOptions.Add(option);
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteOption(PranzoOptions option)
        {
            if (option != null && option.defaultOption == 0)
            {
                carpooltoolEntities entities = new carpooltoolEntities();
                try
                {
                    //Reset users
                    var defaultOption = (from o in entities.PranzoOptions where o.defaultOption == 1 select o).FirstOrDefault();
                    foreach (var user in entities.PranzoUsers)
                    {
                        if (option.id == user.preferenza)
                        {
                            user.preferenza = defaultOption.id;
                        }
                    }

                    var toDelete = (from o in entities.PranzoOptions where o.id == option.id select o).FirstOrDefault();
                    entities.PranzoOptions.Remove(toDelete);
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Reset()
        {
            carpooltoolEntities entities = new carpooltoolEntities();
            try
            {
                var option = (from o in entities.PranzoOptions where o.defaultOption == 1 select o).FirstOrDefault();

                foreach (var user in entities.PranzoUsers)
                {
                    user.preferenza = option.id;
                }
                entities.SaveChanges();
            }
            catch { }

            return RedirectToAction("Index");
        }
    }
}