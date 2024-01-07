﻿using System.Text.Json;

namespace CMS.Model
{
    public class Menu : BaseModel
    {
        public Menu() { }
        public static Menu Instance(Menu item)
        {
            string jsonString = JsonSerializer.Serialize(item);

            var menu = JsonSerializer.Deserialize<Menu>(jsonString);
            return menu != null ? menu : new Menu();
        }

        public int Order { get; set; }
        
        public MenuType Type { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public Guid Parent { get; set; }

        public string Img { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }

        public List<Menu>? Child { get; set; }


    }
}
