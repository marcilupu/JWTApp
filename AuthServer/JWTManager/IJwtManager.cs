using JWTManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTManager
{
    public interface IJwtManager
    {
        public string GenerateJwt(string username);
        public bool ValidateJwt(string token);

        public JwtPayload GetPayload(string token);
    }
}
