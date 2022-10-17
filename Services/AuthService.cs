using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Exceptions;
using NotImplementedException = CRUDNewsApi.Helpers.Exceptions.NotImplementedException;
using CRUDNewsApi.Models.Auth;

namespace CRUDNewsApi.Services
{
    public interface IAuthService
    {
        AuthenticateResponse Login(Login login);
        void Signup(Signup signup);
        void ForgotPassword(ResetPasswordRequest resetPasswordRequest);
        void ChangePassword(ResetPassword resetPassword);
    }
    public class AuthService : IAuthService
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public AuthService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }
        public AuthenticateResponse Login(Login login)
        {
            var user = getUserByEmail(login.Email);

            if(!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                throw new BadRequestException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public void Signup(Signup signup)
        {
            throw new NotImplementedException();
        }
        public void ChangePassword(ResetPassword resetPassword)
        {
            throw new NotImplementedException();
        }

        public void ForgotPassword(ResetPasswordRequest resetPasswordRequest)
        {
            throw new NotImplementedException();
        }

        private User getUserByEmail(string email)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            if (user == null) throw new BadRequestException("User or password invalid");
            return user;
        }
    }
}
