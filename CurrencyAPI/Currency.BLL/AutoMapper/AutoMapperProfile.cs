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
            CreateMap<CurrencyEntities, CurrencyDTO>().ReverseMap();
        }
    }
}
