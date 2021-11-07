using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.DTOs.User
{
    public class RegisterRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
