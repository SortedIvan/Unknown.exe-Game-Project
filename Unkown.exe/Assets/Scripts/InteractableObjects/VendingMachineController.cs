using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineController : MonoBehaviour
{

    [Header("Interaction booleans")]
    [SerializeField] public bool isVendingInteracted;
    public void Start()
    {
    }

    public void VMachineInteraction()
    {
        if (!isVendingInteracted)
        {
            isVendingInteracted = true;
            Debug.Log("Vending machine is now being interacted with");


        }
    }

    public void LeaveVendingMachine()
    {
        isVendingInteracted = false;


        Debug.Log("Vending machine is no longer being interacted with");
    }
    
    




}

