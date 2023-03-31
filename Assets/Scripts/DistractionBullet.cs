using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistractionBullet : MonoBehaviour
{

    public GameObject hitEffect;
    [NonSerialized] public EnemyLikes type;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Destroy(this.gameObject, 3f);

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "Bullet")
        {
            // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            // Destroy(effect, 5f);
            // FindObjectOfType<AudioManager>().Play("BulletCollision"); //play sound
        }

    }


}
