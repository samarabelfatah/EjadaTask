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

    public class DepartmentManager
    {
        private IUnitOfWork _unitOfWork;
        private readonly AppSettingsViewModel _appSettings;
        public DepartmentManager(IUnitOfWork unitOfWork, IOptions<AppSettingsViewModel> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public async Task<Result> GetAllDepartments(SearchCriteriaViewModel serachViewModel)
        {
            var data = await _unitOfWork.DepartmentRepository.GetAllDepartment(serachViewModel);
            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

        public async Task<Result> GetAllDepartmentForDropDown()
        {
            var data = await _unitOfWork.DepartmentRepository.GetAllDepartmentForDropDown();
            return new Result()
            {
                Data = data,
                IsSuccess = true,
                Errors = new List<string>()

            };
        }

        public Result GetDepartmentById(long id)
        {
            var data = _unitOfWork.DepartmentRepository.GetDepartmentById(id);


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
                    Errors = new List<string> { Resources.DepartmentIsNotExist }

                };
            }

        }

        public async Task<Result> CreateDepartment(DepartmentViewModel DepartmentViewModel, string userName)
        {
            var DepartmentModel = _unitOfWork.DepartmentRepository.GetAll().Where(b => b.Name == DepartmentViewModel.Name && b.IsActive == true).FirstOrDefault();

            if (DepartmentModel == null)
            {
                    var data = await _unitOfWork.DepartmentRepository.CreateDepartment(DepartmentViewModel, userName);
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
                    Errors = new List<string> { Resources.DepartmentIsAlreadyExist }
                };
            }

        }

        public async Task<Result> EditDepartment(DepartmentViewModel DepartmentViewModel, string userName)
        {
            var DepartmentModel = _unitOfWork.DepartmentRepository.GetDepartmentById(DepartmentViewModel.Id);
            if (DepartmentModel != null)
            {
                    var data = await _unitOfWork.DepartmentRepository.EditDepartment(DepartmentViewModel, userName);
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
                    Errors = new List<string> { Resources.DepartmentIsAlreadyExist }
                };
            }

        }

        public async Task<Result> DeleteDepartment(long id, string userName)
        {

            var DeletedDepartment = _unitOfWork.DepartmentRepository.GetAll().Where(b => b.Id == id && b.IsActive == true).FirstOrDefault();
            if (DeletedDepartment != null)
            {

                var data = await _unitOfWork.DepartmentRepository.DeleteDepartment(id, userName);
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
                    Errors = new List<string> { Resources.DepartmentIsNotExist }
                };
            }

        }

    }

}
