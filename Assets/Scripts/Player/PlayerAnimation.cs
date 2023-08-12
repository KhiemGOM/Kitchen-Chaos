using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private int id;
    private const string IS_WALKING = "IsWalking";

    private void Start()
    {
        id = Animator.StringToHash(IS_WALKING);
        animator = GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        animator.SetBool(id, player.IsWalking());
    }
}