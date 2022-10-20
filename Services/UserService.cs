using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Exceptions;
using CRUDNewsApi.Helpers.Pagination;
using CRUDNewsApi.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using KeyNotFoundException = CRUDNewsApi.Helpers.Exceptions.KeyNotFoundException;
using NotImplementedException = CRUDNewsApi.Helpers.Exceptions.NotImplementedException;

namespace CRUDNewsApi.Services
{
    public interface IUserService
    {
        PagedList<User> GetAll(int idUserLogged, UserPaginationParams paginationParameters);
        User GetById(int idUserLogged, int id);
        void Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void UpdatePassword(int idUserLogged, UpdatePasswordRequest model);
        void ChangePassword(int id, ChangePasswordRequest model);
        void UpdatePhoto(int id, UpdatePhotoRequest model);
        void ChangeStatus(int id, UpdateStatusRequest model);
        void Delete(int id);
        User GetById(int id);
    }
    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void UpdatePassword(int idUserLogged, UpdatePasswordRequest model)
        {
            if (idUserLogged == model.Id) throw new KeyNotFoundException("action not allowed");

            var userFound = getUser(model.Id);
            if(userFound==null) throw new KeyNotFoundException("User not Found");
            userFound.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            _context.Users.Update(userFound);
            _context.SaveChanges();
        }

        public void ChangePassword(int id, ChangePasswordRequest model)
        {
            var myInfo = getUser(id);
            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, myInfo.PasswordHash))
                throw new BadRequestException("Invalid old password");

            myInfo.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            _context.Users.Update(myInfo);
            _context.SaveChanges();
        }

        public void ChangeStatus(int idUserLogged, UpdateStatusRequest model)
        {
            if (idUserLogged == model.Id) throw new KeyNotFoundException("action not allowed");

            var userFound = getUser(model.Id);
            if (userFound == null) throw new KeyNotFoundException("User not Found");
            userFound.Status = model.status;
            _context.Users.Update(userFound);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PagedList<User> GetAll(int userLogged,UserPaginationParams pagination)
        {
            var search = pagination.Search;

            if(pagination.Status != null && pagination.Roles != null)
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                    .Where(x => (x.Roles == pagination.Roles && x.Status == pagination.Status) && x.Id != userLogged)
                    .OrderBy(u => u.FirstName),
                    pagination.PageNumber,
                    pagination.PageSize
                );
            else if(pagination.Status != null)
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                    .Where(x => x.Status == pagination.Status && x.Id != userLogged)
                    .OrderBy(u => u.FirstName),
                    pagination.PageNumber,
                    pagination.PageSize
                );
            else if(pagination.Roles != null)
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                    .Where(x => x.Roles == pagination.Roles && x.Id != userLogged)
                    .OrderBy(u => u.FirstName),
                    pagination.PageNumber,
                    pagination.PageSize
                );
            else if(search != null)
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                    .Where(x => x.Id != userLogged && (
                            x.FirstName.ToLower().Contains(search.ToLower()) || 
                            x.LastName.ToLower().Contains(search.ToLower()) ||
                            x.Email.ToLower().Contains(search.ToLower())
                        )
                    )
                    .OrderBy(u => u.FirstName),
                    pagination.PageNumber,
                    pagination.PageSize
                );

            return PagedList<User>.ToPagedList(
                _context.Set<User>()
                .Where(x => x.Id != userLogged)
                .OrderBy(u => u.FirstName),
                pagination.PageNumber,
                pagination.PageSize
            );
        }

        public User GetById(int idUserLogged,int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id && x.Id != idUserLogged);
            return user;
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        public void Register(RegisterRequest model)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, UpdateRequest model)
        {
            throw new NotImplementedException();
        }

        public void UpdatePhoto(int id, UpdatePhotoRequest model)
        {
            throw new NotImplementedException();
        }

        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
