using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    /// <summary>
    /// This is validating all of our images.
    /// </summary>
    public class ImageUploadValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;
            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                        ImageFormat.Png.Equals(img.RawFormat) ||
                        ImageFormat.Gif.Equals(img.RawFormat);

                }
            }
            catch 
            {
                return false;
            }

        }
    }
    /// <summary>
    /// this is validating all of our files.
    /// we had to Int32.Parse so we could convert a string into and int
    /// WebConfigurationManager is pointing to Web.config Appsetting
    /// </summary>
    public class FileUploadValidator 
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;
            var maxSize = WebConfigurationManager.AppSettings["MaxFileSize"];
            var minSize = WebConfigurationManager.AppSettings["MinFileSize"];
            if (file.ContentLength > Int32.Parse(maxSize) || file.ContentLength < Int32.Parse(minSize))
                return false;
            try
            {
                var validExtentions = WebConfigurationManager.AppSettings["validExtentions"];
                var fileExt = Path.GetExtension(file.FileName);
                return validExtentions.Contains(fileExt);
               
              
            }
            catch
            {
                return false;
            }

        }



    }
}