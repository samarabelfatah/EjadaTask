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
    public class DepartmentRepository : GenericRepository<Department>
    {
        private readonly EjadaContext _context;
        public DepartmentRepository(EjadaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<object> GetAllDepartment(SearchCriteriaViewModel serachViewModel)
        {
            var Department = (from DepartmentObj in _context.Department.Where(b => (string.IsNullOrEmpty(serachViewModel.SearchText) ? true : b.Name.Contains(serachViewModel.SearchText))
                         && b.IsActive == true).OrderBy(b => b.Name)
                                 select new DepartmentViewModel
                                 {
                                     Id = DepartmentObj.Id,
                                     Name = DepartmentObj.Name,
                                     Manager = DepartmentObj.ManagerId
                                 })
                         ;

            var fieldName = "";
            if (!string.IsNullOrEmpty(serachViewModel.FieldName))
            {
                fieldName = StringExtensions.FirstCharToUpper(serachViewModel.FieldName);
            }
            var orderdDepartmentList = Department;
            if (serachViewModel.OrderingProperty == OrderingProperty.Ascending)
            {
                if (!string.IsNullOrEmpty(serachViewModel.FieldName))
                    orderdDepartmentList = StringFieldNameSortingSupport.OrderBy(Department.AsQueryable(), fieldName);
            }
            else if (serachViewModel.OrderingProperty == OrderingProperty.Descending)
            {
                if (!string.IsNullOrEmpty(serachViewModel.FieldName))
                    orderdDepartmentList = StringFieldNameSortingSupport.OrderByDescending(Department.AsQueryable(), fieldName);
            }
            else
            {
                orderdDepartmentList = Department.OrderByDescending(b => b.Id);
            }


            int Count = orderdDepartmentList.Count();
            var PagedList = await orderdDepartmentList.ToPagedList((int)serachViewModel.PageNumber, (int)serachViewModel.PageSize).ToListAsync();

            var data = new
            {
                DepartmentPagedList = PagedList,
                DepartmentCount = Count
            };
            return data;
        }

        public void SetManager(long departmentId, long employeeid)
        {
            var departementObj = _context.Department.Find(departmentId);
            departementObj.ManagerId =  employeeid;
        }

        public Task<List<DDLDepartmentViewModel>> GetAllDepartmentForDropDown()
        {

            var Department = (from DepartmentObj in _context.Department
                             .Where(s => s.IsActive == true)
                             .OrderBy(b => b.Name)
                                 select new DDLDepartmentViewModel
                                 {
                                     Id = DepartmentObj.Id,
                                     Name = DepartmentObj.Name
                                 }).ToListAsync();

            return Department;
        }

        public DepartmentViewModel GetDepartmentById(long id)
        {
            var Department = (from DepartmentObj in _context.Department.Where(b => b.IsActive && b.Id == id)
                                select new DepartmentViewModel
                                {
                                    Id = DepartmentObj.Id,
                                    Name = DepartmentObj.Name,
                                    IsActive = DepartmentObj.IsActive
                                }).FirstOrDefault();

            return Department;
        }

        public async Task<bool> CreateDepartment(DepartmentViewModel DepartmentViewModel, string userName)
        {
            #region Add New Department

            Department DepartmentModel = new Entities.Models.Department();
            DepartmentModel.ManagerId = DepartmentViewModel.Manager;
            DepartmentModel.Name = DepartmentViewModel.Name;
            DepartmentModel.IsActive = DepartmentViewModel.IsActive;

            #endregion

            await _context.Department.AddAsync(DepartmentModel);

            return true;
        }

        public async Task<bool> EditDepartment(DepartmentViewModel DepartmentViewModel, string userName)
        {

            #region Edit Department
            Department DepartmentEntity = _context.Department.Where(b => b.Id == DepartmentViewModel.Id).FirstOrDefault();
            DepartmentEntity.Name = DepartmentViewModel.Name;
            DepartmentEntity.ManagerId = DepartmentViewModel.Manager;

            #endregion

            return true;
        }

        public async Task<bool> DeleteDepartment(long id, string userName)
        {
            Department deletedDepartment = _context.Department.Where(f => f.Id == id).FirstOrDefault();
             _context.Department.Remove(deletedDepartment);

            return true;
        }



    }
}
