using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




//Pressing E to interact with objects
//Is player in range?
public class InteractableFrog : MonoBehaviour
{
    [Header("Interaction section")]
    public bool isInRange;
    public UnityEvent interactAction;
    public UnityEvent outOfRangeInteraction;


    void Start()
    {

    }

    void Update()
    {
        if (isInRange)
        {

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            collision.gameObject.GetComponent<PlayerManager>().InteractPlayer();
            Debug.Log("Player Is in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is no longer range");
            collision.gameObject.GetComponent<PlayerManager>().NoInteractPlayer();

        }
    }

    public void SetIsInRange(bool newInRange)
    {
        this.isInRange = newInRange;
    }
}
