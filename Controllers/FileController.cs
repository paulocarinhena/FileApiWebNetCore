using FileApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FileController : Controller
    {
        // faz upload para o servidor considerando rootDirectory/subDirectory
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm(Name = "files")] List<IFormFile> files, string subDirectory)
        {
            try
            {
                await Task.Run(() => FileService.SaveFile(files, subDirectory));
                return Ok(new { files.Count, Size = FileService.SizeConverter(files.Sum(f => f.Length)) });
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Parabéns, WebAPI Funcional!");
        }

        //Faz download para o client de cordo com o caminho: rootDirectory/subDirectory with single zip file
        [HttpGet("Download/{subDirectory}")]
        public async Task<IActionResult> DownloadFiles(string subDirectory)
        {
            try
            {
                var (fileType, archiveData, archiveName) = await Task.Run(() => FileService.FetechFiles(subDirectory));
                return File(archiveData, fileType, archiveName);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }


    }
}
