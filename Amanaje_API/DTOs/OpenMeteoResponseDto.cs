namespace Amanaje_API.DTOs
{
    // Raiz da resposta da OpenMeteo
    public class OpenMeteoResponseDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public OpenMeteoCurrent? Current { get; set; }
        public OpenMeteoCurrentUnits? Current_Units { get; set; }
    }

    // Bloco "current" com os valores climáticos atuais
    public class OpenMeteoCurrent
    {
        public string? Time { get; set; }
        public double? Temperature_2m { get; set; }
        public double? Relative_Humidity_2m { get; set; }
        public double? Precipitation { get; set; }
        public double? Wind_Speed_10m { get; set; }
        public double? Surface_Pressure { get; set; }
        public double? Shortwave_Radiation { get; set; }
        public double? Uv_Index { get; set; }
    }

    // Bloco "current_units" para referência/documentação
    public class OpenMeteoCurrentUnits
    {
        public string? Temperature_2m { get; set; }
        public string? Relative_Humidity_2m { get; set; }
        public string? Precipitation { get; set; }
        public string? Wind_Speed_10m { get; set; }
        public string? Surface_Pressure { get; set; }
        public string? Shortwave_Radiation { get; set; }
        public string? Uv_Index { get; set; }
    }
}