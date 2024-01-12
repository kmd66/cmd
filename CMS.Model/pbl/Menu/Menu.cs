using System.Text.Json;

namespace CMS.Model
{
    public class Menu : BaseModel<Menu>
    {
        public Menu() { }

        public int Order { get; set; }
        
        public MenuType Type { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Link { get; set; }

        public Guid PostId { get; set; }

        public Guid Parent { get; set; }

        public string Img { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }

        public string PublishedString { get=> Published.ToString();
            set {
                if (value == "True")
                    Published = true;
                else
                    Published = false;
            }
        }

        public List<Menu>? Child { get; set; }


    }
}
