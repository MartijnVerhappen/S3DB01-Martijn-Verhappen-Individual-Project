using Logic.IService;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Requests;

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
        public async Task<IActionResult> CreateKlant([FromBody] Klant newKlant)
        {
            if (newKlant == null)
                return BadRequest();

            var createdKlant = await _klantService.CreateKlantAsync(newKlant);
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
            if(klant == null)
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
            var klant = await _klantService.GetKlantByIdAsync(id);
            if (klant == null)
                return NotFound();

            await _klantService.DeleteKlantAsync(klant.Id);
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

        [HttpGet("{klantId}/winkelmand")]
        public async Task<ActionResult<Winkelmand>> GetKlantWinkelmand([FromRoute] int klantId)
        {
            Console.WriteLine(klantId);
            var winkelmand = await _klantService.GetKlantWinkelmandsAsync(klantId);
            if (winkelmand == null)
            {
                return NotFound();
            }
            return winkelmand;
        }

        [HttpPost("winkelmand/add")]
        public async Task<IActionResult> AddProductToWinkelmand([FromBody] ProductRequest productRequest, [FromQuery] int winkelmandId, [FromQuery] int klantId)
        {
            if (productRequest.product.Id <= 0 || productRequest.product.ProductPrijs <= 0 || productRequest.product.ProductKorting < 0 || productRequest.aantal <= 0)
            {
                return BadRequest(new { message = "Invalid product data or quantity." });
            }

            var winkelmand = await _klantService.GetKlantWinkelmandsAsync(winkelmandId);
            var klant = await _klantService.GetKlantByIdAsync(klantId);

            var winkelmandProduct = new WinkelmandProduct
            {
                ProductId = productRequest.product.Id,
                aantal = productRequest.aantal
            };

            var updatedWinkelmand = await _klantService.AddProductToWinkelmand(winkelmandProduct, winkelmand, klant);

            return Ok(updatedWinkelmand);
        }

    }
}
