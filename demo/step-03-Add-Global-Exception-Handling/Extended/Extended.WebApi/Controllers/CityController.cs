using System;
using System.Linq;
using System.Threading.Tasks;
using Extended.WebApi.Models;
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
        
        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] City city)
        {
            System.Diagnostics.Trace.TraceInformation(" Post a City."); 
            return Ok(city);
        }
    }
}