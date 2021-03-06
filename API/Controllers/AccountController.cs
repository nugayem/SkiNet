using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Error;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public readonly ITokenService _tokenService ;
        public readonly IMapper _mapper ;

        public AccountController(UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager,
                                ITokenService tokenService,
                                IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper=mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailClaimsPrincipal(HttpContext.User);
            return new UserDto
            {
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token=_tokenService.CreateToken(user)
            };        
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {    
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            return _mapper.Map<AddressDto>(user.Address);

        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {    
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            user.Address= _mapper.Map<Address>(addressDto); 
            
            var result= await _userManager.UpdateAsync(user);


            System.Console.WriteLine(result.Errors.FirstOrDefault());
            if(result.Succeeded)
            {
                return Ok(_mapper.Map<AddressDto>(user.Address));
            }
            return BadRequest("Problem updating User Address");

        }
        

        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email)!=null;
        } 

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if(user ==null)
                return Unauthorized(new ApiResponse(401));
            var result =  await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);

            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token=_tokenService.CreateToken(user)
            };
        } 

        
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(CheckEmailExist(registerDto.Email).Result.Value)
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email Address is in use"}});
            var user = new AppUser
            {
                DisplayName=registerDto.DisplayName,
                Email=registerDto.Email,
                UserName=registerDto.Email
            };

            var result= await _userManager.CreateAsync(user);
            
            if(!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                Email=user.Email,
                DisplayName=user.DisplayName,
                Token=_tokenService.CreateToken(user)            
            };

        }
    }
}