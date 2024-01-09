using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model.Files
{
    public class Item
    {
        public PathType Type { get; set; }
        public string Name { get; set; }
        public string? Path { get; set; }
        public string Url
        {
            get
            {
                string url = $"{Property.Upload}/";
                if (!string.IsNullOrEmpty(Path))
                    url += $"{Path}";

                if (Type == PathType.File)
                    url += $"{Name}";
                return url ;
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
