using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private float yellCooldown;
    private float cooldownTimer = 0;
    private bool canYell = true;

    void Update()
    {
        Move();
        Yell();
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(horizontalMovement, verticalMovement, 0);
    }

    private void Yell()
    {
        if (!canYell)
        {
            cooldownTimer += Time.deltaTime;
        }
        if (cooldownTimer >= yellCooldown) 
        {
            canYell = true;
            cooldownTimer = 0;
        }
        if (canYell && Input.GetKeyDown("r") && this.transform.parent != null)
        {
            canYell = false;
            this.transform.parent.gameObject.GetComponent<ZoneManagment>().Yell();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            print(this.gameObject.name);
            this.transform.parent = other.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone"))
        {
            this.transform.parent = null;
        }
    }
}