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
	class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderDetailRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void UpDate(OrderDetail orderDetail)
		{
			_context.OrderDetails.Update(orderDetail);
			
		}
	}
}
