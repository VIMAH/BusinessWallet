using AutoMapper;
using BusinessWallet.DTOs;
using BusinessWallet.models;

namespace BusinessWallet.configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeCreateDTO, Employee>();
            CreateMap<Employee, EmployeeReadDTO>();
        }
    }
}
