using Translator.Design.Application.IRepositories;
using Translator.Design.Application.IServices;
using Translator.Design.Domain.Responses;

namespace Translator.Design.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TranslateService(ITranslateRepository translateRepository) : ITranslateService
    {
        #region Declarations

        private readonly ITranslateRepository _translateRepository = translateRepository;

        #endregion Declarations

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TranslateRes> Translate(string input)
        {
            return await _translateRepository.Translate(input);
        }

        #endregion Methods
    }
}