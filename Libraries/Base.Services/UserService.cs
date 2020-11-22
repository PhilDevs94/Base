using Base.Core.Model;
using Base.Core.Repository;
using Base.Data;
using Base.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Services.UserService;

namespace Base.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IRepositoryAsync<User> _repository;
        protected readonly DataContext db;
        protected UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserService(IRepositoryAsync<User> repository,
             UserManager<User> userManager,
             SignInManager<User> signInManager,
             DbContextOptions<DataContext> dbOptions
        ) : base(repository)
        {
            _repository = repository;
            db = new DataContext(dbOptions);
            _userManager = userManager;
            _signInManager = signInManager;

        }
        public interface IUserService : IService<User>
        {
            Task<IQueryable<UserViewModel>> GetAllUserAsync();
            Task<User> InsertUserAsync(UserViewModel model);
            Task<UserViewModel> UpdateUserAsync(UserViewModel model);
            bool Delete(Guid Id);
        }
        public bool Delete(Guid Id)
        {
            var user = Find(Id);
            if (user != null)
            {
                user.Delete = true;
                user.LastModifiedDate = DateTime.Now;
                return true;

            }
            else
            {
                throw new Exception("Không tìm thấy người dùng này");
            }
        }
        public Task<IQueryable<UserViewModel>> GetAllUserAsync()
        {
            return Task.Run(() => GetAllUser());
        }
        public IQueryable<UserViewModel> GetAllUser()
        {
            return _repository.Queryable().Where(x => x.Delete == false)
            .Select(x => new UserViewModel()
            {
                Id= x.Id,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                IsBanned = x.IsBanned,
                Delete = x.Delete
            });
        }
        public Task<User> InsertUserAsync(UserViewModel model)
        {
            return Task.Run(() => Insert(model));
        }
        public async Task<User> Insert(UserViewModel model)
        {
            var userExisted = Queryable().Where(x =>
            (x.Email.ToUpper().Trim() == model.Email.ToUpper().Trim()
            || x.UserName.ToUpper().Trim() == model.UserName.ToUpper().Trim())
            && x.Delete == false).FirstOrDefault();
            if (userExisted != null)
            {
                throw new Exception("User existed");
            }
            else
            {
                var user = new User();
                if (model.UserName.Trim() == "" || model.UserName == null)
                {
                    user.UserName = model.Email;
                }
                else
                {
                    user.UserName = model.UserName;
                }
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IsBanned = false;
                user.PhoneNumber = model.PhoneNumber;
                user.CreateDate = DateTime.Now;
                user.LastModifiedDate = DateTime.Now;
                user.UserStatus = model.UserStatus;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return user;
                }
                else
                {
                    throw new Exception(result.Errors.ToString());
                }
            }
        }
        public async Task<UserViewModel> UpdateUserAsync(UserViewModel model)
        {
            return await Task.Run(() => Update(model));
        }
        public async Task<UserViewModel> Update(UserViewModel model)
        {
            var user = Find(model.Id);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                var phoneExisted = Queryable().Where(x => x.PhoneNumber.ToLower().Trim() == model.PhoneNumber.ToLower().Trim()).Any();
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPassword;
                if (model.PhoneNumber != null && model.PhoneNumber.Trim() != "" && !phoneExisted)
                {
                    user.PhoneNumber = model.PhoneNumber;
                }
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return model;
                }
                else
                {
                    throw new Exception(result.Errors.ToString());
                }
            }
        }
    }
}
