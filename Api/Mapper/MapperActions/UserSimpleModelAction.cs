using Api.Models;
using Api.Services;
using AutoMapper;
using DAL.Entities;

namespace Api.Mapper.MapperActions
{
    public class UserSimpleModelAction : IMappingAction<User, UserSimpleModel>
    {
        private LinkGeneratorService _links;
        public UserSimpleModelAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(User source, UserSimpleModel destination, ResolutionContext context) =>
            _links.FixAvatar(source, destination);

    }
}
