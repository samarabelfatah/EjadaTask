using Ejada.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ejada.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private members
        private EjadaContext _context = null;
        private UserRepository _userRepository = null;
        private ErrorLogRepository _ErrorLogRepository = null;
        private DepartmentRepository _DepartmentRepository = null;
        private EmployeeRepository _EmployeeRepository = null;

        #endregion

        /// <summary>
        /// Constructor for UnitOfWork
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(EjadaContext context)
        {
            _context = context;
        }


        #region Repositories

        /// <summary>
        /// UserRepository   
        /// </summary>
        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);

                }
                return _userRepository;
            }
       
        }


        /// <summary>
        /// ErrorLogRepository   
        /// </summary>
        public ErrorLogRepository ErrorLogRepository
        {
            get
            {
                if (_ErrorLogRepository == null)
                {
                    _ErrorLogRepository = new ErrorLogRepository(_context);

                }
                return _ErrorLogRepository;
            }

        }



        /// <summary>
        /// DepartmentRepository   
        /// </summary>
        public DepartmentRepository DepartmentRepository
        {
            get
            {
                if (_DepartmentRepository == null)
                {
                    _DepartmentRepository = new DepartmentRepository(_context);

                }
                return _DepartmentRepository;
            }

        }

        /// <summary>
        /// DepartmentRepository   
        /// </summary>
        public EmployeeRepository EmployeeRepository
        {
            get
            {
                if (_EmployeeRepository == null)
                {
                    _EmployeeRepository = new EmployeeRepository(_context);

                }
                return _EmployeeRepository;
            }

        }

        #endregion


        /// <summary>
        /// Disposing 
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Commit saving entities 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            int saved = _context.SaveChanges();
            if (saved > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

