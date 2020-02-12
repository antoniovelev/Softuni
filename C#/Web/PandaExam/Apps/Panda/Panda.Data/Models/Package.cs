using Panda.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Panda.Data.Models
{
    public class Package
    {
        public Package()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Description  { get; set; }

        public decimal Weight  { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime EstimatedDeliveryDate  { get; set; }

        [Required]
        public string RecipientId  { get; set; }

        public User Recipient  { get; set; }
    }
}
