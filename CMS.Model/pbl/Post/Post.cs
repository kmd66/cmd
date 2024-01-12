using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model.pbl.Post
{
    public class Post: BaseModel<Post>
    {
        public Post() { }

        public string Title { get; set; }

        public string Alias { get; set; }

        public string Summary { get; set; }

        public bool Content { get; set; }

        public string Img { get; set; }

        #region Special
        public bool Special { get; set; }

        public string SpecialString
        {
            get => Special.ToString();
            set
            {
                if (value == "True")
                    Special = true;
                else
                    Special = false;
            }
        }
        #endregion

        #region publish
        public bool published { get; set; }

        public string publishedString
        {
            get => published.ToString();
            set
            {
                if (value == "True")
                    published = true;
                else
                    published = false;
            }
        }

        public DateTime? publishDown { get; set; }
        #endregion

        public DateTime Date { get; set; }

        public PostAccessType Access { get; set; }

        public int Hit { get; set; }
    }
}

