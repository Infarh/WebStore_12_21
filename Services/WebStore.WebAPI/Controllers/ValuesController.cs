using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebStore.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
       .Select(i => (Id: i, Value: $"Value-{i}"))
       .ToDictionary(v => v.Id, v => v.Value);

    private readonly ILogger<ValuesController> _Logger;

    public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

    [HttpGet] // GET -> http://localhost:5001/api/values
    public IActionResult Get() => Ok(_Values.Values);

    [HttpGet("{Id}")] // GET -> /api/values/5
    public IActionResult GetById(int Id)
    {
        if (!_Values.ContainsKey(Id))
            return NotFound();

        return Ok(_Values[Id]);
    }

    [HttpGet("count")] // GET -> /api/values/count
    //public IActionResult Count() => Ok(_Values.Count);
    //public ActionResult<int> Count() => _Values.Count;
    public int Count() => _Values.Count;

    [HttpPost] // POST -> /api/values
    [HttpPost("add")] // POST -> /api/values/add
    public IActionResult Add([FromBody] string Value)
    {
        var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
        _Values[id] = Value;

        return CreatedAtAction(nameof(GetById), new { id });
    }

    [HttpPut("{Id}")] // PUT -> /api/values/5
    public IActionResult Replace(int Id, [FromBody] string Value)
    {
        if (!_Values.ContainsKey(Id))
            return NotFound();

        _Values[Id] = Value;

        return Ok();
    }

    [HttpDelete("{Id}")] // DELETE -> /api/values/5
    public IActionResult Delete(int Id)
    {
        if (!_Values.ContainsKey(Id))
            return NotFound();

        _Values.Remove(Id);

        return Ok();
    }
}