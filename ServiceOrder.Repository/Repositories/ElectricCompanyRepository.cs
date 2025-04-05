using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Repository.Context;

namespace ServiceOrder.Repository.Repositories
{
    public class ElectricCompanyRepository : RepositoryBase<ElectricCompany>, IElectricCompanyRepository
    {
        public ElectricCompanyRepository(AppDbContext context) : base(context) { }
    }
}
