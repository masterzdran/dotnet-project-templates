using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extended.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Extended.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private static readonly string[] Cities = new[]
        {
            "Rome", "Lisbon","Paris","London","New York","Madrid"
        };
        
        /// <summary>
        /// Gets a collection of cities.
        /// </summary>
        /// <returns>Collection of <see cref="City"/>. </returns>
        [HttpGet(Name = "Get_Cities")]
        [ProducesResponseType(typeof(IEnumerable<City>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(IContentType.ApplicationJson)]
        public async Task<IActionResult> Get()
        {
            System.Diagnostics.Trace.TraceInformation(" Getting a list of Cities.");
            var rng = new Random();

            return Ok(Enumerable.Range(1, 5).Select(index => new City
                {
                    Id = index,
                    Name = Cities[index]
                    
                })
                .ToArray());
        }

        /// <summary>
        /// Create a new <see cref="City"/>.
        /// </summary>
        /// <param name="city">Instace of city to be created.</param>
        /// <returns>Instance of the created city.</returns>
        [HttpPost(Name = "Create_New_City")]
        [ProducesResponseType(typeof(City),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(IContentType.ApplicationJson)]
        public async Task<IActionResult> Post([FromBody] City city)
        {
            System.Diagnostics.Trace.TraceInformation(" Post a City."); 
            return Ok(city);
        }
    }
}