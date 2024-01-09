using System;
using System.Reflection;

namespace CMS.Model
{
    public static class Menus{

        public static List<Menu> List = new List<Menu>();

        public static List<Menu> GetList(MenuVM modelVM)
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;

                if (item.Parent == Guid.Empty)
                {
                    var menu = Menu.Instance(item);
                    menu.Child = addChild(modelVM, menu);
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<Menu>? addChild(MenuVM modelVM, Menu model)
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;

                if (item.Parent != Guid.Empty && item.Parent == model.UnicId)
                {
                    var menu = Menu.Instance(item);
                    menu.Child = addChild(modelVM, menu);
                    list.Add(menu);
                }
            }
            return list.Count > 0 ? list:null;
        }

        public static List<Menu> GetList2(MenuVM modelVM, string extraName, Guid? ignore = null)
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;
                if (ignore!= null && item.UnicId == ignore)
                    continue;

                if (item.Parent == Guid.Empty)
                {
                    var menu = Menu.Instance(item);
                    list.Add(menu);
                    var child = GetChildList2(modelVM, menu, extraName, ignore);
                    list.AddRange(child);
                }
            }
            return list;
        }

        private static List<Menu> GetChildList2(MenuVM modelVM, Menu model, string extraName, Guid? ignore)
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;
                if (ignore != null && item.UnicId == ignore)
                    continue;

                if (item.Parent != Guid.Empty && item.Parent == model.UnicId)
                {
                    var menu = Menu.Instance(item);
                    menu.Name = extraName + menu.Name;
                    list.Add(menu);
                    var child = GetChildList2(modelVM, menu, extraName + extraName, ignore);
                    list.AddRange(child);
                }
            }
            return list;
        }

        public static List<Menu> GetChild(MenuVM modelVM)
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;
                if (item.Parent == modelVM.ParentId)
                {
                    var menu = Menu.Instance(item);
                    list.Add(menu);
                }
            }
            return list;
        }

    }
}
