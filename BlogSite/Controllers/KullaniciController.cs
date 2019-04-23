using BlogSite.Helpers;
using BlogSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//homecontroller/makale/kullanıcı/homecontroller güvenlik önlemi
namespace BlogSite.Controllers
{
    public class KullaniciController : YetkiliController
    {
        private BlogDB db = new BlogDB();
        // GET: Kullanici
        public  ActionResult Index()
        {
            string kullaniciad = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(x => x.KullaniciAdi == kullaniciad).SingleOrDefault();
            //ViewBag.deneme = kullanici.isim + kullanici.KayitTarihi;
            return View();
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int id)
        {
            var kisi = db.Kullanicis.Where(i => i.ID == id).SingleOrDefault();
            return View(kisi);
        }

        public ActionResult Profil()
        {
            string kullaniciadik = Session["username"].ToString();
            var kisi = db.Kullanicis.Where(x => x.KullaniciAdi == kullaniciadik).SingleOrDefault();
            return View(kisi);
        }
    
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }



        // GET: Kullanici/Edit/5
        public ActionResult Edit(int id)//buradan bize bir id geliyor
        {
            string kullaniciadi = Session["username"].ToString();
            var user = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();

            if (OrtakSinif.EdityYetki(id,user))
            {
                var kisi = db.Kullanicis.Where(i => i.ID == id).SingleOrDefault();//gelen id degerini burada kisiye atıyoruz
                return View(kisi);
            }
        
              
            
          
                return HttpNotFound();
          

        }

        // POST: Kullanici/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Kullanici model)
        {
            try
            {
                var kisi = db.Kullanicis.Where(i => i.ID == id).SingleOrDefault();
                kisi.isim = model.isim;
                kisi.Soyisim = model.Soyisim;
                kisi.Sifre = model.Sifre;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kullanici/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
