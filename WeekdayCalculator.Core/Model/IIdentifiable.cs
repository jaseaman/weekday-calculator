namespace WeekdayCalculator.Core.Model
{
    public interface IIdentifiable<TIdentifier>
    {
        public TIdentifier Id { get; set; }
    }
}