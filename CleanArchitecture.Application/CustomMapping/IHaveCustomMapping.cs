using AutoMapper;

namespace CleanArchitecture.Application.CustomMapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}
