using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    public abstract class BaseController:ControllerBase
    {
        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected string? Role => User.FindFirst(ClaimTypes.Role)?.Value;

    }
}
