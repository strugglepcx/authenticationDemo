using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authenticationMvc.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
