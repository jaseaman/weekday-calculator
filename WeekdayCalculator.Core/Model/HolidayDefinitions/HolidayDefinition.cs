using WeekdayCalculator.Core.Model.HolidayDefinitions.Util;

namespace WeekdayCalculator.Core.Model.HolidayDefinitions
{
    public partial class HolidayDefinition : IIdentifiable<long>
    {
        public long Id { get; set; }
        public HolidayPlacementStrategy PlacementStrategy { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}