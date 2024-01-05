using CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class Property
    {
        #region
        public static string MsgUnUnauthorized = "Un Unauthorized";

        #endregion
        public static string ConnectionString { get; set; }
        public static int AccessTokenExpireTimeSpan { get; set; }
        public static int AttachmentSize { get; set; }
        public static string WebRootPath { get; set; }
        public static string Upload { get; set; }
        public static string UploadPath => @$"{WebRootPath}\{Upload}\";


        public static List<FileExtensions> ImgExtensions =new List<FileExtensions> {  FileExtensions.jpeg
            , FileExtensions.jpg
            , FileExtensions.png
            , FileExtensions.bmp
        };
    }
}

