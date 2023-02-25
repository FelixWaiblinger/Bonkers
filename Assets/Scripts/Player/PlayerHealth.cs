using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    void Start()
    {
        _currentHealth = _maxHealth;

        _playerInfo.UpdateHealth(_maxHealth, _currentHealth);
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth > 0)
        {
            // TODO hit effects
            _playerInfo.UpdateHealth(_maxHealth, _currentHealth);
        }
        else
        {
            // TODO death animation
            _playerInfo.UpdateHealth(_maxHealth, 0);
        }
    }
}
