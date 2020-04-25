using System;
using System.Threading.Tasks;
using Extended.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Extended.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        public EnvironmentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// Return the Environment
        /// </summary>
        /// <returns>Enviroment</returns>
        [HttpGet(Name = "Get_Environment")]
        [ProducesResponseType(typeof(String),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(IContentType.ApplicationJson)]
        public async Task<IActionResult> Get()
        {
            return Ok(Configuration["Application:Environment"]);
        }
    }
}