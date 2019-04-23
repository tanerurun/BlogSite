using BlogSite.Helpers;
using BlogSite.Models;
using BlogSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//homecontroller/kullanici/makale/
namespace BlogSite.Controllers
{
    public class MakaleController : YetkiliController
    {
        BlogDB db = new BlogDB();
        // GET: Makale
        public ActionResult Index(string AramaYap=null , int KategoriId=0)
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            var makaleler = from a in db.Makales
                            select a;
            if(KategoriId != 0)
            {//kategoriye gore arama
                makaleler = makaleler.Where(i => i.KategoriID == KategoriId);

            }
            if(!string.IsNullOrEmpty(AramaYap))
            {
                makaleler = makaleler.Where(i => i.Baslik.Contains(AramaYap));
            }
            return View(makaleler);
        }

        // GET: Makale/Details/5
        public ActionResult Details(int id)
        {
            //gelen id ile hangi makale olduğunu çekeceğiz ilk başta 
            var makale = db.Makales.Where(i => i.ID == id).SingleOrDefault();//burada geleni makaleye attık
            if(makale==null)
            {
                return HttpNotFound();
            }
            SonAtilanMakaleViewModel vm = new SonAtilanMakaleViewModel();
            vm.Makalem = makale;
           // vm.SonMakaleler = db.Makales.OrderByDescending(i => i.Tarih).Take(5).ToList();

            return View(vm);//bastıktan sonra göster
        }

        public ActionResult KisiMakaleListele()
        {
            var kullaniciadi = Session["username"].ToString();//burada kullanıcıyı yakalıyoruz
            //alt satıda yakalana kullanıcının değerleirni makaleler atıyoruz ve onu da viewde gosteriyoruz.olay bundan ibaret
            var makaleler = db.Kullanicis.Where(a => a.KullaniciAdi == kullaniciadi).SingleOrDefault().Makales.ToList();

            return View(makaleler);
        }
        // GET: Makale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriID", "KategoriAdi");
            return View();
        }

        // POST: Makale/Create
        [HttpPost]
        public ActionResult Create(Makale model,string etiketler)
        {
            try
            {
                // TODO: Add insert logic here
                string kullanicix = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullanicix).SingleOrDefault();
                model.KullaniciID = kullanici.ID;
                model.Tarih = DateTime.Now;
                db.Makales.Add(model);//database ekle diyoruz 
                if (!string.IsNullOrEmpty(etiketler))
                {
                    string[] etiketdizi = etiketler.Split(',');

                     foreach(var i in etiketdizi)
                    {
                        var yenietiket = new Etiket { EtiketAd = i };
                     var data= db.Etikets.Add(yenietiket);
                        model.Etikets.Add(yenietiket);
                    }
                }
              
                db.SaveChanges();//en son kayıt et diyoruz
                return RedirectToAction("Index", "Kullanici");//indexe git diyoruz olmaza kullanıcıdaki index git diyoruz
            }
            catch
            {
                return View();
            }
        }

        // GET: Makale/Edit/5
        public ActionResult Edit(int id)
        {
            string kullaniciadi = Session["username"].ToString();
            var makale = db.Makales.Where(x => x.ID == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            if (makale.Kullanici.KullaniciAdi == kullaniciadi)
            {
                return View(makale);
            }

            return HttpNotFound();
        }

        // POST: Makale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Makale model)
        {
            try
            {
                var makale = db.Makales.Where(i => i.ID == id).SingleOrDefault();//BURADA MAKALEYİ ALADIK
                makale.Baslik = model.Baslik;
                makale.icerik = model.icerik;
                makale.KategoriID = model.KategoriID;//buradaki değerleri eskisiyle değiştiriyoruz.
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                var makale = db.Makales.Where(i => i.ID == id).SingleOrDefault();
                if (kullanici.ID==makale.KullaniciID)
                {
                    foreach(var i in makale.Yorums.ToList())
                    {//yorum silinecek
                        db.Yorums.Remove(i);
                       // db.SaveChanges();
                    }

                    foreach(var i in makale.Etikets.ToList())
                    {
                        db.Etikets.Remove(i);//etiketler silindi
                      //  db.SaveChanges();

                    }
                    db.Makales.Remove(makale);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
              
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi." });
            }
            catch
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi." });
            }
        }

        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
            if (yorum == "")
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorums.Add(new Yorum { KullaniciID = kullanici.ID, MakaleID = Makaleid, Tarih = DateTime.Now, YorumIcerik = yorum });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult YorumDelete(int id)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                var yorum = db.Yorums.Where(i => i.ID == id).SingleOrDefault();
                var makale = db.Makales.Where(i => i.ID == yorum.MakaleID).SingleOrDefault();
                if (yorum == null)
                {
                    return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum bulunamadı" });
                }
                if (OrtakSinif.DeleteYetki(id, kullanici) || makale.KullaniciID == kullanici.ID)
                {
                    db.Yorums.Remove(yorum);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Makale", new { id = yorum.MakaleID });

                }
                return RedirectToAction("Hata", "Yetkili", new { yazilaccak = "yorum silinemedi" });

            }

            catch
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilaccak = "yorum silinemedi" });

            }
            
        }
    }
}
