using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly DbService db;
        public AuthorizationController(DbService db)
        {
            this.db = db;
        }

        [HttpPost("registration")]
        public IActionResult Post([FromBody] RegistrationDate request)
        {
            if (request.Password.Length < 5)
            {
                return BadRequest("Длина пароля должна быть хотя бы 5 символов");
            }
            if (!request.Email.Contains('@'))
            {
                return BadRequest("Некорректный адрес электронной почты");
            }
            if (request.Nickname.Length < 3)
            {
                return BadRequest("Длина имени пользователя должна быть хотя бы 3 символа");
            }
            if (request.Nickname.Length > 50)
            {
                return BadRequest("Длина имени пользователя не должна превышать 50 символов");
            }
            if (request.Email.Length > 100)
            {
                return BadRequest("Длина адреса электронной почты не должна превышать 100 символов");
            }
            if (db.CheckEmail(request.Email))
            {
                return Conflict("Данный адрес электронной почты уже был зарегистрирован");
            }
            if (db.CheckUserName(request.Nickname))
            {
                return Conflict("Данное имя пользователя занято");
            }
            db.InsertUser(request.Email, request.Nickname, request.Password);
            db.SaveChange();
            return Ok(db.GetUsers()[0].username);
        }


        [HttpPost("autorization")]
        public IActionResult Post2([FromBody] AuthorizationDate request)
        {
            if (!db.CheckEmail(request.Email))
            {
                return NotFound("Пользователь не найден");
            }
            if (!db.CheckPassword(request.Email, request.Password))
            {
                return BadRequest("Неправильный пароль");
            }
            int id = db.GetIdByEmail(request.Email);
            if (db.ActiveSession(id))
            {
                return Conflict("У вас уже есть активная сессия");
            }
            string secret_key = "YA_USTAL_POMOGITE";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("email", request.Email),
                }),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            db.InsertSession(id, tokenString, DateTime.Now.AddMinutes(20));
            db.SaveChange();
            return Ok(tokenString);
        }
    }

}
