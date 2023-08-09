using AutoMapper;
using PlatformServcie.Dtos;
using PlatformServcie.Models;

namespace PlatformServcie.Profiles
{
    public class PlatformsProfiles : Profile
    {
        public PlatformsProfiles()
        {
            //Source --> target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>();
        }
    }
}