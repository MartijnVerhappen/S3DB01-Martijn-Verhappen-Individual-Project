using Logic.IService;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KlantController : ControllerBase
    {
        private readonly IKlantService _klantService;

        public KlantController(IKlantService klantService)
        {
            _klantService = klantService;
        }

        [HttpPost]
        public async Task<ActionResult<Klant>> CreateKlant(Klant klant)
        {
            var createdKlant = await _klantService.CreateKlantAsync(klant);
            return CreatedAtAction(nameof(GetKlantById), new { id = createdKlant.Id }, createdKlant);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlantById(int id)
        {
            var klant = await _klantService.GetKlantByIdAsync(id);
            if (klant == null)
            {
                return NotFound();
            }
            return klant;
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<Klant>> GetKlantByUsername(string username)
        {
            var klant = await _klantService.GetKlantByUsernameAsync(username);
            if (klant == null)
            {
                return NotFound();
            }
            return klant;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKlant(int id, Klant klant)
        {
            if (id != klant.Id)
            {
                return BadRequest();
            }

            await _klantService.UpdateKlantAsync(klant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(int id)
        {
            await _klantService.DeleteKlantAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/mfa-status")]
        public async Task<IActionResult> SetMFAStatus(int id, [FromBody] bool mfaStatus)
        {
            await _klantService.SetMFAStatusAsync(id, mfaStatus);
            return NoContent();
        }

        [HttpPatch("{id}/mfa-form")]
        public async Task<IActionResult> SetMFAForm(int id, [FromBody] string mfaForm)
        {
            await _klantService.SetMFAFormAsync(id, mfaForm);
            return NoContent();
        }

        [HttpGet("{id}/winkelmand")]
        public async Task<ActionResult<Winkelmand>> GetKlantWinkelmand(int winkelmandId)
        {
            var winkelmand = await _klantService.GetKlantWinkelmandsAsync(winkelmandId);
            if (winkelmand == null)
            {
                return NotFound();
            }
            return winkelmand;
        }
    }
}
