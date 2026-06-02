using System.Text.Json.Serialization;

namespace Amanaje_API.DTOs
{
    public class OpenMeteoResponseDto
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("current")]
        public OpenMeteoCurrent? Current { get; set; }

        [JsonPropertyName("current_units")]
        public OpenMeteoCurrentUnits? CurrentUnits { get; set; }
    }

    public class OpenMeteoCurrent
    {
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double? Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public double? RelativeHumidity2m { get; set; }

        [JsonPropertyName("precipitation")]
        public double? Precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double? WindSpeed10m { get; set; }

        [JsonPropertyName("surface_pressure")]
        public double? SurfacePressure { get; set; }

        [JsonPropertyName("shortwave_radiation")]
        public double? ShortwaveRadiation { get; set; }

        [JsonPropertyName("uv_index")]
        public double? UvIndex { get; set; }
    }

    public class OpenMeteoCurrentUnits
    {
        [JsonPropertyName("temperature_2m")]
        public string? Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public string? RelativeHumidity2m { get; set; }

        [JsonPropertyName("precipitation")]
        public string? Precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public string? WindSpeed10m { get; set; }

        [JsonPropertyName("surface_pressure")]
        public string? SurfacePressure { get; set; }

        [JsonPropertyName("shortwave_radiation")]
        public string? ShortwaveRadiation { get; set; }

        [JsonPropertyName("uv_index")]
        public string? UvIndex { get; set; }
    }
}