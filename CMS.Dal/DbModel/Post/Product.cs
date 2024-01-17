using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace CMS.Dal.DbModel
{
    public class Product : BaseModel
    {
        public long Price { get; set; }

        public long SpecialPrice { get; set; }

        public string ProductID { get; set; }

        public byte Type { get; set; }

        public int Number { get; set; }

        public string Property { get; set; }

    }
}

