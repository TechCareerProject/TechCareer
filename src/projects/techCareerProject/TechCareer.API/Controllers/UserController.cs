using Microsoft.AspNetCore.Mvc;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserWithTokenService _userWithTokenService;
    public UserController(IUserService userService, IUserWithTokenService userWithTokenService)
    {
        _userService = userService;
        _userWithTokenService = userWithTokenService;
    }



















}
