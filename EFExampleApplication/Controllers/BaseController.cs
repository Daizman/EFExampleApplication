using Microsoft.AspNetCore.Mvc;

namespace EFExampleApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller;
