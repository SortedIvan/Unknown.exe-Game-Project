using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




//Pressing E to interact with objects
//Is player in range?
public class Interactable : MonoBehaviour
{
    [Header("Interaction section")]
    public bool isInRange;
    [SerializeField] public KeyCode interactionKey;
    public UnityEvent interactAction;
    public UnityEvent outOfRangeInteraction;


    void Start()
    {

    }

    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                interactAction.Invoke(); // This instantiates the event
            }
        }
        if (!isInRange)
        {

            outOfRangeInteraction.Invoke();

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
}
