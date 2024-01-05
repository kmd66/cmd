using AutoMapper;
using CMS.Dal.DbModel;
using CMS.Model;
using CMS.Model.Interface;
using System;
using System.Threading.Tasks;


namespace CMS.Dal
{
    public abstract class BaseDataSource
    {
        public T1 Map<T1, T2>(T2 t2)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T2, T1>();
            });
            // only during development, validate your mappings; remove it before release
            //mapperConfiguration.AssertConfigurationIsValid();

            var mapper = mapperConfiguration.CreateMapper();

            var model = mapper.Map<T1>(t2);

            return model;
        }
        public IEnumerable<T1> MapList<T1, T2>(IEnumerable<T2> t2)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T2, T1>();
            });
            // only during development, validate your mappings; remove it before release
            //mapperConfiguration.AssertConfigurationIsValid();

            var mapper = mapperConfiguration.CreateMapper();

            var model = mapper.Map<IEnumerable<T1>>(t2);

            return model;
        }
    }
}
