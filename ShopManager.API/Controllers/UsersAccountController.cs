namespace ShopManager.API.Controllers;

using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShopManager.API.Contracts.Requests;
using ShopManager.API.Contracts.Responses;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;
using ShopManager.Domain.Options;

public class UsersAccountController : BaseController
{
    private readonly ILogger _logger;
    private readonly JWTSecretOptions _options;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ISessionsRepository _sessionsRepository;
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly IUsersService _usersService;
    private readonly IConfiguration _configuration;

    public UsersAccountController(
        ILogger<UsersAccountController> logger,
        IOptions<JWTSecretOptions> options,
        UserManager<UserEntity> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ITransactionsRepository transactionsRepository,
        ISessionsRepository sessionsRepository,
        IUsersService usersService,
        IConfiguration configuration)
    {
        _logger = logger;
        _options = options.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _sessionsRepository = sessionsRepository;
        _transactionsRepository = transactionsRepository;
        _usersService = usersService;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("user-registration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserAndCompanyRegistrationAsync(
        [FromBody] UserRegistration request)
    {
        var transaction = await _transactionsRepository.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var error = "User is existing.";
                _logger.LogError("{error}", error);
                throw new Exception(error);
            }

            var userModel = Domain.Models.User.Create(
                request.Email,
                request.Email,
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.DateOfBirth);

            if (userModel.IsFailure)
            {
                _logger.LogError("{error}", userModel.Error);
                throw new Exception(userModel.Error);
            }

            var newUser = new UserEntity
            {
                UserName = userModel.Value.Email,
                Email = userModel.Value.Email,
                FirstName = userModel.Value.FirstName,
                LastName = userModel.Value.LastName,
                MiddleName = userModel.Value.MiddleName,
                DateOfBirth = userModel.Value.DateOfBirth,
                RegistrationDate = userModel.Value.RegistrationDate,
            };

            var result = await _userManager.CreateAsync(
                newUser,
                request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("{errors}", result.Errors);
                throw new Exception(result.Errors.ToString());
            }

            var roleExists = await _roleManager.RoleExistsAsync(nameof(Roles.User));
            if (!roleExists)
            {
                var role = new IdentityRole<Guid>
                {
                    Name = nameof(Roles.User)
                };

                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(newUser, nameof(Roles.User));
            await _transactionsRepository.CommitTransactionAsync(transaction);
        }
        catch (Exception e)
        {
            await _transactionsRepository.RollbackTransactionAsync(transaction);
            return BadRequest(e.Message);
        }

        return Ok(true);
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
    {
        var userResult =
            await _usersService.GetByEmailAndCompanyIdAsync<UserEntity>(request.Email);

        if (userResult.IsFailure)
        {
            return BadRequest(userResult.Error);
        }

        if (userResult.Value is null)
        {
            return BadRequest("User not found.");
        }

        var isSuccess = await _userManager
            .CheckPasswordAsync(userResult.Value, request.Password);

        if (!isSuccess)
        {
            return BadRequest("Password is incorrect.");
        }

        var roles = await _userManager.GetRolesAsync(userResult.Value);
        var role = roles.FirstOrDefault();
        if (role is null)
        {
            return BadRequest("Role isn't exist.");
        }

        var userInformation =
            new UserInformation(userResult.Value.UserName, userResult.Value.Id, role);
        var accessToken = JwtHelper.CreateAccessToken(userInformation, _options);
        var refreshToken = JwtHelper.CreateRefreshToken(userInformation, _options);

        var session = Session.Create(userResult.Value.Id, accessToken, refreshToken);
        if (session.IsFailure)
        {
            _logger.LogError("{error}", session.Error);
            return BadRequest(session.Error);
        }

        var result = await _sessionsRepository.Create(session.Value);

        if (result.IsFailure)
        {
            _logger.LogError("{error}", result.Error);
            return BadRequest(result.Error);
        }

        Response.Cookies.Append(DefaultAuthenticationTypes.ApplicationCookie, refreshToken, new CookieOptions()
        {
            Secure = false,
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });

        return Ok(new TokenResponse
        {
            Id = userResult.Value.Id,
            Role = role,
            AccessToken = accessToken,
            Nickname = userResult.Value.UserName,
            Email = userResult.Value.Email,
        });
    }

    [AllowAnonymous]
    [HttpPost("refreshaccesstoken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshAccessToken()
    {
        const bool jwtTokenV2 = false;
        Result<UserInformation> userInformation;
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest("Refresh Token can't be null");
        }

        if (jwtTokenV2)
        {
            userInformation = JwtHelper.GetPayloadFromJWTTokenV2(refreshToken, _options);
        }
        else
        {
            var payload = JwtHelper.GetPayloadFromJWTToken(refreshToken, _options);
            userInformation = JwtHelper.ParseUserInformation(payload);
        }

        if (userInformation.IsFailure)
        {
            _logger.LogError("{error}", userInformation.Error);
            return BadRequest(userInformation.Error);
        }

        var resultGet = await _sessionsRepository.GetById(userInformation.Value.UserId);
        if (resultGet.IsFailure)
        {
            _logger.LogError("{error}", resultGet.Error);
            return BadRequest(resultGet.Error);
        }

        if (resultGet.Value.RefreshToken != refreshToken)
        {
            _logger.LogError("error", "Refresh tokens not equals.");
            return BadRequest("Refresh tokens not equals.");
        }

        var accessToken = JwtHelper.CreateAccessToken(userInformation.Value, _options);

        var session = Session.Create(userInformation.Value.UserId, accessToken, resultGet.Value.RefreshToken);
        if (resultGet.IsFailure)
        {
            _logger.LogError("{error}", resultGet.Error);
            return BadRequest(resultGet.Error);
        }

        var result = await _sessionsRepository.Create(session.Value);
        if (result.IsFailure)
        {
            _logger.LogError("{error}", result.Error);
            return BadRequest(result.Error);
        }

        var user = await _userManager.FindByIdAsync(userInformation.Value.UserId.ToString());

        if (user is null)
        {
            _logger.LogError("error", "No user with that id");
            return BadRequest("No user with that id");
        }

        return Ok(new TokenResponse
        {
            Id = userInformation.Value.UserId,
            Role = userInformation.Value.Role,
            AccessToken = accessToken,
            Nickname = userInformation.Value.UserName,
            Email = user.Email,
        });
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout()
    {
        string? refreshToken = Request.Cookies[DefaultAuthenticationTypes.ApplicationCookie];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest("Your refresh token not exist");
        }

        var result = await _sessionsRepository.Delete(UserId.Value);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        Response.Cookies.Delete(DefaultAuthenticationTypes.ApplicationCookie);

        return Ok("Success");
    }
}