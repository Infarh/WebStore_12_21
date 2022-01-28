using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers.API;

[ApiController, Route("api/console")]
public class ConsoleApiController : ControllerBase
{
    [HttpGet("clear")]
    public void Clear() => Console.Clear();

    [HttpGet("write")]
    public void WriteLine(string Str) => Console.WriteLine(Str);
}