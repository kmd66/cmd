﻿namespace CMS.Model
{
    public class MenuVM : ListVM
    {
        public MenuVM() { }

        public bool? Published { get; set; }
        public Guid? ParentId { get; set; }

    }
}
