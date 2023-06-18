﻿using System.Reflection;
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
    public DbSet<DefaultConfig> DefaultConfig => Set<DefaultConfig>();
    public DbSet<RegionalMinimumWage> RegionalMinimumWage => Set<RegionalMinimumWage>();




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
               Sunday = ShiftType.NotWork ,
               Saturday= ShiftType.Morning,
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
                PersonalTaxDeduction= 11000000,
                DependentTaxDeduction = 4400000,
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
