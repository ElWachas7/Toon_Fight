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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * Speed * Time.deltaTime);
        //Movement();
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
    /*
     ALTERNATIVA SIN CHARACTER CONTROLLER
    void Movement() 
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Vector3.right * Speed * Time.deltaTime);
        }
    }*/
}
