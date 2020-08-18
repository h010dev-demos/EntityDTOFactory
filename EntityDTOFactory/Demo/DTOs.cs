namespace EntityDTOFactory.Demo
{
    public abstract class AnimalDTO { }

    public class PigeonDTO : AnimalDTO
    {
        public string Name { get; set; }
        public int MaxAltitude { get; set; }
        public bool IsReal { get; set; }
        public SpecialSkill Skill { get; set; }
    }

    public class HawkDTO : AnimalDTO
    {
        public string Name { get; set; }
        public int MaxAltitude { get; set; }
        public bool IsReal { get; set; }
        public SpecialSkill Skill { get; set; }
        public Bird FlyingPartner { get; set; }
    }

    public class RattlesnakeDTO : AnimalDTO
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public bool IsPoisonous { get; set; }
        public SpecialSkill Skill { get; set; }
    }
}
