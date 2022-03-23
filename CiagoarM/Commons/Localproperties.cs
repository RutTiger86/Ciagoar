using Ciagoar.Data.Response.Users;
using CiagoarM.Commons.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CiagoarM.Commons
{
    public static class Localproperties
    {
        public static Ci_User LoginUser { get; set; }

        private static JsonSerializerOptions _JsonOption;
        public static JsonSerializerOptions JsonOption
        {
            get
            {
                if (_JsonOption == null)
                {
                    _JsonOption = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,//대소문자구분안함
                    };
                    _JsonOption.Converters.Add(new DateTimeConverterUsingDateTimeParseAsFallback());
                }

                return _JsonOption;
            }
        }
    }
}
