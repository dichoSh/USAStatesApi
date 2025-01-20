namespace Services.Dto
{
    public abstract class BaseStateInfoDto
    {
        public required string Name { get; set; }
    }

    public class PopulationInfoDto : BaseStateInfoDto
    {
        public int Population { get; set; }
    }
}
