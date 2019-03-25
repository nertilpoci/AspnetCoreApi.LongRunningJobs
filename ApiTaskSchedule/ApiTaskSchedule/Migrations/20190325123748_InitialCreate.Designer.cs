﻿// <auto-generated />
using System;
using ApiTaskSchedule.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiTaskSchedule.Migrations
{
    [DbContext(typeof(TaskSchedulerDbContext))]
    [Migration("20190325123748_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApiTaskSchedule.DB.JobOutputs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(newsequentialid())");

                    b.Property<string>("Content");

                    b.Property<Guid>("JobId");

                    b.Property<DateTime>("Time")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("JobOutputs");
                });

            modelBuilder.Entity("ApiTaskSchedule.DB.Jobs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(newsequentialid())");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("EndTime");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.Property<int>("PercentCompleted");

                    b.Property<DateTime?>("StartTime");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("ApiTaskSchedule.DB.JobOutputs", b =>
                {
                    b.HasOne("ApiTaskSchedule.DB.Jobs", "Job")
                        .WithMany("JobOutputs")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK_Job_JobOutputs");
                });
#pragma warning restore 612, 618
        }
    }
}
