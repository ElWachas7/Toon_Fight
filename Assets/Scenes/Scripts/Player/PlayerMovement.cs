using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;
    public int currentHealth = 100;
    private CharacterController controller;
    float vertical;
    float horizontal;
    public float Horizontal => horizontal; //animator flip render
    private Vector3 move;
    public Vector3 Move => move; //animator necesita esta variable para determinar si se mueve el player

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * Speed * Time.deltaTime);
        //para animaciones si se mueve o no
    }

    //Agregado de Nico para que el player pueda recibir daño, esta funcion es llamada desde EnemyAttackState
    public void GotHit(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hit! Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
            // Aquí puedes agregar lógica adicional para cuando el jugador muere
        }
    }
}
