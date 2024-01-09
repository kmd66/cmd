using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model.Files
{
    public class Item
    {
        public static string GetThumbnail(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";
            var t = url.Split('.');
            string ext = System.IO.Path.GetExtension(url);
            return $"{t[0]}-thumbnail{ext}";
        }
        public static string GetUrlFromThumbnail(string thumbnail)
        {
            if (string.IsNullOrEmpty(thumbnail))
                return "";
            return thumbnail.Replace("-thumbnail.",".");
        }

        public PathType Type { get; set; }
        public string Name { get; set; }
        public string? Path { get; set; }
        public string Url
        {
            get
            {
                string url = $"/{Property.Upload}/";
                if (!string.IsNullOrEmpty(Path))
                    url += $"{Path}";

                if (Type == PathType.File)
                    url += $"{Name}";
                return url;
            }
        }
        public string Thumbnail
        {
            get
            {
                if (!IsImg)
                    return "./img/flm-img.png";
                return GetThumbnail(Url);
            }
        }
        public bool IsImg
        {
            get
            {
                if (Type == PathType.File && Property.ImgExtensions.Any(x=>x== Extension))
                    return true;
                return false;
            }
        }
        public FileExtensions Extension { get; set; }
        //{
        //    get
        //    {
        //        if (Type == PathType.File)
        //            return Path.GetExtension(Name).Replace(".", "").ToUpper();
        //        else
        //            return "DIR";
        //    }
        //}
    }
}
