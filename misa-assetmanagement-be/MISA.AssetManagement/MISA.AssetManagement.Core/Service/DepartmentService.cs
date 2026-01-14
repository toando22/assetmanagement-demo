using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Service
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        public DepartmentService(IDepartmentRepository repository) : base(repository)
        {
        }
    }
}
