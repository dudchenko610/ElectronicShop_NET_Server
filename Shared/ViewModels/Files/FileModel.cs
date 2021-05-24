using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Shared.ViewModels.Files
{
    public class FileModel<T>
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }

        public T Optional { get; set; }
        public string OptionalStr { get; set; }

        public void Deserialize()
        {
            Optional = JsonConvert.DeserializeObject<T>(OptionalStr);
        }
    }
}
