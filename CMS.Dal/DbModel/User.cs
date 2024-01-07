using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class User : BaseModel
    {
        //public User() { }

        [MaxLength(25)]
        public string FirstName { get; set; }
        
        [MaxLength(25)]
        public string LastName { get; set; }

        [MaxLength(25)]
        public string UserName { get; set; }

        [MaxLength(25)]
        public string Password { get; set; }

        [DefaultValue(0)]
        public byte Type { get; set; }

        public DateTime Date { get; set; }
    }
}
