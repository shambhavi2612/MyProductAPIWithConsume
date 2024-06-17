//using Microsoft.AspNetCore.Mvc;
//using ProductsApp.Models;
//using System.Linq;
//using Microsoft.Extensions.Logging;

//namespace ProductsApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<UsersController> _logger;

//        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        [HttpPost("register")]
//        public IActionResult Register(User user)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            _context.Users.Add(user);
//            _context.SaveChanges();

//            return Ok();
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] User user)
//        {
//            _logger.LogInformation("Login attempt for user: {Username}", user.Username);

//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("Invalid login request. ModelState: {@ModelState}", ModelState);
//                return BadRequest(ModelState);
//            }

//            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
//            if (existingUser == null)
//            {
//                _logger.LogWarning("Login failed for user: {Username}", user.Username);
//                return Unauthorized();
//            }

//            _logger.LogInformation("Login successful for user: {Username}", user.Username);
//            return Ok();
//        }
//    }
//}

// myyyyyy user controller 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsApp.Helpers;
using ProductsApp.Models;
using System.Linq;

namespace ProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger, JwtTokenHelper jwtTokenHelper)
        {
            _context = context;
            _logger = logger;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            _logger.LogInformation("Login attempt for user: {Username}", user.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request. ModelState: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                _logger.LogWarning("Login failed for user: {Username}", user.Username);
                return Unauthorized();
            }

            var token = _jwtTokenHelper.GenerateToken(user.Username);
            _logger.LogInformation("Login successful for user: {Username}", user.Username);
            return Ok(new { token });
        }

        [HttpGet]
        [Route("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("This is a secured endpoint.");
        }
    }
}
