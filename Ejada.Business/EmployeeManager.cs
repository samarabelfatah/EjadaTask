using Ejada.Data.UnitOfWork;
using Ejada.Shared;
using Ejada.Shared.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.Business
{
    public class EmployeeManager
    {
        private IUnitOfWork _unitOfWork;
        private readonly AppSettingsViewModel _appSettings;
        public EmployeeManager(IUnitOfWork unitOfWork, IOptions<AppSettingsViewModel> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public async Task<Result> GetAllEmployees(SearchCriteriaViewModel serachViewModel)
        {
            var data = await _unitOfWork.EmployeeRepository.GetAllEmployee(serachViewModel);
            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

        public async Task<Result> GetAllEmployeeForDropDown()
        {
            var data = await _unitOfWork.EmployeeRepository.GetAllEmployeeForDropDown();
            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

        public Result GetEmployeeById(long id)
        {
            var data = _unitOfWork.EmployeeRepository.GetEmployeeById(id);


            if (data != null && data.IsActive == true)
            {
                return new Result()
                {
                    Data = data,
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmployeeIsNotExist }

                };
            }

        }

        public async Task<Result> CreateEmployee(EmployeeViewModel EmployeeViewModel, string userName)
        {
            var EmployeeModel = _unitOfWork.EmployeeRepository.GetAll().Where(b => b.Name == EmployeeViewModel.Name && b.IsActive == true).FirstOrDefault();
          

            if (EmployeeModel == null)
            {
                 var empl= await _unitOfWork.EmployeeRepository.CreateEmployee(EmployeeViewModel, userName);
                _unitOfWork.Save();

                if (EmployeeViewModel.IsManager == true)
                {
                    var dep = _unitOfWork.DepartmentRepository.Get(empl.DepartmentId);
                    dep.ManagerId = empl.Id;
                    _unitOfWork.Save();
                }

                return new Result()
                {
                    Data = null,
                    IsSuccess = true,
                    Errors = new List<string>()
                };
            }
            else
            {
                return new Result()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmployeeIsAlreadyExist }
                };
            }

        }

        public async Task<Result> EditEmployee(EmployeeViewModel EmployeeViewModel, string userName)
        {
            var EmployeeModel = _unitOfWork.EmployeeRepository.GetEmployeeById(EmployeeViewModel.Id);
            if (EmployeeModel != null)
            {
                var data = await _unitOfWork.EmployeeRepository.EditEmployee(EmployeeViewModel, userName);
                _unitOfWork.Save();

                return new Result()
                {
                    Data = null,
                    IsSuccess = true,
                    Errors = new List<string>()
                };
            }
            else
            {
                return new Result()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmployeeIsAlreadyExist }
                };
            }

        }

        public async Task<Result> DeleteEmployee(long id, string userName)
        {

            var DeletedEmployee = _unitOfWork.EmployeeRepository.GetAll().Where(b => b.Id == id && b.IsActive == true).FirstOrDefault();
            if (DeletedEmployee != null)
            {

                var data = await _unitOfWork.EmployeeRepository.DeleteEmployee(id, userName);
                _unitOfWork.Save();
                return new Result()
                {
                    Data = data,
                    IsSuccess = true,
                    Errors = new List<string>()

                };
            }
            else
            {
                return new Result()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string> { Resources.EmployeeIsNotExist }
                };
            }

        }

    }

}
