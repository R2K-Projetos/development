using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Ghb.Psicossoma.Api.Controllers.Base
{
    public abstract class BaseApiController : ControllerBase
    {
        #region " Autenticação "

        protected string GetUserId()
        {
            return User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Sid)?.Value!;
        }

        protected string GetUserMail()
        {
            return User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value!;
        }

        protected string GetUserName()
        {
            return User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value!;
        }

        protected string GetUserRole()
        {
            return User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Role)?.Value!;
        }

        #endregion
    }
}
