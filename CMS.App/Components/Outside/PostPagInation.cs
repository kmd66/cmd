using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class PostPagInationView
    {
        string s = "";
        double count = 0;
        int? pageIndex;
        List<int> item = new List<int>();
        int start;
        int end;
        int min;
        int max;
        bool endMore;
        bool startMore; 
        PostPagInationModel Model { get; set; }

        public async Task<string> InvokeAsync(PostPagInationModel model)
        {
            Model = model;
            if (model.ItemModel == null || model.ItemModel.ItemsCount == 0)
                return "";
            pageIndex = model.ItemModel.PageIndex;
            setItem();
            if (count > 1)
            {
                s = $"<nav aria-label=\"Page navigation example\" style=\"{model.Style}\"><ul class=\"pagination justify-content-center\">";
                
                if (getPreviousCalss().Contains(" disabled"))
                    s += $"<li class=\"{getPreviousCalss()}\"><span class=\"page-link\" aria-label=\"Next\"><span aria-hidden=\"true\">&laquo;</span></span></li>";
                else
                    s += $"<li class=\"{getPreviousCalss()}\"><a href=\"{Model.Link}/{pageIndex.Default() -1}\" class=\"page-link\"><span aria-hidden=\"true\">&laquo;</span></a></li>";

                foreach (var i in item)
                {
                    if (getNextCalss(i).Contains(" disabled"))
                        s += $"<li class=\"{getNextCalss(i)}\"><span class=\"page-link\">{getText(i)}</span></li>";
                    else
                        s += $"<li class=\"{getNextCalss(i)}\"><a href=\"{Model.Link}/{@getText(i)}\" class=\"page-link\">{getText(i)}</a></li>";
                }
                    

                if (getNextCalss().Contains("disabled"))
                    s += $"<li class=\"{getNextCalss()}\"><span class=\"page-link\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></span></li>";
                else
                    s += $"<li class=\"{getNextCalss()}\"><a href=\"{Model.Link}/{pageIndex.Default() +1}\" class=\"page-link\"><span aria-hidden=\"true\">&raquo;</span></a></li>";
                s += "</ul></nav>";
            }   
            return s;
        }

        private string getPreviousCalss() =>
            pageIndex != 1 ? "page-item" : "page-item disabled";
        private string getNextCalss() =>
            pageIndex != count ? "page-item" : "page-item disabled";
        private string getNextCalss(int i) =>
            i != 0 && pageIndex != i ? "page-item" : "page-item disabled";
        private string getText(int i) =>
            i != 0 ? i.ToString() : "...";


        private void setItem()
        {
            item = new List<int>();
            count = 0;
            if (Model.ItemModel.PageIndex.Default() == 0
                || Model.ItemModel.PageSize.Default() == 0
                || Model.ItemModel.ItemsCount.Default() <= 0
            )
                return;

            count = Math.Ceiling(Model.ItemModel.Total.ToDouble() / Model.ItemModel.PageSize.ToDouble());

            startMore = false;
            endMore = false;

            min = 1;
            max = count.ToInt();
            start = pageIndex.Default() - 2;
            end = pageIndex.Default() + 2;
            if (start <= 1)
            {
                start = 1;

                min = 0;
            }

            if (end >= count)
            {
                end = count.ToInt();
                max = 0;
            }

            if (min > 0 && min < start - 1)
                startMore = true;
            if (max > 0 && max > end + 1)
                endMore = true;


            if (min > 0)
            {
                item.Add(min);
            }
            if (startMore)
            {
                item.Add(0);
            }
            for (int x = start; x <= end; x++)
            {
                item.Add(x);
            }
            if (endMore)
            {
                item.Add(0);
            }
            if (max > 0)
            {
                item.Add(max);
            }
        }
    }
    public class PostPagInationModel
    {
        public PageInation ItemModel { get; set; }
        public string Style { get; set; }
        public string Link { get; set; }
    }
}