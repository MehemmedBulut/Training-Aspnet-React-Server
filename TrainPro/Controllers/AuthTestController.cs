using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainPro.Utility;

namespace TrainPro.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {
            return "You are authenticated";
        }
        [HttpPost("{id:int}")]
        [Authorize(Roles =SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int someIntValue)
        {
            return "You are autheorized with role of admin";
        }
    }
}
