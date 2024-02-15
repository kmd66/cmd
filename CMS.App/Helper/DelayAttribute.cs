using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net;

namespace CMS.App.Helper
{

    public class DelayAttribute : ActionFilterAttribute

    {
        static bool isMulti = false;
        private static int isExecuting = 0;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (isExecuting < 10)
            {
                isExecuting++;
                if (isExecuting > 1)
                    await Task.Delay(isExecuting * 1500);
                await next();
                await Task.Delay(200);
                isExecuting--;
                if (isExecuting < 0)
                    isExecuting = 0;
            }
            else
            {
                context.Result = new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><head><meta charset=\"utf-8\" /></head><body>محدودیت درخواست. کمی صبر کنید و دوباره امتحان کنید</body></html>"
                };
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
    public class DelayApiAttribute : ActionFilterAttribute

    {
        private static int isExecuting = 0;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (isExecuting < 5)
            {
                isExecuting++;
                if (isExecuting > 1)
                    await Task.Delay(isExecuting * 2500);
                await next();
                await Task.Delay(1000);
                isExecuting--;
                if (isExecuting < 0)
                    isExecuting = 0;
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                };
            }
        }
    }

    public class InsideDelayAttribute : ActionFilterAttribute

    {
        static bool isMulti = false;
        private static int isExecuting = 0;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (isExecuting < 10)
            {
                isExecuting++;
                if (isExecuting > 1)
                    await Task.Delay(isExecuting * 2500);
                await next();
                await Task.Delay(1000);
                isExecuting--;
                if (isExecuting < 0)
                    isExecuting = 0;
            }
            else
            {
                context.Result = new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html><head><meta charset=\"utf-8\" /></head><body>محدودیت درخواست. کمی صبر کنید و دوباره امتحان کنید</body></html>"
                };
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
    public class InsideDelayApiAttribute : ActionFilterAttribute

    {
        private static int isExecuting = 0;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (isExecuting < 5)
            {
                isExecuting++;
                if (isExecuting > 1)
                    await Task.Delay(isExecuting * 3000);
                await next();
                await Task.Delay(1500);
                isExecuting--;
                if (isExecuting < 0)
                    isExecuting = 0;
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                };
            }
        }
    }
}
