namespace Northwind.Mvc.Models;

public class FileUploadViewModel
{
    public List<FileOnDataBaseModel> FileOnDataBase { get; set; }
    public List<FileOnFileSystemModel> FileOnFileSystem { get; set; }
}