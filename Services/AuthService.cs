using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Exceptions;
using NotImplementedException = CRUDNewsApi.Helpers.Exceptions.NotImplementedException;
using KeyNotFoundException = CRUDNewsApi.Helpers.Exceptions.KeyNotFoundException;
using CRUDNewsApi.Models.Auth;
using CRUDNewsApi.Helpers.EmailsTemplates;
using System;
using Microsoft.OpenApi.Any;

namespace CRUDNewsApi.Services
{
    public interface IAuthService
    {
        AuthenticateResponse Login(Login login);
        Task<bool> Signup(Signup signup);
        void ActivateAccount(string uuid);
        Task<object> ForgotPassword(ResetPasswordRequest resetPasswordRequest);
        Task<object> ResendEmail(ResetPasswordRequest resetPasswordRequest);
        void ChangePassword(ResetPassword resetPassword);
        User UserLogged(int idUserLogged);
    }
    public class AuthService : IAuthService
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(
            DataContext context,
            IJwtUtils jwtUtils,
            IMapper mapper,
            IEmailSender emailSender,
            IHttpContextAccessor httpContext)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContext = httpContext;
        }
        public AuthenticateResponse Login(Login login)
        {
            var user = getUserByEmail(login.Email);

            if(user.Status != EStatus.Active)
                throw new BadRequestException($"This User is {user.Status}, please check your email for instructions or contact support");
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                throw new BadRequestException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public Task<bool> Signup(Signup signup)
        {

            if (_context.Users.Any(x => x.Email == signup.Email)) 
                throw new BadRequestException($"A user with {signup.Email} email already exists");

            // map model to new user object
            var user = _mapper.Map<User>(signup);

            try
            {
                // hash password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(signup.Password);
                user.Roles = ERoles.User;
                user.Status = EStatus.Inactive;
                user.Uuid = Guid.NewGuid();

                // save user
                _context.Users.Add(user);
                _context.SaveChanges();

                return _emailSender.SendEmailAsync(user.Email, "User Registration", RegisteredUser.composeHTML(user, _httpContext));
            }
            catch(Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public void ActivateAccount(string uuid)
        {
            if (!_context.Users.Any(x => x.Uuid.ToString() == uuid && x.Status == EStatus.Inactive))
                throw new KeyNotFoundException("User not Found");

            var user = _context.Users.SingleOrDefault(x => x.Uuid == Guid.Parse(uuid));
            user.Status = EStatus.Active;
            _context.Users.Update(user);
            _context.SaveChanges();

        }

        public async Task<object> ForgotPassword(ResetPasswordRequest resetPasswordRequest)
        {
            if (!_context.Users.Any(x => x.Email == resetPasswordRequest.Email && x.Status == EStatus.Active))
                return new KeyNotFoundException("User not Found");

            var user = _context.Users.SingleOrDefault(x => x.Email == resetPasswordRequest.Email);
            user.PasswordResetToken = Guid.NewGuid().ToString();
            _context.Users.Update(user);
            _context.SaveChanges();

            var emailSend = await _emailSender.SendEmailAsync(user.Email, "Recover Password", RecoverPassword.composeHTML(user, _httpContext));

            if (emailSend) return user;

            return "Error sending email, contact support to recover your password";//throw new Exception("Error sending email, contact support to recover your password");
        }

        public async Task<object> ResendEmail(ResetPasswordRequest resetPasswordRequest)
        {
            if (!_context.Users.Any(x => x.Email == resetPasswordRequest.Email && x.Status == EStatus.Inactive))
                return new KeyNotFoundException("User not Found");

            var user = _context.Users.SingleOrDefault(x => x.Email == resetPasswordRequest.Email);
            user.Uuid = Guid.NewGuid();
            _context.Users.Update(user);
            _context.SaveChanges();

            var emailSend = await _emailSender.SendEmailAsync(user.Email, "Activate Account", RegisteredUser.composeHTML(user, _httpContext));

            if (emailSend) return user;

            return "Error sending email, contact support to activate your account";//throw new Exception("Error sending email, contact support to recover your password");
        }

        public void ChangePassword(ResetPassword resetPassword)
        {
            if (!_context.Users.Any(x => x.PasswordResetToken == resetPassword.PasswordResetToken))
                throw new KeyNotFoundException("User not Found");

            try
            {
                var user = _context.Users.SingleOrDefault(x => x.PasswordResetToken == resetPassword.PasswordResetToken);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPassword.Password);
                user.PasswordResetToken = null;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public User UserLogged(int idUserLogged)
        {
            return _context.Users.Find(idUserLogged);
        }

        private User getUserByEmail(string email)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            if (user == null) throw new BadRequestException("User or password invalid");
            return user;
        }
    }
}
