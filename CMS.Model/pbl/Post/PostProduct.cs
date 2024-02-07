using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class PostProduct : BaseModel<PostProduct>
    {
        public PostProduct() { }

        public string Title { get; set; }

        public long Price { get; set; }

        public long SpecialPrice { get; set; }

        public string Summary { get; set; }

        public string Alias { get; set; }

        public string Img { get; set; }

        public DateTime Date { get; set; }
        
        public Guid MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuAlias { get; set; }

        public int Total { get; set; }

        public bool IsProduct { get; set; }
        public bool Special { get; set; }
    }
}

