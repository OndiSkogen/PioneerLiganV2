using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static PioneerLigan.Pages.MetaGame.CreateModel;

namespace PioneerLigan.Pages
{
    public class HandlerTestModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUpdateData([FromBody] MetaGameData metaGameData)
        {
            return new JsonResult(new { success = true });
        }
    }
}
