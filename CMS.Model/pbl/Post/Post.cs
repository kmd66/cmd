using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class Post: BaseModel<Post>
    {
        public Post() { }

        public string Title { get; set; }

        public string Alias { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

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
        public bool Published { get; set; }

        public string PublishedString
        {
            get => Published.ToString();
            set
            {
                if (value == "True")
                    Published = true;
                else
                    Published = false;
            }
        }

        public DateTime? PublishDown { get; set; }

        public string PublishDownString
        {
            get => PublishDown != null ? ((DateTime)PublishDown).ToString("yyyy-MM-dd") : "";
            set
            {
                if (string.IsNullOrEmpty(value))
                    PublishDown = null;
                else
                {
                    try
                    {
                        PublishDown = DateTime.Parse(value);
                    }
                    catch
                    {
                        PublishDown = null;
                    }
                }
            }
        }
        #endregion

        public DateTime Date { get; set; }

        public PostAccessType Access { get; set; }

        public int Hit { get; set; }
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }
    }
}

