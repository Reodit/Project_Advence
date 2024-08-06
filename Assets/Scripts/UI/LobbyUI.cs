using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [ContextMenu("Player Instantiate")]
    public void PlayerInstantiate()
    {
        GameManager.instance.PlayerInstantiate(1);
    }
}
