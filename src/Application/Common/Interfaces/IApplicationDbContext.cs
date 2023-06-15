using System.Transactions;
using mentor_v1.Domain.Common;
using mentor_v1.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<AnnualWorkingDay> AnnualWorkingDays { get; }
    DbSet<ConfigDay> ConfigDays { get; }
    DbSet<Coefficient> Coefficients { get; }


    //public DbSet<Blog> Blogs { get; }
    //public DbSet<BlogCategory> BlogCategorys { get; }
    //public DbSet<Certificate> Certificates { get; }
    //public DbSet<City> Citys { get; }
    //public DbSet<ClassCourse> ClassCourses { get; }
    //public DbSet<Course> Courses { get; }
    //public DbSet<CourseCategory> CourseCategorys { get; }
    //public DbSet<CourseImageMapping> CourseImageMappings { get; }
    //public DbSet<CustomerAccount> CustomerAccounts { get; }
    //public DbSet<District> Districts { get; }
    //public DbSet<Ward> Wards { get; }
    //public DbSet<Feedback> Feedbacks { get; }
    //public DbSet<Image> Images { get; }
    //public DbSet<ManagerAccount> ManagerAccounts { get; }
    //public DbSet<MentorAccount> MentorAccounts { get; }
    //public DbSet<Order> Orders { get; }
    //public DbSet<OrderDetail> OrderDetails { get; }
    //public DbSet<UserRole> UserRoles { get; }
    //public DbSet<Transaction> Transactions { get; }
    //public DbSet<Withdraw> Withdraws { get; }
    DbSet<T> Get<T>() where T : BaseAuditableEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);


}
