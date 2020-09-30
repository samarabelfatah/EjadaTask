using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ejada.Business;
using Ejada.Data.UnitOfWork;
using Ejada.Shared;
using Ejada.Shared.Helpers;
using Ejada.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ejada.API.Controllers
{

    [Route("Department")]
    [Authorize]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly DepartmentManager _DepartmentManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;



        public DepartmentController(IUnitOfWork unitOfWork, IOptions<AppSettingsViewModel> appSettings)
        {
            _unitOfwork = unitOfWork;
            _DepartmentManager = new DepartmentManager(unitOfWork, appSettings);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        [HttpPost]
        [Route("GetAllDepartments")]
        public async Task<ActionResult<Result>> GetAllDepartments(SearchCriteriaViewModel serachViewModel)
        {
            try
            {
                var Result = await _DepartmentManager.GetAllDepartments(serachViewModel);

                return Result;

            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, serachViewModel, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }
        [HttpGet]
        [Route("GetDepartmentsList")]
        public async Task<ActionResult<Result>> GetDepartmentsList()
        {
            try
            {
                var Result = await _DepartmentManager.GetAllDepartmentForDropDown();

                return Result;

            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path,0, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }

        // GET By Id
        [Route("GetDepartmentById")]
        [HttpPost]
        public ActionResult<Result> GetDepartmentById(long id)
        {
            try
            {
                var Result = _DepartmentManager.GetDepartmentById(id);
                return Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, id, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }


        // POST : CreateDepartment
        [Route("CreateDepartment")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateDepartment( DepartmentViewModel DepartmentViewModel)
        {
            try
            {
                var Result = _DepartmentManager.CreateDepartment(DepartmentViewModel, GetUserName());
                return await Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, DepartmentViewModel, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }


        //POST :EditDepartment
        [Route("EditDepartment")]
        [HttpPost]
        public async Task<ActionResult<Result>> EditDepartment([FromForm] DepartmentViewModel DepartmentViewModel)
        {
            try
            {
                var Result = _DepartmentManager.EditDepartment(DepartmentViewModel, GetUserName());
                return await Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, DepartmentViewModel, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }


        // DELETE : DeleteDepartment
        [Route("DeleteDepartment")]
        [HttpPost]
        public async Task<ActionResult<Result>> DeleteDepartment(long id)
        {
            try
            {
                var Result = await _DepartmentManager.DeleteDepartment(id, GetUserName());
                return Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, id, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }
    }

}