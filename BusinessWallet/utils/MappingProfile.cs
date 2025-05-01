using AutoMapper;
using BusinessWallet.models;
using BusinessWallet.DTOs;

namespace BusinessWallet.utils
{
    /// <summary>
    /// Centraal AutoMapper-profiel voor BusinessWallet.
    /// Voeg hier alle CreateMap-regels toe.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ====== Employee ======
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeCreateDto, Employee>()
                .ForMember(d => d.FullName, opt => opt.Ignore());

            CreateMap<EmployeeUpdateDto, Employee>()
                .ForMember(d => d.FullName, opt => opt.Ignore());

            // ===> Voeg hier eventueel mappings voor andere entiteiten toe.
        }
    }
}
