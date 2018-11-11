using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;
using sexivirt.Web.Global;

namespace sexivirt.Web.Areas.Default.Controllers
{
    public class FileController : DefaultController
    {
        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {
            var destinationDir = "Content/files/uploads/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), destinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));

                var img = new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + destinationDir + uFile });
        }

        public FineUploaderResult UploadGift(FineUpload upload)
        {
            var destinationDir = "Content/files/gifts/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), destinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=256&maxheight=256&crop=auto"));

                var img = new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + destinationDir + uFile });
        }

        public FineUploaderResult UploadAvatar(FineUpload upload)
        {
            var destinationDir = "Content/files/avatars/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), destinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=800&maxheight=800&crop=auto"));
                
                var img = new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            CurrentUser.AvatarPath = "/" + destinationDir + uFile;
            Repository.UpdateUser(CurrentUser);
            return new FineUploaderResult(true, new { fileUrl = "/" + destinationDir + uFile });
        }

        public FineUploaderResult UploadPhoto(FineUpload upload)
        {
            var destinationDir = "Content/files/uploads/";
            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), destinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));

                var img = new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + destinationDir + uFile });
        }

	}
}