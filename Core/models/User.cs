using Core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.models
{
    public class User
    {


        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public byte[] PasswordHash { get; set; }

        
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
