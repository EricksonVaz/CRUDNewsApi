using AutoMapper;
using CRUDNewsApi.Entities;
using CRUDNewsApi.Models.Auth;
using CRUDNewsApi.Models.User;

namespace CRUDNewsApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse (AUTH)
            CreateMap<User, AuthenticateResponse>();

            // Signup -> User (AUTH)
            CreateMap<Signup, User>();

            // ResetPasswordRequest -> User (AUTH)
            CreateMap<ResetPasswordRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<ResetPasswordRequest, User>(src, dest, prop)
                ));

            // ChangePasswordRequest -> User (USER AUTH)
            CreateMap<ChangePasswordRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<ChangePasswordRequest, User>(src, dest, prop)
                ));

            // RegisterRequest -> User (USER)
            CreateMap<RegisterRequest, User>();

            // UpdatePasswordRequest -> User (USER)
            CreateMap<UpdatePasswordRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<UpdatePasswordRequest, User>(src, dest, prop)
                ));


            // UpdatePhotoRequest -> User (USER)
            CreateMap<UpdatePhotoRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<UpdatePhotoRequest, User>(src, dest, prop)
                ));

            // UpdateRequest -> User (USER)
            CreateMap<UpdateRequestUser, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<UpdateRequestUser, User>(src,dest,prop)
                ));

            // UpdateStatusRequest -> User (USER)
            CreateMap<UpdateStatusRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) => mapConditionCB<UpdateStatusRequest, User>(src, dest, prop)
                ));
        }

        private static bool mapConditionCB<T,U>(T src,U dest,object prop)
        {
            // ignore null & empty string properties
            if (prop == null) return false;
            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

            return true;
        }
    }
}
