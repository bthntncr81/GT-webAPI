using System.Security.Claims;
using AutoMapper;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Http;

namespace GTBack.Service.Services;

public class SharedFunctions
{
    private readonly ClaimsPrincipal? _loggedUser;

    public SharedFunctions(
        IHttpContextAccessor httpContextAccessor
     )
    {
        _loggedUser = httpContextAccessor.HttpContext?.User;
    }
    
    public int? GetLoggedCompanyId()
    {
        var userRoleString = _loggedUser.FindFirstValue("companyId");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }
    
    public int? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }
}