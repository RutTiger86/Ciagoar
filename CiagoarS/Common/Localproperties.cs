using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CiagoarS.Common
{
    public static class Localproperties
    {
        private static JsonSerializerOptions _mJsonOption;
        public static JsonSerializerOptions JsonOption
        {
            get
            {
                _mJsonOption ??= new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                    };
                return _mJsonOption;
            }
        }
    }
}
