namespace CMS.Model
{
    public class User : BaseModel
    {
        public User() { }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public UserType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
