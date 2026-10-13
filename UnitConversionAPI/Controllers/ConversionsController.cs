using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Models;
using UnitConversion.Api.Services;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionsController : ControllerBase
    {
        private readonly ConversionService _conversionService;

        public ConversionsController(ConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpPost]
        [ProducesResponseType<ConversionResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ConversionResponse> Convert([FromBody] ConversionRequest request)
        {
            try
            {
                var result = _conversionService.Convert(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
