using Translator.Design.Domain.Responses;

namespace Translator.Design.Application.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITranslateService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TranslateRes> Translate(string input);
    }
}