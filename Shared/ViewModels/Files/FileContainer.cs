using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Files
{
    public class FileContainer
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
    }
}
