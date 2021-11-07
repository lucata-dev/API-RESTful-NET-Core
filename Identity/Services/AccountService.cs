using Application.DTOs.User;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ApiException($"No existe registrado el email {request.Email}.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.Email, request.Password, false, false);

            if (result.Succeeded)
            {
                throw new ApiException($"Credenciales no válidas.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var response = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                Username = user.UserName,
                Roles = roleList.ToList(),
                IsVerified = user.EmailConfirmed
            };

            var refreshToken = GenerateRefreshToken(ipAddress);

            response.RefreshToken = refreshToken.Token;

            return new Response<AuthenticationResponse>(response, $"Usuario autenticado: {user.UserName}");
        }

        public async Task<Response<string>> Registersync(RegisterRequest request, string origin)
        {
            var userSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userSameUserName != null)
            {
                throw new ApiException($"Usuario {request.UserName} ya se encuentra registrado.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                Name = request.Name,
                LastName = request.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var userSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userSameEmail != null)
            {
                throw new ApiException($"El email {request.Email} ya se encuentra registrado.");
            }
            else
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());

                    return new Response<string>(user.Id, message: $"Usuario registrado exitosamente, {request.UserName}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var ipAddress = IpHelper.GetIpAddress();

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredentials
            );

            return jwtSecurityToken;
        }

        private static RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };
        }

        private static string RandomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[40];

            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
