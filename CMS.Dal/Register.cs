using CMS.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Model.Data
{
    public static class Register
    {
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            //services.AddDbContext<PblContexts>(options =>
            //options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TalaPishe;User ID =kama; Password=kama@@1389"));

            return services;

        }
    }
}
