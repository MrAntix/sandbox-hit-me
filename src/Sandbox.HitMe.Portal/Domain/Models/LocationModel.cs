namespace Sandbox.HitMe.Portal.Domain.Models
{
    public class LocationModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public static readonly LocationModel Origin = new LocationModel {Longitude = 0, Latitude = 0};
    }
}