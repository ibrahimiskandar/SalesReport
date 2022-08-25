using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reporter.CustomAtributes;

namespace Reporter.Models
{
    public class Excel
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".xlsx", ".xls" })]
        public IFormFile ExcelFile { get; set; }

        public string preview { get; set; }

    }
}
