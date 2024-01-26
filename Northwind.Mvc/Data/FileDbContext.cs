using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Models;

namespace Northwind.Mvc.Data;

public class FileDbContext : DbContext
{
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
    {

    }

    public virtual DbSet<FileOnDataBaseModel> FileOnDataBase { get; set; }

    public virtual DbSet<FileOnFileSystemModel> FileOnFileSystem { get; set; }
}