﻿using myEcommerceEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
	public interface IOrderDetailRepository:IGenericRepository<OrderDetail>
	{

		void UpDate (OrderDetail orderDetail);
	}
}
