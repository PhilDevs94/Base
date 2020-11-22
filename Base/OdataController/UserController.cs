using Base.Core.UnitOfWork;
using Base.Domain.ViewModel;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Services.UserService;

namespace Base.OdataController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IUserService _userService;
        public UserController(
            IUserService userService,
            IUnitOfWorkAsync unitOfWorkAsync
        )
        {
            _userService = userService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IQueryable<UserViewModel>> Get()
        {
            return await _userService.GetAllUserAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.InsertUserAsync(model);
                _unitOfWorkAsync.Commit();
                var returnUser = new UserViewModel()
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    IsBanned = user.IsBanned
                };
                return Created("Created new user", returnUser);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.UpdateUserAsync(model);
                _unitOfWorkAsync.Commit();
                await TryUpdateModelAsync(model);
                return Content("Updated User", "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]

        public IActionResult Delete([FromODataUri] Guid key)
        {
            _userService.Delete(key);
            _unitOfWorkAsync.Commit();
            return StatusCode(200, new { Deleted = "Success" });

        }
    }
}
