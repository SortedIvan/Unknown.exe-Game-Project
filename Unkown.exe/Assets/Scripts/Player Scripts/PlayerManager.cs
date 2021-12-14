using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int keyCount;
    public GameObject interactionObject;

    public void Start()
    {
        interactionObject = GameObject.Find("E_interact_alt");
        interactionObject.SetActive(false);
    }

    public void PickupKey()
    {
        keyCount++;
    }

    public void InteractPlayer()
    {
        interactionObject.SetActive(true);
    }

    public void NoInteractPlayer()
    {
        interactionObject.SetActive(false);
    }





}
