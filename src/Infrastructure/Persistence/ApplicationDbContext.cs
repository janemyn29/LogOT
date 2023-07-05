using System.Reflection;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Entities;
using mentor_v1.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mentor_v1.Domain.Identity;
using mentor_v1.Infrastructure.Common;
using mentor_v1.Domain.Common;
using mentor_v1.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using mentor_v1.Domain;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly ILoggerFactory _loggerFactory;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<DetailTaxIncome> DetailTaxIncomes => Set<DetailTaxIncome>();
    public DbSet<AnnualWorkingDay> AnnualWorkingDays => Set<AnnualWorkingDay>();
    public DbSet<PayDay> PayDays => Set<PayDay>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<DepartmentAllowance> DepartmentAllowances => Set<DepartmentAllowance>();

    public DbSet<Coefficient> Coefficients => Set<Coefficient>();
    public DbSet<ConfigDay> ConfigDays => Set<ConfigDay>();
    public DbSet<DefaultConfig> DefaultConfigs => Set<DefaultConfig>();
    public DbSet<RegionalMinimumWage> RegionalMinimumWages => Set<RegionalMinimumWage>();

    public DbSet<ConfigWifi> ConfigWifis => Set<ConfigWifi>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ShiftConfig> ShiftConfigs => Set<ShiftConfig>();
    public DbSet<JobReport> JobReports => Set<JobReport>();
    public DbSet<ExcelContract> ExcelContracts => Set<ExcelContract>();
    public DbSet<ExcelEmployeeQuit> ExcelEmployeeQuits => Set<ExcelEmployeeQuit>();
    public DbSet<InsuranceConfig> InsuranceConfigs => Set<InsuranceConfig>();
    public DbSet<DetailTax> DetailTaxs => Set<DetailTax>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Department>()
            .HasData(
            new Department
            {
                Id = Guid.Parse("ac69dc8e-f88d-46c2-a861-c9d5ac894142"),
                Name = "Phòng IT",
                Description = "Đảm nhận công việc liên quan phần mềm",
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "Test",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "Test",
                IsDeleted = false
            }
        );

        builder.Entity<ConfigDay>()
           .HasData(
           new ConfigDay
           {
               Id = Guid.Parse("ea7cebd4-6de8-40a3-958b-f4d28ee9c843"),
               Normal = ShiftType.Full,
               Holiday = ShiftType.NotWork,
               Sunday = ShiftType.NotWork,
               Saturday = ShiftType.Morning,
               Created = DateTime.Parse("1/1/2023"),
               CreatedBy = "Test",
               LastModified = DateTime.Parse("1/1/2023"),
               LastModifiedBy = "Test",
               IsDeleted = false
           }
       );

        builder.Entity<Level>()
            .HasData(
            new Level
            {
                Id = Guid.Parse("f62f7527-3a8b-4e2a-a36b-81d9a1a5a906"),
                Name = "Nhân Viên",
                Description = "aaaaa",
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "Test",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "Test",
                IsDeleted = false
            }
        );

        builder.Entity<Position>()
            .HasData(
            new Position
            {
                Id = Guid.Parse("2949e5bc-18c4-457b-b828-86d31c53b168"),
                Name = "Tester",
                DepartmentId = Guid.Parse("ac69dc8e-f88d-46c2-a861-c9d5ac894142"),
                LevelId = Guid.Parse("f62f7527-3a8b-4e2a-a36b-81d9a1a5a906"),
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "Test",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "Test",
                IsDeleted = false
            }
        );

        builder.Entity<Coefficient>()
          .HasData(
          new Coefficient
          {
              Id = Guid.Parse("a510ba38-65d8-445c-95fd-f1b719b19c08"),
              AmountCoefficient = 1,
              TypeDate = TypeDate.Normal,
              Created = DateTime.Parse("1/1/2023"),
              CreatedBy = "Test",
              LastModified = DateTime.Parse("1/1/2023"),
              LastModifiedBy = "Test",
              IsDeleted = false
          }
      );
        builder.Entity<Coefficient>()
          .HasData(
          new Coefficient
          {
              Id = Guid.Parse("b861adcd-208c-4b6c-bef1-962cd147a6f7"),
              AmountCoefficient = 1.5,
              TypeDate = TypeDate.Saturday,
              Created = DateTime.Parse("1/1/2023"),
              CreatedBy = "Test",
              LastModified = DateTime.Parse("1/1/2023"),
              LastModifiedBy = "Test",
              IsDeleted = false
          }
      );
        builder.Entity<Coefficient>()
          .HasData(
          new Coefficient
          {
              Id = Guid.Parse("22104ebc-c6e6-4f44-a7b6-344752e8d1e5"),
              AmountCoefficient = 1.5,
              TypeDate = TypeDate.Sunday,
              Created = DateTime.Parse("1/1/2023"),
              CreatedBy = "Test",
              LastModified = DateTime.Parse("1/1/2023"),
              LastModifiedBy = "Test",
              IsDeleted = false
          }
      );
        builder.Entity<Coefficient>()
         .HasData(
         new Coefficient
         {
             Id = Guid.Parse("7fd46536-291c-40f0-8f19-0aeed5d26e63"),
             AmountCoefficient = 2,
             TypeDate = TypeDate.Holiday,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "Test",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "Test",
             IsDeleted = false
         }
     );

        builder.Entity<DefaultConfig>()
            .HasData(
            new DefaultConfig
            {
                Id = Guid.Parse("581e5321-94d3-4a13-8f95-c2938462e2fa"),
                CompanyRegionType = RegionType.Region1,
                BaseSalary = 1490000,
                PersonalTaxDeduction = 11000000,
                DependentTaxDeduction = 4400000,
                InsuranceLimit = 20,
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "",
                IsDeleted = false
            }
        );
        builder.Entity<RegionalMinimumWage>()
            .HasData(
            new RegionalMinimumWage
            {
                Id = Guid.Parse("d1564a77-716a-4e36-94f7-0f3a781548b8"),
                RegionType = RegionType.Region1,
                Amount = 4680000,
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "",
                IsDeleted = false
            }
        );
        builder.Entity<RegionalMinimumWage>()
           .HasData(
           new RegionalMinimumWage
           {
               Id = Guid.Parse("c859f162-3e32-4f41-af77-15c4628d7f22"),
               RegionType = RegionType.Region2,
               Amount = 4160000,
               Created = DateTime.Parse("1/1/2023"),
               CreatedBy = "",
               LastModified = DateTime.Parse("1/1/2023"),
               LastModifiedBy = "",
               IsDeleted = false
           }
       );
        builder.Entity<RegionalMinimumWage>()
           .HasData(
           new RegionalMinimumWage
           {
               Id = Guid.Parse("cd3262db-dd9d-409f-8944-1b8929dd9a41"),
               RegionType = RegionType.Region3,
               Amount = 3640000,
               Created = DateTime.Parse("1/1/2023"),
               CreatedBy = "",
               LastModified = DateTime.Parse("1/1/2023"),
               LastModifiedBy = "",
               IsDeleted = false
           }
       );
        builder.Entity<RegionalMinimumWage>()
          .HasData(
          new RegionalMinimumWage
          {
              Id = Guid.Parse("2c4e8e53-7de6-4b56-a6e1-8472343677a9"),
              RegionType = RegionType.Region4,
              Amount = 3250000,
              Created = DateTime.Parse("1/1/2023"),
              CreatedBy = "",
              LastModified = DateTime.Parse("1/1/2023"),
              LastModifiedBy = "",
              IsDeleted = false
          }
      );


        //TAX
        builder.Entity<DetailTaxIncome>()
         .HasData(
         new DetailTaxIncome
         {
             Id = Guid.Parse("6e64f928-88c8-4b11-8833-108a3246ab61"),
             Muc_chiu_thue_From = 0,
             Muc_chiu_thue_To = 5000000,
             He_so_tru = 0,
             Thue_suat = 5,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "",
             IsDeleted = false
         }
         );

        builder.Entity<DetailTaxIncome>()
         .HasData(
         new DetailTaxIncome
         {
             Id = Guid.Parse("3334c3d2-a2cb-41b2-a9ce-10bc284456d1"),
             Muc_chiu_thue_From = 5000000,
             Muc_chiu_thue_To = 10000000,
             He_so_tru = 250000,
             Thue_suat = 10,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "",
             IsDeleted = false
         }
        );
        builder.Entity<DetailTaxIncome>()
        .HasData(
        new DetailTaxIncome
        {
            Id = Guid.Parse("d54ee76c-e2d3-44f9-9006-784e481b881e"),
            Muc_chiu_thue_From = 10000000,
            Muc_chiu_thue_To = 18000000,
            He_so_tru = 750000,
            Thue_suat = 15,
            Created = DateTime.Parse("1/1/2023"),
            CreatedBy = "",
            LastModified = DateTime.Parse("1/1/2023"),
            LastModifiedBy = "",
            IsDeleted = false
        }
       );
        builder.Entity<DetailTaxIncome>()
        .HasData(
        new DetailTaxIncome
        {
            Id = Guid.Parse("66196c62-d126-44df-b94a-a59515e16ce4"),
            Muc_chiu_thue_From = 18000000,
            Muc_chiu_thue_To = 32000000,
            He_so_tru = 1650000,
            Thue_suat = 20,
            Created = DateTime.Parse("1/1/2023"),
            CreatedBy = "",
            LastModified = DateTime.Parse("1/1/2023"),
            LastModifiedBy = "",
            IsDeleted = false
        }
       );
        builder.Entity<DetailTaxIncome>()
       .HasData(
       new DetailTaxIncome
       {
           Id = Guid.Parse("029fe2a4-4e0c-4008-91c4-03c2753955d5"),
           Muc_chiu_thue_From = 32000000,
           Muc_chiu_thue_To = 52000000,
           He_so_tru = 3250000,
           Thue_suat = 25,
           Created = DateTime.Parse("1/1/2023"),
           CreatedBy = "",
           LastModified = DateTime.Parse("1/1/2023"),
           LastModifiedBy = "",
           IsDeleted = false
       }
      );
        builder.Entity<DetailTaxIncome>()
       .HasData(
       new DetailTaxIncome
       {
           Id = Guid.Parse("29052a9d-fb7c-42ee-b7c1-dbc159da4069"),
           Muc_chiu_thue_From = 52000000,
           Muc_chiu_thue_To = 80000000,
           He_so_tru = 5850000,
           Thue_suat = 30,
           Created = DateTime.Parse("1/1/2023"),
           CreatedBy = "",
           LastModified = DateTime.Parse("1/1/2023"),
           LastModifiedBy = "",
           IsDeleted = false
       }
      );

        builder.Entity<DetailTaxIncome>()
       .HasData(
       new DetailTaxIncome
       {
           Id = Guid.Parse("c08eeb41-0955-4f71-a5dc-ec2359a565f3"),
           Muc_chiu_thue_From = 80000000,
           Muc_chiu_thue_To = null,
           He_so_tru = 9850000,
           Thue_suat = 35,
           Created = DateTime.Parse("1/1/2023"),
           CreatedBy = "",
           LastModified = DateTime.Parse("1/1/2023"),
           LastModifiedBy = "",
           IsDeleted = false
       }
      );



        //EXCHANGE
        builder.Entity<Exchange>()
         .HasData(
         new Exchange
         {
             Id = Guid.Parse("084daab6-9d5d-46b5-8cf9-305b62587610"),
             Muc_Quy_Doi_From = 0,
             Muc_Quy_Doi_To = 4750000,
             Giam_Tru = 0,
             Thue_Suat = 0.95,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "",
             IsDeleted = false
         }
        );

        builder.Entity<Exchange>()
        .HasData(
        new Exchange
        {
            Id = Guid.Parse("3cae8d06-9386-47f1-ba90-ece556ac66e1"),
            Muc_Quy_Doi_From = 4750000,
            Muc_Quy_Doi_To = 9250000,
            Giam_Tru = 250000,
            Thue_Suat = 0.9,
            Created = DateTime.Parse("1/1/2023"),
            CreatedBy = "",
            LastModified = DateTime.Parse("1/1/2023"),
            LastModifiedBy = "",
            IsDeleted = false
        }
       );


        builder.Entity<Exchange>()
         .HasData(
         new Exchange
         {
             Id = Guid.Parse("317b9f1b-738b-4c46-b3c2-2861f3db8f2f"),
             Muc_Quy_Doi_From = 9250000,
             Muc_Quy_Doi_To = 16050000,
             Giam_Tru = 750000,
             Thue_Suat = 0.85,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "",
             IsDeleted = false
         }
        );

        builder.Entity<Exchange>()
        .HasData(
        new Exchange
        {
            Id = Guid.Parse("3b415c3e-d3de-4869-8d09-2f78ce4490c1"),
            Muc_Quy_Doi_From = 16050000,
            Muc_Quy_Doi_To = 27250000,
            Giam_Tru = 1650000,
            Thue_Suat = 0.8,
            Created = DateTime.Parse("1/1/2023"),
            CreatedBy = "",
            LastModified = DateTime.Parse("1/1/2023"),
            LastModifiedBy = "",
            IsDeleted = false
        }
       );

        builder.Entity<Exchange>()
       .HasData(
       new Exchange
       {
           Id = Guid.Parse("42021ad4-5806-42fd-a665-0faba74611c4"),
           Muc_Quy_Doi_From = 27250000,
           Muc_Quy_Doi_To = 42250000,
           Giam_Tru = 3250000,
           Thue_Suat = 0.75,
           Created = DateTime.Parse("1/1/2023"),
           CreatedBy = "",
           LastModified = DateTime.Parse("1/1/2023"),
           LastModifiedBy = "",
           IsDeleted = false
       }
      );


        builder.Entity<Exchange>()
         .HasData(
         new Exchange
         {
             Id = Guid.Parse("9776d56f-20a8-4054-badf-eb7605d70aef"),
             Muc_Quy_Doi_From = 42250000,
             Muc_Quy_Doi_To = 61850000,
             Giam_Tru = 5850000,
             Thue_Suat = 0.7,
             Created = DateTime.Parse("1/1/2023"),
             CreatedBy = "",
             LastModified = DateTime.Parse("1/1/2023"),
             LastModifiedBy = "",
             IsDeleted = false
         }
        );

        builder.Entity<Exchange>()
        .HasData(
        new Exchange
        {
            Id = Guid.Parse("9cbde439-bfef-4bf7-9d88-714bfd559cd8"),
            Muc_Quy_Doi_From = 61850000,
            Muc_Quy_Doi_To = null,
            Giam_Tru = 9850000,
            Thue_Suat = 0.65,
            Created = DateTime.Parse("1/1/2023"),
            CreatedBy = "",
            LastModified = DateTime.Parse("1/1/2023"),
            LastModifiedBy = "",
            IsDeleted = false
        }
       );


        //SHIFT
        builder.Entity<ShiftConfig>()
           .HasData(
           new ShiftConfig
           {
               Id = Guid.Parse("8caed193-c40e-448b-bdab-cd7cd4f24844"),
               StartTime = DateTime.Parse("08:00:00"),
               EndTime = DateTime.Parse("12:00:00"),
               ShiftEnum = ShiftEnum.Morning,
               Created = DateTime.Parse("1/1/2023"),
               CreatedBy = "Test",
               LastModified = DateTime.Parse("1/1/2023"),
               LastModifiedBy = "Test",
               IsDeleted = false
           }
       ) ;

        builder.Entity<ShiftConfig>()
           .HasData(
           new ShiftConfig
           {
               Id = Guid.Parse("25482772-d7ac-473b-9548-9ef38dfb1be1"),
               StartTime = DateTime.Parse("13:30:00"),
               EndTime = DateTime.Parse("17:30:00"),
               ShiftEnum = ShiftEnum.Afternoon,
               Created = DateTime.Parse("1/1/2023"),
               CreatedBy = "Test",
               LastModified = DateTime.Parse("1/1/2023"),
               LastModifiedBy = "Test",
               IsDeleted = false
           }
       );

      
        builder.Entity<InsuranceConfig>()
            .HasData(
            new InsuranceConfig
            {
                Id = Guid.Parse("38f2c77a-b5da-484b-a836-3befc8fb9b89"),
                BHXH_Emp = 8,
                BHYT_Emp= 1.5,
                BHTN_Emp = 1,
                BHXH_Comp = 17.5,
                BHYT_Comp = 3,
                BHTN_Comp=0,
                BHCD_Comp=2,
                Created = DateTime.Parse("1/1/2023"),
                CreatedBy = "Test",
                LastModified = DateTime.Parse("1/1/2023"),
                LastModifiedBy = "Test",
                IsDeleted = false
            }
        );
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }


    public DbSet<T> Get<T>() where T : BaseAuditableEntity => Set<T>();
}
