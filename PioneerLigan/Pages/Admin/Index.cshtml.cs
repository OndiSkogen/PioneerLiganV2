using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using PioneerLigan.HelperClasses;

namespace PioneerLigan.Pages.Admin
{
    [Authorize(Roles = UserRoles.Admin)]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public IndexModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public List<FolderInfo> Folders { get; set; } = new List<FolderInfo>();
        public void OnGet()
        {
            Folders.Clear();
            Folders = FillPages();
        }

        private List<FolderInfo> FillPages()
        {
            var pages = new List<FolderInfo>();

            var razorPagesDirectory = Path.Combine(_environment.ContentRootPath, "Pages");

            DirectoryInfo directoryInfo = new DirectoryInfo(razorPagesDirectory);
            DirectoryInfo[] subDirs = directoryInfo.GetDirectories();

            foreach (DirectoryInfo subDir in subDirs)
            {
                if(subDir.Name != "Shared")
                {
                    FolderInfo folder = new FolderInfo();
                    folder.Name = subDir.Name;
                    // Do something with the subdirectory
                    var files = subDir.GetFiles();
                    foreach (var f in files)
                    {
                        if (f.Name.EndsWith(".cshtml"))
                        {
                            folder.Files.Add(new PageInfo
                            {
                                Path = f.FullName,
                                Name = Path.GetFileNameWithoutExtension(f.Name)
                            });
                        }
                    }
                    pages.Add(folder);
                }                
            }
            return pages;
        }
    }

    
}
