using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    public class FileUploadHelper
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {

            if (file == null)
                return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;


            try 
            {
                foreach(var ext in WebConfigurationManager.AppSettings["validExtensions"].Split(',')) 
                {
                    if (VirtualPathUtility.GetExtension(file.FileName).Contains(ext))
                        return true;
                }
            }
            catch 
            {
                return false;
            }
            return false;
        }



    }
}