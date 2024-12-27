using myEcommerceDataAccess.Data;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceDataAccess.RepositoriesImplementation
{
	public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void UpDate(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
			
		}

		public void UpDateOrderStatus(int id, string orderStatus, string? paymentStatus)
		{

			var orderHeaderInDb = _context.OrderHeaders.FirstOrDefault(x=>x.Id == id);
			if (orderHeaderInDb != null)
			{
				orderHeaderInDb.OrderStatus = orderStatus;
				orderHeaderInDb.PaymentDate = DateTime.Now;

				if (paymentStatus != null)
				{
						orderHeaderInDb.PaymentStatus = paymentStatus;
				}

			}
			
		}
	}
}
