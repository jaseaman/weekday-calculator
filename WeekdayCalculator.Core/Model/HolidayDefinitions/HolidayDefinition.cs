using System;

namespace WeekdayCalculator.Core.Model.HolidayDefinitions
{
    public partial class HolidayDefinition : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PlacementStrategy { get; set; }
        public int? Day { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public int? WeekOfMonth { get; set; }
        public int Month { get; set; }
    }
}