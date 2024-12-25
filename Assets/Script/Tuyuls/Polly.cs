using UnityEngine;

public class Polly : RollyPolly
{
    public Polly()
    {
        Name = "Polly; The Stalwart Defender";
        maxHealth = 25;
        AttackPower = 5;
        Money = 15;
        Type = TuyulType.RollyPolly;
    }

    public override bool TakeDamage(int damage, Player playerCharacter)
    {
        // Polly tidak bisa menerima damage jika Rolly masih hidup
        if (HasPartnerAlive)
        {
            Debug.Log($"{Name} tidak bisa diserang karena Rolly masih hidup!");
            return false;
        }

        // Jika Rolly sudah mati, Polly menerima damage seperti biasa
        return base.TakeDamage(damage, playerCharacter);
    }

    public override int GetAttackPower()
    {
        return AttackPower;
    }
}