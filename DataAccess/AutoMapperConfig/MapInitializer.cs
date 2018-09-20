using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.POCOs;
using DataAccess.XmlClasses;

namespace DataAccess.AutoMapperConfig
{
    public static class MapInitializer
    {
        public static void Activate()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RecipeElement, Recipe>()
                    .ForMember(dest => dest.Facets, opt => opt.MapFrom(src => src.Facet));

                cfg.CreateMap<TaxonomyElement, Taxonomy>();
                cfg.CreateMap<FacetElement, Facet>();
                cfg.CreateMap<NutritionElement, Nutrition>();
            });

            Mapper = config.CreateMapper();
        }


        public static IMapper Mapper { get; set; }

    }
}
