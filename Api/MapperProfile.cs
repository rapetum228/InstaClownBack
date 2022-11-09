using Api.Models;
using AutoMapper;
using Common;
using DAL.Entities;

namespace Api
{
    public class MapperProfile: Profile
    {
        public MapperProfile() {
            CreateMap<CreateUserModel, User>()
                .ForMember(d=>d.Id, m=>m.MapFrom(s=>Guid.NewGuid()))
                .ForMember(d=>d.PasswordHash, m=>m.MapFrom(s=>PasswordHash.HashPassword(s.Password)))
                .ForMember(d=>d.BirthDate, m=>m.MapFrom(s=>s.BirthDate.UtcDateTime));

            CreateMap<User, UserModel>();
            CreateMap<Avatar, AttachModel>();
            CreateMap<PostCreateModel, Post>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.DateTimeCreation, m => m.MapFrom(s => s.DateTimeCreation.UtcDateTime));
            CreateMap<PostAttach, AttachModel>();
            CreateMap<PostAttach, PostAttachOutputModel>()
                .ForMember(pao => pao.ContentUri,pa=>pa.MapFrom(s=>"/api/Post/GetContent?contentId="+s.Id));
            CreateMap<Post, PostModel>()
                .ForMember(pm => pm.PostMetas, p => p.MapFrom(s => s.Attachments));
        }
    }
}
