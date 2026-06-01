namespace Amanaje_API.DTOs
{
    public class ObservacaoClimaticaResponseDto
    {
        public int IdObservacao { get; set; }
        public int IdRegiao { get; set; }
        public string NmFonte { get; set; } = string.Empty;
        public decimal? NrTemperaturaC { get; set; }
        public decimal? NrUmidadePct { get; set; }
        public decimal? NrPrecipMm { get; set; }
        public decimal? NrVentoKmh { get; set; }
        public decimal? NrPressaoHpa { get; set; }
        public decimal? NrRadiacaoSolar { get; set; }
        public decimal? NrIndiceUv { get; set; }
        public DateTime DtObs { get; set; }
        public DateTime DtCriadoEm { get; set; }
    }
}