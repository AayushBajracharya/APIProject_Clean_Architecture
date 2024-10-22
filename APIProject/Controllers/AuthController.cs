using APIProject.Repo;
using APIProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using APIProject.Models;
using APIProject.DTO;

namespace APIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(IUserRepository userRepository, JwtTokenHelper jwtTokenHelper)
        {
            _userRepository = userRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] loginDto loginDto)
        {
            var user = _userRepository.Authenticate(loginDto.Username, loginDto.Password);
            if (user == null) return Unauthorized();

            var token = _jwtTokenHelper.GenerateToken(user);
            return Ok(new { token });
        }

    }


}
