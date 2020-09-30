using Ejada.Data.Repositories.GenericRepository;
using Ejada.Entities.Models;
using Ejada.Shared;
using Ejada.Shared.Helpers;
using Ejada.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Ejada.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        private readonly EjadaContext _context;
        public EmployeeRepository(EjadaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<object> GetAllEmployee(SearchCriteriaViewModel serachViewModel)
        {
            var Employee = (from EmployeeObj in _context.Employee.Where(b => (string.IsNullOrEmpty(serachViewModel.SearchText) ? true : (b.Name.Contains(serachViewModel.SearchText))|| b.Department.Name.Contains(serachViewModel.SearchText))
                         && b.IsActive == true).OrderBy(b => b.Name)
                              select new EmployeeViewModel
                              {
                                  Id = EmployeeObj.Id,
                                  Name = EmployeeObj.Name,
                                  Mobile = EmployeeObj.Mobile,
                                  Email = EmployeeObj.Email,
                                  DepartmentId = EmployeeObj.DepartmentId,
                                  DepartmentName = EmployeeObj.Department != null ? EmployeeObj.Department.Name :"" ,
                                  Age = EmployeeObj.Age,
                                  IsActive = EmployeeObj.IsActive,
                                  IsManager= EmployeeObj.Department != null && EmployeeObj.Department.ManagerId !=null ? EmployeeObj.Department.ManagerId == EmployeeObj.Id ?true:false: false

                              });

            var fieldName = "";
            if (!string.IsNullOrEmpty(serachViewModel.FieldName))
            {
                fieldName = StringExtensions.FirstCharToUpper(serachViewModel.FieldName);
            }
            var orderdEmployeeList = Employee;
            if (serachViewModel.OrderingProperty == OrderingProperty.Ascending)
            {
                if (!string.IsNullOrEmpty(serachViewModel.FieldName))
                    orderdEmployeeList = StringFieldNameSortingSupport.OrderBy(Employee.AsQueryable(), fieldName);
            }
            else if (serachViewModel.OrderingProperty == OrderingProperty.Descending)
            {
                if (!string.IsNullOrEmpty(serachViewModel.FieldName))
                    orderdEmployeeList = StringFieldNameSortingSupport.OrderByDescending(Employee.AsQueryable(), fieldName);
            }
            else
            {
                orderdEmployeeList = Employee.OrderByDescending(b => b.Id);
            }


            int Count = orderdEmployeeList.Count();
            var PagedList = await orderdEmployeeList.ToPagedList((int)serachViewModel.PageNumber, (int)serachViewModel.PageSize).ToListAsync();

            var data = new
            {
                EmployeePagedList = PagedList,
                EmployeeCount = Count
            };
            return data;
        }


        public Task<List<EmployeeViewModel>> GetAllEmployeeForDropDown()
        {

            var Employee = (from EmployeeObj in _context.Employee
                             .Where(s => s.IsActive == true)
                             .OrderBy(b => b.Name)
                              select new EmployeeViewModel
                              {
                                  Id = EmployeeObj.Id,
                                  Name = EmployeeObj.Name,
                                  IsActive = EmployeeObj.IsActive,
                              }).ToListAsync();

            return Employee;
        }

        public EmployeeViewModel GetEmployeeById(long id)
        {
            var Employee = (from EmployeeObj in _context.Employee.Where(b => b.IsActive && b.Id == id)
                              select new EmployeeViewModel
                              {
                                  Id = EmployeeObj.Id,
                                  Name = EmployeeObj.Name,
                                  IsActive = EmployeeObj.IsActive
                              }).FirstOrDefault();

            return Employee;
        }

        public async Task<Employee> CreateEmployee(EmployeeViewModel EmployeeViewModel, string userName)
        {
            #region Add New Employee

            Employee EmployeeModel = new Entities.Models.Employee();
            EmployeeModel.Name = EmployeeViewModel.Name;
            EmployeeModel.Mobile = EmployeeViewModel.Mobile;
            EmployeeModel.Email = EmployeeViewModel.Email;
            EmployeeModel.DepartmentId = EmployeeViewModel.DepartmentId;
            EmployeeModel.Age = EmployeeViewModel.Age;
            EmployeeModel.IsActive = EmployeeViewModel.IsActive;
            await _context.Employee.AddAsync(EmployeeModel);            
            #endregion
            return EmployeeModel;
        }

        public async Task<bool> EditEmployee(EmployeeViewModel EmployeeViewModel, string userName)
        {

            #region Edit Employee
            Employee EmployeeEntity = _context.Employee.Where(b => b.Id == EmployeeViewModel.Id).FirstOrDefault();
            EmployeeEntity.Name = EmployeeViewModel.Name;
            EmployeeEntity.Mobile = EmployeeViewModel.Mobile;
            EmployeeEntity.Email = EmployeeViewModel.Email;
            EmployeeEntity.DepartmentId = EmployeeViewModel.DepartmentId;
            EmployeeEntity.Age = EmployeeViewModel.Age;
            EmployeeEntity.IsActive = true;

            #endregion

            return true;
        }

        public async Task<bool> DeleteEmployee(long id, string userName)
        {
            Employee deletedEmployee = _context.Employee.Where(f => f.Id == id).FirstOrDefault();
             _context.Employee.Remove(deletedEmployee);

            return true;
        }



    
    }
}
