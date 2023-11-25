using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.API.Controllers
{
    //https://localhost:1234/api/regions
    //Attribute
    [Route("api/[controller]")]
    //Api controller attribute will tell this application that this controller is for API use.
    //So it will automatically validates the modal state and gives a 400 response back
    [ApiController]
    public class RegionsControllers : ControllerBase
    {
    }
}
