using Ejada.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Data.UnitOfWork
{
    public interface IUnitOfWork
    {

        /// <summary>
        /// Repository that can be use for logging
        /// </summary>
        public UserRepository UserRepository { get; }
        public ErrorLogRepository ErrorLogRepository { get; }
        public DepartmentRepository DepartmentRepository { get; }
        public EmployeeRepository EmployeeRepository { get; }

        /// <summary>
        /// Commit saving entities 
        /// </summary>
        /// <returns></returns>
        bool Save();
    }

}
