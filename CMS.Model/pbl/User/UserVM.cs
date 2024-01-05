namespace CMS.Model
{
    public class UserVM : ListVM
    {
        public UserVM() { }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
    }
}
