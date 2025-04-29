using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Context;

namespace ServiceOrder.Repository.Repositories
{
    public class OrderDeadlineRepository : RepositoryBase<OrderDeadline>, IOrderDeadlineRepository
    {
        public OrderDeadlineRepository(AppDbContext context) : base(context) { }

        public async Task<bool> HasDeadlineForOrdername(string orderName)
        {
            return await _context.OrderDeadlines.Where(o => o.OrderIdentifier == orderName).AnyAsync();
        }
    }
}
