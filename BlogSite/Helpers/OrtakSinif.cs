using BlogSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSite.Helpers
{
    public  class OrtakSinif
    {
        //base class diyebiliriz 

        BlogDB db = new BlogDB();
        public static bool EdityYetki(int id,Kullanici user)
        {
          
            //var kullaniciadi =Session["username"].ToString();
            //var user = db.Kullanicis.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();

            if (user.ID == id)
            {
                return true;
            }
            else if (user.YetkiId > 2)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool DeleteYetki(int id,Kullanici user)
        {
           
            if(user.ID==id)
            {
                return true;
            }
            else if(user.YetkiId>3)
            {
                return true;
            }
           bool result = false;
            return result;
        }


    }
}