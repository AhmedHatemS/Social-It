using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialIt.Models
{
    public class DbConnection : Controller
    {
        protected SocialIt_DBEntities db = new SocialIt_DBEntities();
    }
}