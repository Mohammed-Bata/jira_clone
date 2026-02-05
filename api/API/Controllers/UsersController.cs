using Application.DTOs;
using Application.Users.Commands.ExternalLogin;
using Application.Users.Commands.Login;
using Application.Users.Commands.Logout;
using Application.Users.Commands.Refresh;
using Application.Users.Commands.Register;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //[HttpGet("me")]
        //[Authorize]
        //public IActionResult Me()
        //{
        //    var user = HttpContext.User;

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var cookieValue = Request.Cookies["accessToken"];
        //    Console.WriteLine(cookieValue);

        //    Console.WriteLine($"IsAuthenticated: {User.Identity?.IsAuthenticated}");
        //    Console.WriteLine($"User claims count: {User.Claims.Count()}");

        //    var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        //    var userName = user.FindFirst(ClaimTypes.Name)?.Value;
        //    var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;

        //    Console.WriteLine("User ID: " + userId);

        //    return Ok(new
        //    {
        //        Id = userId,
        //        Name = userName,
        //        Email = userEmail
        //    });

        //}

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequestDto model)
        {
            var command = new LoginCommand(model.Email, model.Password);

            var tokens = await _mediator.Send(command);


            SetRefreshTokenInCookie(tokens.RefreshToken);

            return Ok(new
            {
                accessToken = tokens.AccessToken
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequestDto model)
        {
            var command = new RegisterCommand(model.Name, model.Email, model.Password, model.ConfirmPassword);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<string>> GetNewTokenFromRefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(refreshToken == "" || refreshToken == null)
            {
                return BadRequest("No refresh token provided");
            }

            var command = new RefreshTokensCommand(refreshToken);
            var tokens = await _mediator.Send(command);

            if(tokens == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            SetRefreshTokenInCookie(tokens.RefreshToken);

            return Ok(new
            {
                accessToken = tokens.AccessToken
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var command = new LogoutCommand(refreshToken);

            var result = await _mediator.Send(command);

            DeleteRefreshTokenCookie();

            return result;
        }

        [HttpGet("microsoft/login")]
        public async Task<IActionResult> MicrosoftLogin ()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/Users/microsoft-callback"
            };
            return Challenge(props, "Microsoft");
        }

        [HttpGet("microsoft-callback")]
        public async Task<IActionResult> MicrosoftCallback()
        {
            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
                return Unauthorized();
            var command = new ExternalLoginCommand(
                Provider: "Microsoft",
                ProviderId: result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Email: result.Principal.FindFirstValue(ClaimTypes.Email)!,
                Name: result.Principal.FindFirstValue(ClaimTypes.Name),
                Picture: null
            );
            var Tokens = await _mediator.Send(command);

            SetRefreshTokenInCookie(Tokens.RefreshToken);

            return Redirect($"http://localhost:4200/oauth-callback#accesstoken={Tokens.AccessToken}");
        }

        [HttpGet("github/login")]
        public async Task<IActionResult> GitHubLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/Users/github-callback"
            };
            return Challenge(props, "GitHub");
        }

        [HttpGet("github-callback")]
        public async Task<IActionResult> GitHubCallback()
        {
            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
                return Unauthorized();
            var command = new ExternalLoginCommand(
                Provider: "GitHub",
                ProviderId: result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Email: result.Principal.FindFirstValue(ClaimTypes.Email)!,
                Name: result.Principal.FindFirstValue(ClaimTypes.Name),
                Picture: result.Principal.FindFirstValue("picture")
            );
            var Tokens = await _mediator.Send(command);

          
            SetRefreshTokenInCookie(Tokens.RefreshToken);

            return Redirect($"http://localhost:4200/oauth-callback#accesstoken={Tokens.AccessToken}");
        }


        [HttpGet("google/login")]
        public async Task<IActionResult> GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/Users/google-callback"
            };

            return Challenge(props, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("External");

            if (!result.Succeeded)
                return Unauthorized();

            var command = new ExternalLoginCommand(
                Provider: "Google",
                ProviderId: result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Email: result.Principal.FindFirstValue(ClaimTypes.Email)!,
                Name: result.Principal.FindFirstValue(ClaimTypes.Name),
                Picture: result.Principal.FindFirstValue("picture")
            );

            var Tokens = await _mediator.Send(command);

            
            SetRefreshTokenInCookie(Tokens.RefreshToken);

            return Redirect($"http://localhost:4200/oauth-callback#accesstoken={Tokens.AccessToken}");

        }

        //=============================================================================
       
        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        private void DeleteRefreshTokenCookie()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1) // Set expiration to past date
            };
            Response.Cookies.Append("refreshToken", "", cookieOptions);
        }

    }
}
