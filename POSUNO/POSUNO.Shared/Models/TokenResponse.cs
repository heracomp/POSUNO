using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace POSUNO.Models
{
    public class TokenResponse
    {
        //[JsonProperty("$id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        //public long Id { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

    }
}
