using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Northwind.Mvc.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Northwind.Mvc.Models;
using Packt.Shared;
using Northwind.Mvc.Data;

namespace Northwind.Mvc.Controllers;

//[Authorize(Roles = "Administrator")]
public class FileManagerController : Controller 
{
    private readonly UserManager<UserExtendedForIdentity> userManager;
    private readonly FileDbContext context;

    public FileManagerController(UserManager<UserExtendedForIdentity> userManager, FileDbContext context)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<IActionResult> Index()
    {
        var fileUploadViewModel = await LoadAllFiles();
        ViewBag.Message = TempData["Message"];
        if(TempData["Alert"] is not null)
        {
            ViewBag.Alert = TempData["Alert"];
        }
        ViewBag.Alert = "success";
        
        return View(fileUploadViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string name)
    {
        var fileUploadViewModel = await SearchFile(name);
        return View(fileUploadViewModel);
    }

    private async Task<FileUploadViewModel> SearchFile(string name)
    {
        if (name is null) return null;
        var viewModel = new FileUploadViewModel();
        viewModel.FileOnDataBase = await context.FileOnDataBase.Where(m => m.Name == name).ToListAsync();
        viewModel.FileOnFileSystem = await context.FileOnFileSystem.Where(m => m.Name == name).ToListAsync();
        return viewModel;
    }

    public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
    {
        IFileIdProvider fileIdProvider = new FileOnFileSystemIdProvider(context);
        var user = await userManager.GetUserAsync(User);
        foreach (var file in files)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "/Files");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists) Directory.CreateDirectory(basePath);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var filePath = Path.Combine(basePath, file.FileName);
            var extension = Path.GetExtension(file.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fileModel = new FileOnFileSystemModel
                {
                    Id = await fileIdProvider.TakeId(),
                    CreateOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    FilePath = filePath,
                    UploadedBy = user.UserName,
                };
                context.FileOnFileSystem.Add(fileModel);
                context.SaveChanges();
                TempData["Message"] = "File successfully uploaded to File System.";
            }
            else
            {
                TempData["Message"] = "File unccessfully uploaded to File System. File allready exists.";
                TempData["Alert"] = "warning";
                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Index");   
    }

    public async Task<IActionResult> DeleteFromFileSystem(int id)
    {
        var file = await context.FileOnFileSystem.Where(i => i.Id == id).FirstOrDefaultAsync();
        if(file is not null && System.IO.File.Exists(file.FilePath))
        {       
            context.FileOnFileSystem.Remove(file);
            if(context.SaveChanges() == 1)
            {
                System.IO.File.Delete(file.FilePath);
            }
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from file system.";
        }
        else
        {
            TempData["Message"] = $"Can't remove, file is not exists.";
            TempData["Alert"] = "warning";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DownloadFileFromFileSystem(int id)
    {
        var file = await context.FileOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
        if(file is not null && System.IO.File.Exists(file.FilePath))
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }
        else
        {
            return null;
        }
    }

    public async Task<IActionResult> UploadToDataBase(List<IFormFile> files, string description)
    {
        IFileIdProvider fileIdProvider = new FileOnDataBaseIdProvider(context);
        var user = await userManager.GetUserAsync(User);
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var result = await context.FileOnDataBase.Where(m => m.Name == file.Name).FirstOrDefaultAsync();
            if (result is null)
            {
                var fileModel = new FileOnDataBaseModel
                {
                    Id = await fileIdProvider.TakeId(),
                    CreateOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    UploadedBy = user.UserName,
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                context.FileOnDataBase.Add(fileModel);
                context.SaveChanges();
                TempData["Message"] += "File successfully uploaded to Database.";
            }
            else
            {
                TempData["Message"] = "File unccessfully uploaded to Database. File allready exists.";
                TempData["Alert"] = "warning";
                return RedirectToAction("Index");
            }
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteFromDataBase(int id)
    {
        var file = await context.FileOnDataBase.Where(i => i.Id == id).FirstOrDefaultAsync();
        if (file is not null)
        {
            context.FileOnDataBase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
        }
        else
        {
            TempData["Message"] = $"Can't remove, file is not exists.";
            TempData["Alert"] = "warning";
            
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DownloadFromDataBase(int id)
    {
        var file = await context.FileOnDataBase.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (file is not null)
        {
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }
        else
        {
            return null;
        }
    }

    private async Task<FileUploadViewModel> LoadAllFiles()
    {
        var viewModel = new FileUploadViewModel();
        viewModel.FileOnDataBase = await context.FileOnDataBase.ToListAsync();   
        viewModel.FileOnFileSystem = await context.FileOnFileSystem.ToListAsync();
        return viewModel;
    }

}