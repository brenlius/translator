using Translator.Design.Domain.Responses;

namespace Translator.Design.Application.IRepositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITranslateRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<TranslateRes> Translate(string input);
    }
}
