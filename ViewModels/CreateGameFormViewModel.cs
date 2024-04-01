using GameZone.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace GameZone.ViewModels
{
    public class CreateGameFormViewModel: GameFormViewModel
    {
        [AllowedExtensions(FileSetting.AllowedExtensions), MaxFileSize(FileSetting.MaxFileSizeInByte)]
        public IFormFile Cover { get; set; } = default!;
    }
}
