using MonsterSkill;

public class Monster7 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        
        base.Start();
    }
}