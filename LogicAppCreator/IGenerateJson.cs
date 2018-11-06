using Newtonsoft.Json.Linq;

namespace LogicAppCreator
{
    /// <summary></summary>
    public interface IGenerateJson
    {
        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        JToken GenerateJsonObject();
    }


    /// <summary></summary>
    public static class GenerateJsonExtensions
    {
        /// <summary>
        /// Generates the json.
        /// </summary>
        /// <param name="igen">The igen.</param>
        /// <returns></returns>
        public static string GenerateJson(this IGenerateJson igen) => igen.GenerateJsonObject().ToString();
    }
}