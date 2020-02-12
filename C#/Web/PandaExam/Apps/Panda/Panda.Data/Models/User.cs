namespace Panda.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Packages = new List<Package>();
            this.Receipts = new List<Receipt>();
        }
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username  { get; set; }

        [Required]
        [MaxLength(20)]
        public string Email  { get; set; }

        [Required]
        public string Password  { get; set; }

        public ICollection<Package> Packages{ get; set; }

        public ICollection<Receipt> Receipts { get; set; }
    }
}
