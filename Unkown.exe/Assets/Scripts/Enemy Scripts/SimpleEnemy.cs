using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    private int enemyHealth = 10;
    private bool enemyDead = false;
    public GameObject enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DestroyFlyUponNullHealth();
    }

    private void DestroyFlyUponNullHealth()
    {
        if(enemyDead == true)
        {
            Destroy(this.enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (enemyHealth != 0)
            {
                this.enemyHealth -= 2;
            } else if(enemyHealth <= 0)
            {
                enemyDead = true;
            } 
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }


}
