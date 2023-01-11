using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KitchenConnection.BusinessLogic.Helpers;

    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
           /* CreateMap<Category>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<Unit, UnitCreateDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();*/
        }
    }

