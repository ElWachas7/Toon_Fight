using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerChangeWeapon playerChangeWeapon;

    public void Update()
    {
        animator.SetFloat("Speed", playerMovement.Move.magnitude);
        animator.SetInteger("WeaponIndex", playerChangeWeapon.Index);
        animator.SetFloat("CoolDown", playerChangeWeapon.Weapon.CoolDownCounter);
        if(playerMovement.Horizontal > 0.1) 
        {
            sprite.flipX = true;
        }
        else if (playerMovement.Horizontal < -0.1 )
        {

            sprite.flipX = false;
        }
    }
}
