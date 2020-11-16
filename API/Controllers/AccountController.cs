using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    public class AccountController:BaseApiController {
        private readonly DataContext _dataContext; 

        public AccountController(DataContext dataContext ) {
            this._dataContext = dataContext;                        
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string userName, string password)
        {
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
        
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }
    }
}