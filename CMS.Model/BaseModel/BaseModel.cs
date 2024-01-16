using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class BaseModel<T>: BaseModel
    {
        public int Total { get; set; }

        public static T Instance(T item)
        {
            string jsonString = JsonSerializer.Serialize(item);

            var menu = JsonSerializer.Deserialize<T>(jsonString);
            return menu;
        }
    }
    public class BaseModel
    {
        public long Id { get; set; }
        public Guid UnicId { get; set; }
    }
}
