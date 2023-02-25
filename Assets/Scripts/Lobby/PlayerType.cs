using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    public void TogglePlayerType(string type)
    {
        _playerData.prefab = type;
    }
}
