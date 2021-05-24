using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Shared.ViewModels.Files;
using Shared.Exceptions;
using Shared.Constants;

namespace ElectronicShopDataAccessLayer.Services
{
    public class FileService
    {
        private IWebHostEnvironment appEnvironment;


        public FileService(IWebHostEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public void DeleteFile(string path, string fileName)
        {
            string _path = appEnvironment.WebRootPath + "/files/" + path + "/" + fileName;

            if (File.Exists(_path))
            { 
                File.Delete(_path);
            }

        }

        public FileContainer DownloadFile(string path, string fileName)
        {

            string fullPath = appEnvironment.WebRootPath + "/files/" + path + "/" + fileName;

            FileContainer model = new FileContainer
            {
                FileName = fileName,
                FileType = System.IO.Path.GetExtension(fullPath),
                File = System.IO.File.ReadAllBytes(fullPath)
            };

            return model;
        }

        public async Task SaveFileAsync(IFormFile file, string fileLocation, string fileName)
        {

            Console.WriteLine("File save");
            string path = appEnvironment.WebRootPath + "/files/" + fileLocation;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            /*  string ext = System.IO.Path.GetExtension(path + file.FileName);

              Console.WriteLine("FILE_TYPE : " + ext);*/

            try
            {
                using (FileStream fileStream = new FileStream(path + "/" + fileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

            }
            catch (Exception e)
            {
                throw new ServerException(Constants.Errors.File.IMAGE_WAS_NOT_SAVED);
            }


        }

        public void CheckFileModelConsistency<T>(FileModel<T> fileModel)
        {
            if (fileModel == null || fileModel.FormFile == null || string.IsNullOrEmpty(fileModel.FileName))
            {
                throw new ServerException(Constants.Errors.File.FILE_INVALID);
            }
        }
    }
}
