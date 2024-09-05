using AutoMapper;
using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterDto, Users>();
            CreateMap<AddBorrowedBookDto, BorrowedBook>();

        }
    }
}
