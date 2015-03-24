﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Models
{
    public class UserDTO
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string Email { get; set; }
        public Roles Role { get; set; }
    }

    public class UserDetailDTO
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public Roles Role { get; set; }
    }
}