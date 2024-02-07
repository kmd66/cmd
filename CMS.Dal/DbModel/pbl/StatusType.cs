using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace CMS.Dal.DbModel
{
    [Keyless]
    public class StatusType
    {
        public byte Type { get; set; }

        public int Total { get; set; }

    }
}
