using Northwind.Mvc.Models;
using Northwind.Mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Service;

public interface IFileIdProvider
{
    Task<int> TakeId();
}

public class FileOnFileSystemIdProvider : IFileIdProvider
{
    private readonly FileDbContext context;

    public FileOnFileSystemIdProvider(FileDbContext context)
    {
        this.context = context;
    }

    public async Task<int> TakeId()
    {
        if (!context.FileOnFileSystem.Any())
        {
            return 1;
        }
        var file = await context.FileOnFileSystem.MaxAsync(m => m.Id);
        return file + 1;
    }
}

public class FileOnDataBaseIdProvider : IFileIdProvider
{
    private readonly FileDbContext context;

    public FileOnDataBaseIdProvider(FileDbContext context)
    {
        this.context = context;
    }

    public async Task<int> TakeId()
    {
        if (!context.FileOnDataBase.Any())
        {
            return 1;
        }
        var file = await context.FileOnDataBase.MaxAsync(m => m.Id);
        return file + 1;
    }
}