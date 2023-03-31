using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public PlayerController player; 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            //do something after colliding:
            Debug.Log("We hit a wall");
            player.rb.velocity = new Vector3(0f,0f, 0f);
        }
        
    }
}
