using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystem.EntityFrameworkCore
{
    public interface IWorkOfUnit
    {
        public UserSystemDbContext DbContext { get; set; }
        void BeginTransaction();
        int Commit();
        void Dispose();
    }
}