namespace Translator.Design.Domain.Responses
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TranslateRes
    {
        /// <summary>
        /// 
        /// </summary>
        public string CorrectedRomaji { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string English { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Japanese { get; set; } = string.Empty;
    }
}
