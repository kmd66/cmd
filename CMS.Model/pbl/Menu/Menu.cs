namespace CMS.Model
{
    public class Menu : BaseModel
    {
        public Menu() { }

        public int Order { get; set; }
        
        public MenuType Type { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public Guid Parent { get; set; }

        public string Img { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }


    }
}
