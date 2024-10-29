using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // יש להוסיף את המאפיין ApiController
    public class DataController : ControllerBase // צריך להוריש מ-ControllerBase
    {
        // GET api/data
        [HttpGet]
        public IActionResult GetData()
        {
            var data = new
            {
                Message = "Hello, World!",
                Date = DateTime.Now
            };

            return Ok(data); // מחזיר את המידע כסטטוס 200
        }
    }
}

