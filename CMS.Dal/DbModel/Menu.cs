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
    public class Menu : BaseModel
    {
        public int Order { get; set; }

        public byte Type { get; set; }
        
        [MaxLength(25)]
        public string Name { get; set; }

        [DefaultValue("")]
        public string Link { get; set; }

        public Guid Parent { get; set; }

        [DefaultValue("")]
        public string Img { get; set; }

        public DateTime Date { get; set; }

        [DefaultValue(true)]
        public bool Published { get; set; }
    }
}
