using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private Player player;
    private const float FOOTSTEP_INTERVAL = 0.1f;
    private float footstepTimer = FOOTSTEP_INTERVAL;

    private void Update()
    {
        if (!player.IsWalking()) return;
        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0) return;
        footstepTimer = FOOTSTEP_INTERVAL;
        SFXManager.Instance.PlaySound(SFXManager.SFXType.Footstep, player.transform.position, 100);
    }
}