using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels.Identity;

public class RegisterUserViewModel
{
    [Required]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string PasswordConfirm { get; set; }
}