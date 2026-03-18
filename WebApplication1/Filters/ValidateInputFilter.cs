using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace server.Filters;

public class ValidateInputFilter : IPageFilter
{
    public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        if (context.HttpContext.Request.Method == HttpMethods.Get)
            return;

        if (!context.ModelState.IsValid)
        {
            context.Result = new PageResult();
        }
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
}