using Giftify.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Giftify.Filters;


public class CartCountFilter : IAsyncResultFilter
{
    private readonly ICartService _cartService;

    public CartCountFilter(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ViewResult viewResult)
        {
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is not null)
                {
                    var count = await _cartService.GetCartItemCountAsync(userId);
                    viewResult.ViewData["CartCount"] = count;  
                }
            }
            else
            {
                viewResult.ViewData["CartCount"] = 0;
            }
        }

        await next();
    }
}
