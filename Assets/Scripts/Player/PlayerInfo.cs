using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private Image redHealth;
    [SerializeField] private Image grayHealth;
    [SerializeField] private float redDecaySpeed = 2f;
    [SerializeField] private float grayDecaySpeed = 1f;
    [SerializeField] private float grayDelayTime = 1f;
    private float target = 1f;
    private float grayHealthTimer = 0f;
    private Camera cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // billboard effect
        if (cam != null)
            canvas.transform.rotation = cam.transform.rotation;
        else
        {
            Debug.Log("Look for camera");
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        // update red health bar
        redHealth.fillAmount = Mathf.MoveTowards(
            redHealth.fillAmount,
            target,
            redDecaySpeed * Time.deltaTime
        );

        // update gray health bar
        if (grayHealthTimer > 0)
            grayHealthTimer -= Time.deltaTime;
        else
            grayHealth.fillAmount = Mathf.MoveTowards(
                grayHealth.fillAmount,
                target,
                grayDecaySpeed * Time.deltaTime
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
        target = currentHealth / (float)maxHealth;
        grayHealthTimer = grayDelayTime;
        if (IsOwner) UpdateHealthServerRpc(maxHealth, currentHealth);
    }
}
