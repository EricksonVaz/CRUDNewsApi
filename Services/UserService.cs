using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Pagination;
using CRUDNewsApi.Models.User;
using Microsoft.EntityFrameworkCore;

namespace CRUDNewsApi.Services
{
    public interface IUserService
    {
        PagedList<User> GetAll(int idUserLogged, UserPaginationParams paginationParameters);
        User GetById(int id);
        void Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void UpdatePassword(int id, UpdatePasswordRequest model);
        void ChangePassword(int id, ChangePasswordRequest model);
        void UpdatePhoto(int id, UpdatePhotoRequest model);
        void ChangeStatus(int id, UpdateStatusRequest model);
        void Delete(int id);
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
        public void UpdatePassword(int id, UpdatePasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(int id, ChangePasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(int id, UpdateStatusRequest model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PagedList<User> GetAll(int userLogged,UserPaginationParams pagination)
        {
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
            return PagedList<User>.ToPagedList(
                _context.Set<User>()
                .Where(x => x.Id != userLogged)
                .OrderBy(u => u.FirstName),
                pagination.PageNumber,
                pagination.PageSize
            );
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
