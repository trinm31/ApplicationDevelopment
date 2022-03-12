using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace App_Dev.Models.ViewModels
{
    public class FileModel
    {
        public string FileName { get; set; }
        public List<IFormFile> FormFile { get; set; }
    }
}