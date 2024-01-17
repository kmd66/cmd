using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace CMS.Dal.DbModel
{
    public class Option 
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        
    }
}
