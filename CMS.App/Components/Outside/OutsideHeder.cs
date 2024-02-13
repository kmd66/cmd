using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideHeder : ViewComponent
    {
        //The Invoke method for the View component
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new OutsideHederModel();
            model.logo = CMS.Model.Option.GetItem(Model.OptionType.Icon).Text;
            model.title = CMS.Model.Option.GetItem(Model.OptionType.Title).Text;
            model.comment = CMS.Model.Option.GetItem(Model.OptionType.Comment).Text;
            model.menus = CMS.Model.Menus.GetList(new Model.MenuVM { Published = true });

            model.MenuItem = "";
            foreach (var item in model.menus)
                model.MenuItem += getMenuItem(item, false);

            model.MenuItemOffcanvas = "";
            foreach (var item in model.menus)
                model.MenuItemOffcanvas += getMenuItemOffcanvas(item, false);

            return View("~/Views/Shared/Outside/Heder.cshtml", model);
        }
        private string getItemClass(bool Child)
        {
            var c = Child ? "dropdown-item" : "navbar-brand";
            return c + " px-2";
        }

        private string getNavbarItemClass(bool Child)
        {
            return Child ? "nav-Item" : "nav-Item navbarItem";
        }

        private string getMenuItem(CMS.Model.Menu Item, bool Child)
        {
            string s = "";
            if (Item.Type == Model.MenuType.Link)
            {
                s = $"<li class=\"{getNavbarItemClass(Child)}\"><a href=\"{Item.Link}\" class=\"{getItemClass(Child)}\">{Item.Name}</a></li>";
            }
            else if (Item.Type == Model.MenuType.Content)
            {
                s = $"<li class=\"{getNavbarItemClass(Child)}\"><a href=\"{Item.Alias}\" class=\"{getItemClass(Child)}\">{Item.Name}</a></li>";
            }
            else if (Item.Type == Model.MenuType.Categorie)
            {
                s = $"<li class=\"{getNavbarItemClass(Child)}\"><a href=\"category/{Item.Alias}\" class=\"{getItemClass(Child)}\">{Item.Name}</a></li>";
            }
            else if (Item.Type == Model.MenuType.ProductCategorie)
            {
                s = $"<li class=\"{getNavbarItemClass(Child)}\"><a href=\"product-category/{Item.Alias}\" class=\"{getItemClass(Child)}\">{Item.Name}</a></li>";
            }
            else if (Item.Type == Model.MenuType.Title)
            {
                s = $"<div class=\"dropdown navbarItem\"><span class=\"dropdown-toggle px-2\" type=\"button\" id=\"dropdownMenuButton{Item.Alias}\" " +
                    $"data-bs-toggle=\"dropdown\" aria-expanded=\"false\" style=\" color: var(--bs-nav-link-color);\">{Item.Name}</span>";
                if (Item.Child?.Count > 0)
                {
                    s += $"<ul class=\"dropdown-menu\" aria-labelledby=\"dropdownMenuButton{Item.Alias}\">";
                    foreach (var child in Item.Child) s += $"<li>{getMenuItem(child, true)}</li>";
                s += "</ul>";
                }
                s += "</div>";
            }
            return s;
        }

        private string getMenuItemOffcanvas(CMS.Model.Menu Item, bool Child)
        {
            string s = "";
            if (Item.Type == Model.MenuType.Link)
            {
                s = $"<a href=\"{Item.Link}\" class=\"list-group-item list-group-item-action py-2 ripple\" onclick=\"hideOffcanvas('offcanvasMenu')\"><i class=\"fas fa-money-bill fa-fw me-3\"></i><span>{Item.Name}</span></a>";
            }
            else if (Item.Type == Model.MenuType.Content)
            {
                s = $"<a href=\"{Item.Alias}\" class=\"list-group-item list-group-item-action py-2 ripple\" Match=\"NavLinkMatch.All\" onclick=\"hideOffcanvas('offcanvasMenu')\"><i class=\"fas fa-money-bill fa-fw me-3\"></i><span>{Item.Name}</span></a>";
            }
            else if (Item.Type == Model.MenuType.Categorie)
            {
                s = $"<NavLink href=\"category/{Item.Alias}\" class=\"list-group-item list-group-item-action py-2 ripple\" Match=\"NavLinkMatch.All\" onclick=\"hideOffcanvas('offcanvasMenu')\"><i class=\"fas fa-money-bill fa-fw me-3\"></i><span>{Item.Name}</span></NavLink>";
            }
            else if (Item.Type == Model.MenuType.ProductCategorie)
            {
                s = $"<NavLink href=\"product-category/{Item.Alias}\" class=\"list-group-item list-group-item-action py-2 ripple\" Match=\"NavLinkMatch.All\" onclick=\"hideOffcanvas('offcanvasMenu')\"><i class=\"fas fa-money-bill fa-fw me-3\"></i><span>{Item.Name}</span></NavLink>";
            }
            else if (Item.Type == Model.MenuType.Title)
            {
                s = $"<span data-bs-toggle=\"collapse\" class=\"list-group-item list-group-item-action py-2 ripple\" role=\"button\" aria-expanded=\"false\" href=\"#collapseMenuItem{Item.Alias}\" aria-controls=\"collapseMenuItem{Item.Alias}\"><i class=\"fas fa-money-bill fa-fw me-3\"></i><span>{Item.Name}</span></span>"; 
                if (Item.Child?.Count > 0)
                {
                    s += $"<div class=\"collapse\" id=\"collapseMenuItem{Item.Alias}\"><div class=\"card card-body\">";
                    foreach (var child in Item.Child) s += getMenuItemOffcanvas(child, true);
                    s += $"</div></div>";
                }
            }
            return s;
        }

    }
    public class OutsideHederModel
    {
        public string logo { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
        public List<CMS.Model.Menu> menus { get; set; }
        public int count { get; set; }
        public string searchText { get; set; }
        public string MenuItem { get; set; }
        public string MenuItemOffcanvas { get; set; }
    }
}