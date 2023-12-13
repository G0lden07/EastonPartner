using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastonPartners.Domain.Entities
{
    public class PartnerType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }

        public int PartnerTypeId { get; set; }
        public PartnerType PartnerType { get; set; }
    }
}
