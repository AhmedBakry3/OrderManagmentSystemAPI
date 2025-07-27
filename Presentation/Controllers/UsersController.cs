using Shared.DataTransferObject.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class UsersController(IServiceManager _serviceManager) : ApiBaseController
    {
        //POST : BaseUrl/api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(User);
        }

        //POST : BaseUrl/api/Users/register 
        [HttpPost("register")] 
        public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(User);
        }
    }
}
