using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PruebaEntityFrameworkCore.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        public string HelpPassword { get; set; }

        public string QuestionOne { get; set;}

        public string QuiestionTwo { get; set; }        
    }
}