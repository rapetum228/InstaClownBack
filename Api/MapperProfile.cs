﻿using AutoMapper;
using Common;

namespace Api
{
    public class MapperProfile: Profile
    {
        public MapperProfile() {
            CreateMap<Models.CreateUserModel, DAL.Entities.User>()
                .ForMember(d=>d.Id, m=>m.MapFrom(s=>Guid.NewGuid()))
                .ForMember(d=>d.PasswordHash, m=>m.MapFrom(s=>PasswordHash.HashPassword(s.Password)))
                .ForMember(d=>d.BirthDate, m=>m.MapFrom(s=>s.BirthDate.UtcDateTime));

            CreateMap<DAL.Entities.User, Models.UserModel>();
            CreateMap<DAL.Entities.Avatar, Models.AttachModel>();
        }
    }
}
