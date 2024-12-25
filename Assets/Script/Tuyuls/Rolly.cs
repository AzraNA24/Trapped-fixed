using UnityEngine;

public class Rolly : RollyPolly
{
    public Rolly()
    {
        Name = "Rolly; The Reckless Attacker";
        maxHealth = 25;
        AttackPower = 10;
        Money = 15;
        Type = TuyulType.RollyPolly;
    }

    public override int GetAttackPower()
    {
        return AttackPower;
    }
}