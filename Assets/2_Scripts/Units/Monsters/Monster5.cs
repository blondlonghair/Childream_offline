using MonsterSkill;

public class Monster5 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        
        base.Start();
    }
}