using AutoMapper;
using DatingApp.Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController: Controller
    {
    }
}
