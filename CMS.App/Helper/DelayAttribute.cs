using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net;

namespace CMS.App.Helper
{

    public class DelayAttribute : ActionFilterAttribute

    {
        public static int isDelayExecuting = 0;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (DelayAttribute.isDelayExecuting < 10)
            {
                DelayAttribute.isDelayExecuting++;
                if (DelayAttribute.isDelayExecuting > 1)
                    await Task.Delay(DelayAttribute.isDelayExecuting * 1500);
                await next();
                await Task.Delay(200);
                DelayAttribute.isDelayExecuting--;
                if (DelayAttribute.isDelayExecuting < 0)
                    DelayAttribute.isDelayExecuting = 0;
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
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (DelayAttribute.isDelayExecuting < 5)
            {
                DelayAttribute.isDelayExecuting++;
                if (DelayAttribute.isDelayExecuting > 1)
                    await Task.Delay(DelayAttribute.isDelayExecuting * 2500);
                await next();
                await Task.Delay(1500);
                DelayAttribute.isDelayExecuting--;
                if (DelayAttribute.isDelayExecuting < 0)
                    DelayAttribute.isDelayExecuting = 0;
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
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (DelayAttribute.isDelayExecuting < 5)
            {
                DelayAttribute.isDelayExecuting++;
                if (DelayAttribute.isDelayExecuting > 1)
                    await Task.Delay(DelayAttribute.isDelayExecuting * 3500);
                await next();
                await Task.Delay(2000);
                DelayAttribute.isDelayExecuting--;
                if (DelayAttribute.isDelayExecuting < 0)
                    DelayAttribute.isDelayExecuting = 0;
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
    }
    public class InsideDelayApiAttribute : ActionFilterAttribute

    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (DelayAttribute.isDelayExecuting < 5)
            {
                DelayAttribute.isDelayExecuting++;
                if (DelayAttribute.isDelayExecuting > 1)
                    await Task.Delay(DelayAttribute.isDelayExecuting * 3500);
                await next();
                await Task.Delay(2000);
                DelayAttribute.isDelayExecuting--;
                if (DelayAttribute.isDelayExecuting < 0)
                    DelayAttribute.isDelayExecuting = 0;
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
