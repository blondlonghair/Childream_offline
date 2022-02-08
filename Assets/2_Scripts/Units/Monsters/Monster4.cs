using MonsterSkill;

public class Monster4 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        
        base.Start();
    }
}