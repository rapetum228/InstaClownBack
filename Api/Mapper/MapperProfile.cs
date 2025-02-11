﻿using Api.Mapper.MapperActions;
using Api.Models;
using AutoMapper;
using Common;
using DAL.Entities;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //USER
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => PasswordHash.HashPassword(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime));
            CreateMap<User, UserModel>();
            CreateMap<User, UserSimpleModel>().AfterMap<UserSimpleModelAction>(); ;
            CreateMap<User, UserAvatarModel>()
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate))
                .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
                .AfterMap<UserAvatarMapperAction>();
            CreateMap<User, UserAvatarModel>()
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate))
                .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
                .AfterMap<UserAvatarMapperAction>();

            //COMMENT
            CreateMap<CommentRequestModel, Comment>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.DateTimeWriting, m => m.MapFrom(s => s.DateTimeWriting==null?DateTime.UtcNow:s.DateTimeWriting));
            CreateMap<Comment, CommentModel>()
                .ForMember(d => d.ResponseCommentId, m => m.MapFrom(s => s.ResponseComment!.Id)) ;
            CreateMap<CommentToCommentRequestModel, Comment>()
                .ForMember(d => d.DateTimeWriting, m => m.MapFrom(s => s.DateTimeWriting == null ? DateTime.UtcNow : s.DateTimeWriting));

            //ATTACHES AND METADATA
            CreateMap<MetadataModel, MetadataLinkModel>();
            CreateMap<MetadataLinkModel, PostAttach>();
            CreateMap<PostAttach, AttachExternalModel>().AfterMap<PostAttachMapperAction>();
            CreateMap<Avatar, AttachModel>();

            //POST
            CreateMap<CreatePostModel, Post>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.Attachments, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.DateTimeCreation, m => m.MapFrom(s => s.DateTimeCreation.UtcDateTime));
            CreateMap<CreatePostRequest, CreatePostModel>();
            CreateMap<Post, PostModel>()
                .ForMember(pm => pm.Contents, p => p.MapFrom(s => s.Attachments))
                .ForMember(d => d.LikesCount, m => m.MapFrom(s => s.Likes!.Count));

            //LIKE
            CreateMap<LikeRequest, Like>();


        }
    }
}
