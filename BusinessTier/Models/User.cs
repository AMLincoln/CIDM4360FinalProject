using System;
using Microsoft.EntityFrameworkCore;

namespace FinalProject
{
    public class User
    {
        public int UserId {get; set;}
        public string Username {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
    }
}