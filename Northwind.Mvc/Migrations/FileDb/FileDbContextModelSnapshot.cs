﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwind.Mvc.Data;

#nullable disable

namespace Northwind.Mvc.Migrations.FileDb
{
    [DbContext(typeof(FileDbContext))]
    partial class FileDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Northwind.Mvc.Models.FileOnDataBaseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreateOn")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UploadedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FileOnDataBase");
                });

            modelBuilder.Entity("Northwind.Mvc.Models.FileOnFileSystemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreateOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UploadedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FileOnFileSystem");
                });
#pragma warning restore 612, 618
        }
    }
}
