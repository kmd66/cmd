using CMS.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Domain
{
    public static class Register
    {
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            return services;

        }
    }
}
