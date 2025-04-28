using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsServicioAuth.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}