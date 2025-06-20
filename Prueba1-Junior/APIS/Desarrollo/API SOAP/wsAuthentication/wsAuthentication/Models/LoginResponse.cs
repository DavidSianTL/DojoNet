using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsAuthentication.Models
{
    public class LoginResponse
    {

        public bool Success { get; set; }
        public string Messages { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}