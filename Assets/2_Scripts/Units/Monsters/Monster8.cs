using MonsterSkill;

public class Monster8 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        
        base.Start();
    }
}