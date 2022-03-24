using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CiagoarS.Common
{
    public static class Localproperties
    {
        private static JsonSerializerOptions _jsonOption;
        public static JsonSerializerOptions JsonOption
        {
            get
            {
                if (_jsonOption == null)
                {
                    _jsonOption = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    };
                }

                return _jsonOption;
            }
        }
    }
}
