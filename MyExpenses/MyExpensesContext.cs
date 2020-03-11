namespace MyExpenses
{
    public class MyExpensesContext
    {

    }
    //public class MyExpensesContext : DbContext
    //{
    //    private readonly IWebSettings _webSettings;

    //    public MyExpensesContext(DbContextOptions<MyExpensesContext> options, IWebSettings webSettings)
    //        : base(options)
    //    {
    //        _webSettings = webSettings;
    //    }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);

    //        // define 2 foreign key as primary key
    //        builder
    //            .Entity<UserGroupModel>()
    //            .HasKey(x => new { x.GroupId, x.UserId });

    //        builder
    //            .Entity<UserGroupModel>()
    //            .HasOne(x => x.Group)
    //            .WithMany(x => x.UserGroups);

    //        builder
    //            .Entity<ExpenseModel>()
    //            .HasOne(x => x.Label)
    //            .WithMany(x => x.Expenses);

    //        builder.ForNpgsqlUseIdentityColumns();
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        base.OnConfiguring(optionsBuilder);

    //        optionsBuilder.EnableSensitiveDataLogging();
    //    }

    //    public DbSet<LabelModel> Labels { get; set; }

    //    public DbSet<ExpenseModel> Expenses { get; set; }

    //    public DbSet<GroupModel> Groups { get; set; }

    //    public DbSet<UserModel> Users { get; set; }

    //    public DbSet<UserGroupModel> UserGroup { get; set; }
    //}
}
