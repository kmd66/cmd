using Microsoft.AspNetCore.Components;
using CMS.Model;
using System.Net;
using System.Drawing;
using CMS.Model.Files;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.IdentityModel.Tokens;
using CMS.Tools;
using static System.Net.Mime.MediaTypeNames;

namespace CMS.Helper
{
    public class UploadHelper : BaseHelper
    {
        public UploadHelper(string? auth)
            :base(auth)
        {
        }

        private string currentDir(string? path)
        {
            var dir = Property.UploadPath;
            if (!string.IsNullOrEmpty(path))
                dir += path;
            return dir;
        }

        public Result<List<Item>> GetItems(ItemVM model)
        {
            if (!isAuthorize)
                return Result<List<Item>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            List<Item> items = new List<Item>();
            var dir = currentDir(model.Path);

            DirectoryInfo d = new DirectoryInfo(dir);
            var dirs = d.GetDirectories();
            var files = d.GetFiles();

            foreach (var item in dirs)
            {
                items.Add(new Item
                {
                    Path = model.Path?.Replace("\\", "/"),
                    Type = PathType.Folder,
                    Name = item.Name, 
                    Extension = FileExtensions.dir 
                });
            }

            foreach (var item in files)
            {
                FileExtensions extention = FileExtensions.Unknown;
                System.Enum.TryParse<FileExtensions>(item.Extension.Replace(".", ""), out extention);

                if (!item.Name.Contains("-thumbnail."))
                {
                    if (model.Extension != FileExtensions.Unknown && extention != model.Extension)
                        continue;
                    if (!string.IsNullOrEmpty(model.Name) && !item.Name.Contains(model.Name))
                        continue;
                    items.Add(new Item
                    {
                        Path = model.Path?.Replace("\\","/"),
                        Type = PathType.File,
                        Name = item.Name,
                        Extension = extention
                    });
                }
            }

            return Result<List<Item>>.Successful(data: items);
        }

        public Result CreateFolder(ItemVM model, string folderName)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var dir = currentDir(model.Path)+ folderName;

            bool exists = System.IO.Directory.Exists(dir);
            if (exists)
                return Result.Failure(message:"پوشه با این نام وجود دارد");     
            Directory.CreateDirectory(dir);
            return Result.Successful();

        }
        public Result Delete(string path, Item model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var filePath = currentDir(path) + model.Name;

            if (model.Type == PathType.Folder)
            {
                if (!Directory.Exists(filePath))
                    return Result.Failure(message: "UH_2");
                DirectoryInfo d = new DirectoryInfo(filePath);
                var dirs = d.GetDirectories();
                var files = d.GetFiles();

                if (dirs.Length > 0 || files.Length > 0)
                    return Result.Failure(message: "به دلیل وجود زیر مجموعه در پوشه امکان تغییر نام وجود ندارد");
                Directory.Delete(filePath);
            }
            else
            {
                if (!File.Exists(filePath))
                    return Result.Failure(message: "UH_2");
                File.Delete(filePath);
                if (model.IsImg)
                {
                    var onlyName = model.Name.Replace($".{model.Extension}", "");
                    var filePathThumbnail = currentDir(path) + onlyName + "-thumbnail." + model.Extension;
                    if (File.Exists(filePathThumbnail))
                        File.Delete(filePathThumbnail);
                }
            }
            return Result.Successful();

        }
        public Result Rename(string path, Item model, string newName)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (model.Type == PathType.Folder)
                return RenameFolde(path, model, newName);
            return RenameFile(path, model, newName);

        }
        private Result RenameFile(string path, Item model, string newName)
        {
            var filePath = currentDir(path) + model.Name;
            var filePathThumbnail = filePath;

            if (model.IsImg)
            {
                var onlyName = model.Name.Replace($".{model.Extension}", "");
                filePathThumbnail = currentDir(path) + onlyName + "-thumbnail." + model.Extension;
            }

            if (!File.Exists(filePath) || !File.Exists(filePathThumbnail))
                return Result.Failure(message: "UH_1");

            FileInfo fileInfo = new FileInfo(filePath);

            var newPath = fileInfo.Directory.FullName + "\\"+ newName + "." + model.Extension;
            var newThumbnail = newPath;

            if (model.IsImg)
                newThumbnail = fileInfo.Directory.FullName + "\\" + newName +  "-thumbnail." + model.Extension;

            if (File.Exists(newPath) || File.Exists(newThumbnail))
                return Result.Failure(message: "فایل با این نام در مسیر تعریف شده وجود دارد");

            fileInfo.MoveTo(newPath);
            if (!File.Exists(newThumbnail))
                File.Move(filePathThumbnail, newThumbnail);

            return Result.Successful();
        }
        private Result RenameFolde(string path, Item model, string newName)
        {
            var filePath = currentDir(path) + model.Name;

            if (!Directory.Exists(filePath))
                return Result.Failure(message: "UH_1");


            DirectoryInfo d = new DirectoryInfo(filePath);
            var dirs = d.GetDirectories();
            var files = d.GetFiles();

            if (dirs.Length>0 || files.Length>0)
                return Result.Failure(message: "به دلیل وجود زیر مجموعه در پوشه امکان تغییر نام وجود ندارد");

            var newPath = currentDir(path) + newName;

            if (Directory.Exists(newPath))
                return Result.Failure(message: "پوشه با این نام در مسیر تعریف شده وجود دارد");

            d.MoveTo(newPath);

            return Result.Successful();
        }
    }
}
