namespace AutoMetricsService.Domain.EntitiesCustom
{
    public class PercentageGlobalCustom
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; }
        public int UnitsSold { get; set; }
        public decimal PercentageOfGlobal { get; set; }
    }
}
