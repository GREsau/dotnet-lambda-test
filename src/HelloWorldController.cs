using Microsoft.AspNetCore.Mvc;

namespace LambdaWebApi
{
    [Route("/")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> Get()
        {
            var message = new[] { "hello", "world" };
            return new { message };
        }
    }
}
