using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MyExpenses
{
    public static class MyExpensesConfiguration
    {
        public static IServiceCollection AddMyExpenses(this IServiceCollection service)
        {
            service.AddTransient<IUnitOfWork, MyExpensesUnitOfWork>();

            service.AddAutoMapper(typeof(MyExpensesProfile));
        }
    }
}
