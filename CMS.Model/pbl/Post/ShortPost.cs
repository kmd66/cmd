using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class ShortPost
    {
        public Guid UnicId { get; set; }
        public ShortPost() { }
        public static List<ShortPost> Instance(List<Post> items)
        {
            string jsonString = JsonSerializer.Serialize(items);
            var model = JsonSerializer.Deserialize<List<ShortPost>>(jsonString);
            return model;
        }

        public string Title { get; set; }

        public string Alias { get; set; }

        public string Img { get; set; }

    }
}

