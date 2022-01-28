using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _UserManager;
    private readonly SignInManager<User> _SignInManager;
    private readonly ILogger<AccountController> _Logger;

    public AccountController(
        UserManager<User> UserManager, 
        SignInManager<User> SignInManager,
        ILogger<AccountController> Logger)
    {
        _UserManager = UserManager;
        _SignInManager = SignInManager;
        _Logger = Logger;
    }

    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel Model, [FromServices] IMapper Mapper)
    {
        if (!ModelState.IsValid)
            return View(Model);

        //var str0 = $"Hello, {Model.UserName}";
        //var str1 = string.Format("Hello, {0}", Model.UserName);
        //var str2 = string.Concat("Hello, ", Model.UserName);

        //_Logger.LogInformation($"Начало процедуры регистрации пользователя {Model.UserName}"); // неправильно
        //_Logger.LogInformation("Начало процедуры регистрации пользователя {0}", Model.UserName);
        _Logger.LogInformation("Начало процедуры регистрации пользователя {UserName}", Model.UserName);

        using (_Logger.BeginScope("Регистрация {UserName}", Model.UserName))
        {
            var user = Mapper.Map<User>(Model);

            var registration_result = await _UserManager.CreateAsync(user, Model.Password).ConfigureAwait(true);
            if (registration_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} зарегистрирован", user.UserName);

                await _UserManager.AddToRoleAsync(user, Role.Users).ConfigureAwait(true);

                _Logger.LogInformation("Пользователь {0} наделён ролью {1}", user.UserName, Role.Users);

                await _SignInManager.SignInAsync(user, false).ConfigureAwait(true);

                _Logger.LogInformation("Пользователь {0} временно вошёл в систему после регистрации", user.UserName);

                return RedirectToAction("Index", "Home");
            }

            var errors = string.Join(",", registration_result.Errors.Select(e => e.Description));
            _Logger.LogWarning("При регистрации {0} возникли ошибки:{1}", user.UserName, errors);

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        return View(Model);
    }

    public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel Model)
    {
        if (!ModelState.IsValid)
            return View(Model);

        _Logger.LogInformation("Попытка входа в систему {0}", Model.UserName);

        var login_result = await _SignInManager.PasswordSignInAsync(
            Model.UserName,
            Model.Password,
            Model.RememberMe,
            true).ConfigureAwait(true);

        if (login_result.Succeeded)
        {
            _Logger.LogInformation("Пользователь {0} успешно вошёл в систему", Model.UserName);

            //return Redirect(Model.ReturnUrl); // Не безопасно!!!

            //if(Url.IsLocalUrl(Model.ReturnUrl))
            //    return Redirect(Model.ReturnUrl);
            //return RedirectToAction("Index", "Home");

            return LocalRedirect(Model.ReturnUrl ?? "/");
        }

        _Logger.LogWarning("Ошибка входа пользователя {0}", Model.UserName);

        ModelState.AddModelError("", "Неверное имя пользователя, или пароль");

        return View(Model);
    }

    public async Task<IActionResult> Logout()
    {
        var user_name = User.Identity!.Name;
        await _SignInManager.SignOutAsync().ConfigureAwait(true);
        _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        _Logger.LogWarning("Ошибка доступа к {0}", ControllerContext.HttpContext.Request.Path);
        return View();
    }
}