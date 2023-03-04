using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    // player data
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    // visuals
    [SerializeField] private Transform _playerInfo;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private Image _redHealth;
    [SerializeField] private Image _grayHealth;
    [SerializeField] private float _redDecaySpeed = 2f;
    [SerializeField] private float _grayDecaySpeed = 1f;
    [SerializeField] private float _grayDelayTime = 1f;
    private float _targetHealth = 1f;
    private float _grayHealthTimer = 0f;
    private Transform _playerCamera;

    public static event Action<int> PlayerDeathEvent;

    private void OnEnable()
    {
        GameManager.OnPlayerSpawned += Init;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerSpawned -= Init;
    }

    private void Init()
    {
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _playerName.text = gameObject.name;
        _currentHealth = _maxHealth;

        UpdateHealth(_maxHealth, _currentHealth);
        if (IsOwner) UpdateHealthServerRpc(_maxHealth, _currentHealth);
    }

    void Update()
    {
        // billboard effect
        if (_playerCamera != null)
            _playerInfo.rotation = _playerCamera.rotation;
        else
            _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // update red health bar
        _redHealth.fillAmount = Mathf.MoveTowards(
            _redHealth.fillAmount,
            _targetHealth,
            _redDecaySpeed * Time.deltaTime
        );

        // update gray health bar
        if (_grayHealthTimer > 0)
            _grayHealthTimer -= Time.deltaTime;
        else
            _grayHealth.fillAmount = Mathf.MoveTowards(
                _grayHealth.fillAmount,
                _targetHealth,
                _grayDecaySpeed * Time.deltaTime
            );

    }

    [ServerRpc]
    private void UpdateHealthServerRpc(int maxHealth, int currentHealth)
    {
        UpdateHealthClientRpc(maxHealth, currentHealth);
    }

    [ClientRpc]
    private void UpdateHealthClientRpc(int maxHealth, int currentHealth)
    {
        if (!IsOwner) UpdateHealth(maxHealth, currentHealth);
    }

    // set new target percentage for health bars
    private void UpdateHealth(int maxHealth, int currentHealth)
    {
        _targetHealth = currentHealth / (float)maxHealth;
        _grayHealthTimer = _grayDelayTime;
        if (IsOwner) UpdateHealthServerRpc(maxHealth, currentHealth);
    }

    public void ApplyDamage(int damage)
    {
        // TODO calculate actual damage
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            // TODO death animation
            _currentHealth = 0;
            PlayerDeathEvent.Invoke(gameObject.layer - 20);
        }

        // TODO hit effects
        UpdateHealth(_maxHealth, _currentHealth);
        if (IsOwner) UpdateHealthServerRpc(_maxHealth, _currentHealth);
    }
}
