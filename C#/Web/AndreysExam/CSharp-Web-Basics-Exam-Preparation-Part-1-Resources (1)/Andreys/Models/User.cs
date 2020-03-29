using System;
using System.ComponentModel.DataAnnotations;

namespace Andreys.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Username { get; set; }

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
