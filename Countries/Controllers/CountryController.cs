using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Countries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        public CountryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IList<Country> GetAll()
        {
            using var repo = new CountryRepository(configuration);

            return repo.GetAll().ToList();
        }

        [HttpPost]
        public int AddCountry(Country country)
        {
            using var repo = new CountryRepository(configuration);

            return repo.AddCountry(country);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var repo = new CountryRepository(configuration);

            repo.Delete(id);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Country country)
        {
            using var repo = new CountryRepository(configuration);

            repo.Update(id, country);

            return Ok();
        }

        private readonly IConfiguration configuration;
    }
}
