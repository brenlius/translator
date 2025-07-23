using Microsoft.AspNetCore.Mvc;
using Translator.Design.Application.IServices;
using Translator.Design.Domain.Requests;
using Translator.Design.Domain.Responses;

namespace Translator.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateController(ITranslateService translateService) : Controller
    {
        #region Declarations

        private readonly ITranslateService _translateService = translateService;

        #endregion Declarations

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TranslateRes>> Translate([FromBody] TranslateReq request)
        {
            if (string.IsNullOrWhiteSpace(request.Input))
                return BadRequest("Input is required.");

            TranslateRes response = await _translateService.Translate(request.Input);
            
            if (response == null)
                return NotFound("Translation not found.");

            return Ok(response);
        }

        #endregion Methods
    }
}