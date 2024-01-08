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

        private static List<Menu>? addChild(MenuVM modelVM, Menu model )
        {
            var list = new List<Menu>();

            foreach (var item in List)
            {
                if (modelVM.Published != null && item.Published != modelVM.Published)
                    continue;

                if (item.Parent != Guid.Empty && item.Parent == model.UnicId)
                {
                    item.Child = addChild(modelVM, item);
                    list.Add(item);
                }
            }
            return list.Count > 0 ? list:null;
        }

    }
}
