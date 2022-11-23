using Api.Models;
using Api.Services;
using AutoMapper;
using DAL.Entities;

namespace Api.Mapper.MapperActions
{
    public class PostAttachMapperAction : IMappingAction<PostAttach, AttachExternalModel>
    {
        private LinkGeneratorService _links;
        public PostAttachMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(PostAttach source, AttachExternalModel destination, ResolutionContext context)
            => _links.FixContent(source, destination);
    }
}
