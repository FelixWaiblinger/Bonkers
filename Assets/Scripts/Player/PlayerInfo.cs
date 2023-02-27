using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private PlayerData _playerData;
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

    void Start()
    {
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _playerName.text = _playerData.playerName;
    }

    void Update()
    {
        // billboard effect
        if (_playerCamera != null)
            _playerInfo.rotation = _playerCamera.rotation;
        else
        {
            Debug.Log("Look for camera");
            _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

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
    public void UpdateHealth(int maxHealth, int currentHealth)
    {
        _targetHealth = currentHealth / (float)maxHealth;
        _grayHealthTimer = _grayDelayTime;
        if (IsOwner) UpdateHealthServerRpc(maxHealth, currentHealth);
    }
}
