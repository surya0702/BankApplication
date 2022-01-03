using AutoMapper;
using Technovert.BankApp.Models;
using Technovert.BankApp.WebApi.DTOs.AccountDTOs;
using Technovert.BankApp.WebApi.DTOs.BankDTOs;

namespace Technovert.BankApp.WebApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GetAccountDTO,Account>().ReverseMap();
            CreateMap<Account, PostAccountDTO>().ReverseMap();
            CreateMap<DeleteAccountDTO, Account>().ReverseMap();
            CreateMap<BalanceDTO, Account>().ReverseMap();
            CreateMap<PostBankDTO, Bank>();
        }
    }
}