using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MyExpenses.Models;

namespace MyExpenses
{
    public class MyExpensesContext : DbContext
    {
        private static readonly ILoggerFactory _myLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole();
        });

        public MyExpensesContext(DbContextOptions<MyExpensesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // define 2 foreign key as primary key
            builder
                .Entity<GroupUserModel>()
                .HasKey(x => new { x.GroupId, x.UserId });

            builder.Entity<GroupUserModel>()
                .HasOne(pt => pt.Group)
                .WithMany(p => p.GroupUser)
                .HasForeignKey(pt => pt.GroupId);

            builder.Entity<GroupUserModel>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.GroupUser)
                .HasForeignKey(pt => pt.UserId);

            builder.UseIdentityColumns();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<GroupModel> Groups { get; set; }

        public DbSet<GroupUserModel> GroupUser { get; set; }

        public DbSet<LabelModel> Labels { get; set; }

        public DbSet<ExpenseModel> Expenses { get; set; }
    }
}
