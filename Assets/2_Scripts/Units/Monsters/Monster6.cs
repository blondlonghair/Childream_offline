using MonsterSkill;

public class Monster6 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        
        base.Start();
    }
}