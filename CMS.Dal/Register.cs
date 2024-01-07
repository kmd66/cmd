using CMS.Dal;
using CMS.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

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
        private static string connectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    using (StreamReader sr = File.OpenText(AppContext.BaseDirectory+"/dalSettings.txt"))
                    {
                        connectionString = sr.ReadToEnd();
                    }

                }
                return connectionString;
            }
        }
    }
}
