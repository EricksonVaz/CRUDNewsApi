using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Helpers;
using CRUDNewsApi.Helpers.Exceptions;
using NotImplementedException = CRUDNewsApi.Helpers.Exceptions.NotImplementedException;
using CRUDNewsApi.Models.Auth;
using CRUDNewsApi.Helpers.EmailsTemplates;

namespace CRUDNewsApi.Services
{
    public interface IAuthService
    {
        AuthenticateResponse Login(Login login);
        Task<bool> Signup(Signup signup);
        void ForgotPassword(ResetPasswordRequest resetPasswordRequest);
        void ChangePassword(ResetPassword resetPassword);
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

            if(user.Status == EStatus.Active)
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

                // save user
                _context.Users.Add(user);
                _context.SaveChanges();

                return _emailSender.SendEmailAsync(user.Email, "User Registration", RegisteredUser.composeHTML(user.FirstName, _httpContext));
            }
            catch(Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
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
