using System.Reflection;

namespace CMS.App
{
    public class Container : Model.Interface.IContainer
    {
        private IServiceCollection services;

        public static void Init(IServiceCollection _services)
        {
            Model.Interface.IContainer.Instance = new Container(_services);
        }

        public void RegisterType<I, O>()
        {
            throw new NotImplementedException();
        }

        private Container(IServiceCollection _services) => services = _services;

        public T? GetService<T>() => services.BuildServiceProvider().GetService<T>();

        public void AddScoped<I, O>()
            => services.Add(new ServiceDescriptor(typeof(I), typeof(O), ServiceLifetime.Scoped));

        public void AddScoped(TypeInfo I, TypeInfo O)
            => services.Add(new ServiceDescriptor(I, O, ServiceLifetime.Scoped));

        public void AddSingleton<I, O>()
            => services.Add(new ServiceDescriptor(typeof(I), typeof(O), ServiceLifetime.Singleton));

        public void AddSingleton(TypeInfo I, TypeInfo O)
            => services.Add(new ServiceDescriptor(I, O, ServiceLifetime.Singleton));
    }
}
