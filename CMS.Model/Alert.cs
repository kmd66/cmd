namespace CMS.Model
{
    public class Alert
    {
        public string msg { get; set; }

        public AlertType alertType { get; set; }

        public bool expires = true;

        public bool withProgress = true;

        public string container = ".anotherElement";
    }
}