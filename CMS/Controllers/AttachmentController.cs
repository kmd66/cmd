using Azure.Core;
using CMS.Helper;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [Tools.Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public AttachmentController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost, Route("Upload")]
        public async Task<Result> Upload()
        {
            var filelist = Request.Form.Files;
            var dir = Request.Form["dir"];
            var uploadDirecotroy = $"{Property.Upload}/";
            if (dir.FirstOrDefault() != null && !string.IsNullOrEmpty(dir.FirstOrDefault()))
                uploadDirecotroy += dir.First();

            var uploadPath = Path.Combine(env.WebRootPath, uploadDirecotroy);
            if (!System.IO.Directory.Exists(uploadPath))
            {
                return Result.Failure(message: "مسیر اشتباه است");
            }

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            try
            {
                if (filelist.Count> 0)
                {
                    var file = filelist[0];
                    if (file.Length > Property.AttachmentSize)
                        return Result.Failure(message: "حجم فایل بزرگتر از حد مجاز است");

                    MemoryStream target = new MemoryStream();
                    file.CopyTo(target);
                    var fileData = target.ToArray();

                    if (!FileHelper.Validate(file.FileName, file.ContentType, fileData,
                        FileExtensions.pdf
                        | FileExtensions.jpeg
                        | FileExtensions.jpg
                        | FileExtensions.png
                        | FileExtensions.tiff
                        | FileExtensions.gif
                        | FileExtensions.bmp
                        ))
                        return Result.Failure(message: "مجاز به آپلود این نوع فایل نیستید");

                    var filePath = FileHelper.SetFileName(uploadPath, file.FileName);

                    FileHelper.WriteAllBytes(filePath, fileData);
                }
                return Result.Successful();

            }
            catch (Exception e)
            {
                return Result.Failure(message: e.Message);
            }
        }

        //[HttpPost]
        //public async Task<JsonResult> Remove(Core.Model.Attachment model)
        //{
        //    var result = await _attachmentService.Delete(model);
        //    return Json(result);
        //}

    }
}
