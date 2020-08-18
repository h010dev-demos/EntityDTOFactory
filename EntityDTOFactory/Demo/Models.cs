namespace EntityDTOFactory.Demo
{
    public enum SpecialSkill
    {
        ROLLOVER,
        SIT,
        FLY,
        FETCH,
        BITE,
        STARE
    }

    public abstract class Animal
    {
        public Animal(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public abstract class Bird : Animal
    {
        public Bird(string name, int maxAltitude, bool isReal, int index, SpecialSkill skill)
            : base(name)
        {
            Name = name;
            MaxAltitude = maxAltitude;
            IsReal = isReal;
            Index = index;
            Skill = skill;
        }

        public int MaxAltitude { get; set; }
        public bool IsReal { get; set; }
        public int Index { get; }
        public SpecialSkill Skill { get; set; }
    }

    public abstract class Snake : Animal
    {
        public Snake(string name, double length, bool isPoisonous, SpecialSkill skill)
            : base(name)
        {
            Name = name;
            Length = length;
            IsPoisonous = isPoisonous;
            Skill = skill;
        }

        public double Length { get; set; }
        public bool IsPoisonous { get; set; }
        public SpecialSkill Skill { get; set; }
    }

    public class Pigeon : Bird
    {
        public Pigeon(string name, int maxAltitude, bool isReal, int index, SpecialSkill skill)
            : base(name, maxAltitude, isReal, index, skill)
        {
        }
    }

    public class Hawk : Bird
    {
        public Hawk(string name, int maxAltitude, bool isReal, int index, SpecialSkill skill, Bird flyingPartner)
            : base(name, maxAltitude, isReal, index, skill)
        {
            FlyingPartner = flyingPartner;
        }

        public Bird FlyingPartner { get; set; }
    }

    public class Rattlesnake : Snake
    {
        public Rattlesnake(string name, double length, bool isPoisonous, SpecialSkill skill)
            : base(name, length, isPoisonous, skill)
        {
        }
    }
}
