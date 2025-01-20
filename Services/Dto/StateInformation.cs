using DataAccess.Models;

namespace Services.Dto
{
    public class StateInformation
    {
        public string Name { get; set; } = string.Empty;
        public int Population { get; set; }
        public DateTime LastUpdatedWhen { get; set; }

        public StateInformation()
        {
        }

        public StateInformation(State state)
        {
            if (state == null)
                return;

            Name = state.Name;
            Population = state.Population;
            LastUpdatedWhen = state.LastUpdatedWhen;
        }
    }
}
