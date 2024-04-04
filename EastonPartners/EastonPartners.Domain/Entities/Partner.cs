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

		[StringLength(60)]
		[Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

		[Display(Name = "Phone Number")]
        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Address { get; set; }
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City name can only contain letters and spaces")]
		public string City { get; set; }
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "State name can only contain letters and spaces")]
		public string State { get; set; }

		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }
		[MultipleUrl(ErrorMessage = "One or more URLs are invalid")]
		public string Website { get; set; }

		[Display(Name = "Partner Type")]
		public int PartnerTypeId { get; set; }
		[Display(Name = "Partner Type")]
		public PartnerType PartnerType { get; set; }
    }

	public class MultipleUrlAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value != null)
			{
				// Split the input string into individual URLs
				var urls = value.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				// Validate each URL separately
				foreach (var url in urls)
				{
					if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out Uri uriResult) ||
						(uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
					{
						// Return validation error if any URL is invalid
						return new ValidationResult(ErrorMessage);
					}
				}
			}

			return ValidationResult.Success;
		}
	}
}
