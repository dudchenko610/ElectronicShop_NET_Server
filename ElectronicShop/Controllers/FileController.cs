using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShopDataAccessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.ViewModels.Files;

namespace ElectronicShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        private FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [Route(Constants.Routes.File.DOWNLOAD_PROFILE_FILE)]
        public FileResult DownloadProfileFile(PathModel model)
        {
            FileContainer file = _fileService.DownloadFile(Constants.File.AUTH + model.Path, model.Name);

            if (file.File == null)
            {
                return null;
            }

            return File(file.File, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
        }

  
        [Route(Constants.Routes.File.DOWNLOAD_FILE)]
        public FileResult DownloadFile(PathModel model)
        {
            FileContainer file = _fileService.DownloadFile(Constants.File.NOT_AUTH + model.Path, model.Name);
            return File(file.File, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
        }
    }
}