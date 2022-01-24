using AutoMapper;
using Technovert.BankApp.Models;
using Technovert.BankApp.WebApi.DTOs.AccountDTOs;
using Technovert.BankApp.WebApi.DTOs.BankDTOs;
using Technovert.BankApp.WebApi.DTOs.TransactionDTOs;

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
            CreateMap<BankDTO, Bank>().ReverseMap();
            CreateMap<CreateTransactionDTO, Transactions>().ReverseMap();
            CreateMap<GetTransactionDTO, Transactions>().ReverseMap();
            CreateMap<DefaultTransactionDTO, Transactions>().ReverseMap();
        }
    }
}