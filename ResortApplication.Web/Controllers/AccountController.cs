using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResortApplication.Application.Common;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Web.ViewModels;

namespace ResortApplication.Web.Controllers;

public class AccountController(
    IUnitOfWork _unitOfWork,
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    RoleManager<IdentityRole> _roleManager)
    : Controller
{

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        LoginViewModel vm = new()
        {
            RedirectUrl = returnUrl
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Register(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        RegisterViewModel vm = new()
        {
            RedirectUrl = returnUrl,
            RoleList = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (ModelState.IsValid is false)
        {
            vm.RoleList = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToListAsync();
            return View(vm);
        }

        ApplicationUser user = new()
        {
            Name = vm.Name,
            Email = vm.Email,
            PhoneNumber = vm.PhoneNumber,
            NormalizedEmail = vm.Email.ToUpper(),
            EmailConfirmed = true,
            UserName = vm.Email,
            CreatedAt = DateTime.Now
        };
        var result = await _userManager.CreateAsync(user, vm.Password);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(vm.Role))
                await _userManager.AddToRoleAsync(user, vm.Role);
            else
                await _userManager.AddToRoleAsync(user, SD.RoleCustomer);

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        vm.RoleList = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Name
        }).ToListAsync();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if(ModelState.IsValid is false)
        {
            return View(vm);
        }
        
        var user = await _userManager.FindByEmailAsync(vm.Email);
        if(user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(vm);
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(vm.RedirectUrl) && Url.IsLocalUrl(vm.RedirectUrl))
            {
                return Redirect(vm.RedirectUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(vm);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}