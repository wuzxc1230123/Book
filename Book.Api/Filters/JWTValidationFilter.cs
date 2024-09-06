using Book.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;

namespace Book.Api.Filters;

public class JWTValidationFilter(IMemoryCache cache, UserManager<User> userManager) : IAsyncActionFilter
{
    private readonly IMemoryCache _cache = cache;
    private readonly UserManager<User> _userManager = userManager;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claimUserId == null)
        {
            await next();
            return;
        }
        var userId = claimUserId!.Value;
        string cacheKey = $"JWTValidationFilter.UserInfo.{userId}";

        var user = await _cache.GetOrCreateAsync(cacheKey, async e => {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return await _userManager.FindByIdAsync(userId.ToString());
        });

        if (user == null)
        {
            var result = new ObjectResult($"UserId({userId}) not found")
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            context.Result = result;
            return;
        }
        var claimVersion = context.HttpContext.User.FindFirst(ClaimTypes.Version);
        long jwtVerOfReq = long.Parse(claimVersion!.Value);
        if (jwtVerOfReq >= user.JWTVersion)
        {
            await next();
        }
        else
        {
            var result = new ObjectResult($"JWTVersion mismatch")
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            context.Result = result;
            return;
        }
    }
}