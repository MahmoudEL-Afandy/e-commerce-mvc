using myEcommerceEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
	public interface IOrderHeaderRepository:IGenericRepository<OrderHeader>
	{
		void UpDate (OrderHeader orderHeader);
		void UpDateOrderStatus(int id, string orderStatus, string? paymentStatus);

	}
}
