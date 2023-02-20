using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        playerInfo.UpdateHealth(maxHealth, currentHealth);
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            // TODO hit effects
            playerInfo.UpdateHealth(maxHealth, currentHealth);
        }
        else
        {
            // TODO death animation
            playerInfo.UpdateHealth(maxHealth, 0);
        }
    }
}
