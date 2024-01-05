using System.Reflection;

namespace CMS.Model.Interface
{
    public interface IContainer
    {
        static IContainer Instance;

        T? GetService<T>();

        void RegisterType<I, O>();

        void AddScoped<I, O>();

        void AddSingleton<I, O>();

        void AddScoped(TypeInfo I, TypeInfo O);

        void AddSingleton(TypeInfo I, TypeInfo O);
    }
}
