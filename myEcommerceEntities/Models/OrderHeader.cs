using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }

		public DateTime ShippingDate { get; set; }	

		public decimal TotalPrice { get; set; }

        public string? OrderStatus { get; set; }

		public string? PaymentStatus { get; set; }

		public string? TrackingNumber { get; set; }


		public string? Carrier { get; set; }

		public DateTime PaymentDate { get; set; }

		//stripe ProPerties 

		public string? SessionId { get; set; }
        public string? PayIntentId { get; set; }

		// data of user 

		public string Name { get; set; }
		public string City { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }

		// navigation property 


		public string ApplicationUserId { get; set; }
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }






    }
}
