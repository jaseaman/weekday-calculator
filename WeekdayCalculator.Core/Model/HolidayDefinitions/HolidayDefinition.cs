using WeekdayCalculator.Core.Model.HolidayDefinitions.Util;

namespace WeekdayCalculator.Core.Model.HolidayDefinitions
{
    public class HolidayDefinition
    {
        public long Id { get; set; }
        public HolidayPlacementStrategy PlacementStrategy { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
    }
}