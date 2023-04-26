using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurrencyDAL.Entities;
using AutoMapper;
using Currency.BLL.Currency.API.Common.DTO;

namespace Currency.BLL.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<CurrencyDTO, CurrencyEntities>()
                .ForMember(dest => dest.FromCurrency, source => source.MapFrom(s => s.old_currency))
                .ForMember(dest => dest.ToCurrency, source => source.MapFrom(s => s.new_currency))
                .ForMember(dest => dest.Amout, source => source.MapFrom(s => s.old_amount))
                .ForMember(dest => dest.Result, source => source.MapFrom(s => s.new_amount))
                .ReverseMap();
        }
    }
}
