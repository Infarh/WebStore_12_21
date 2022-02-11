namespace WebStore.ViewModels;

public class AjaxTestDataViewModel
{
    public int Id { get; set; }

    public string Message { get; set; }

    public DateTime ServerTime { get; set; } = DateTime.Now;
}