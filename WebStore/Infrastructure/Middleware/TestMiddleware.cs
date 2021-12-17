namespace WebStore.Infrastructure.Middleware;

public class TestMiddleware
{
    private readonly RequestDelegate _Next;

    public TestMiddleware(RequestDelegate Next)
    {
        _Next = Next;
    }

    public async Task Invoke(HttpContext Context)
    {
        var controller_name = Context.Request.RouteValues["controller"];
        var action_name = Context.Request.RouteValues["action"];

        // Обработка информации из Context.Request

        var processing_task = _Next(Context); // далее здесь работает оставшаяся часть конвейера

        // Выполнить какие-то действия параллельно асинхронно с остальной частью конвейера

        await processing_task;

        // Дообработка данных в Context.Response
    }
}