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

    [Route("Employee")]
    [Authorize]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly EmployeeManager _EmployeeManager;
        private readonly ExceptionManager _exceptionManager;
        private readonly IUnitOfWork _unitOfwork;



        public EmployeeController(IUnitOfWork unitOfWork, IOptions<AppSettingsViewModel> appSettings)
        {
            _unitOfwork = unitOfWork;
            _EmployeeManager = new EmployeeManager(unitOfWork, appSettings);
            _exceptionManager = new ExceptionManager(unitOfWork);
        }

        [HttpPost]
        [Route("GetAllEmployees")]
        public async Task<ActionResult<Result>> GetAllEmployees(SearchCriteriaViewModel serachViewModel)
         {
            try
            {
                var Result = await _EmployeeManager.GetAllEmployees(serachViewModel);

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

        // GET By Id
        [Route("GetEmployeeById")]
        [HttpPost]
        public ActionResult<Result> GetEmployeeById(long id)
        {
            try
            {
                var Result = _EmployeeManager.GetEmployeeById(id);
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


        // POST : CreateEmployee
        [Route("CreateEmployee")]
        [HttpPost]
        public async Task<ActionResult<Result>> CreateEmployee( EmployeeViewModel EmployeeViewModel)
        {
            try
            {
                var Result = _EmployeeManager.CreateEmployee(EmployeeViewModel, GetUserName());
                return await Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, EmployeeViewModel, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }

        }


        //POST :EditEmployee
        [Route("EditEmployee")]
        [HttpPost]
        public async Task<ActionResult<Result>> EditEmployee([FromForm] EmployeeViewModel EmployeeViewModel)
        {
            try
            {
                var Result = _EmployeeManager.EditEmployee(EmployeeViewModel, GetUserName());
                return await Result;
            }
            catch (Exception ex)
            {
                _exceptionManager.SaveLog(Request.Path, EmployeeViewModel, ex, GetUserName());
                return new Result()
                {
                    IsSuccess = false,
                    Errors = new List<string> { Resources.ExceptionMessage }
                };
            }
        }


        // DELETE : DeleteEmployee
        [Route("DeleteEmployee")]
        [HttpPost]
        public async Task<ActionResult<Result>> DeleteEmployee(long id)
        {
            try
            {
                var Result = await _EmployeeManager.DeleteEmployee(id, GetUserName());
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