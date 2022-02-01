using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseFromVendingMachine(string order)
    {
        switch (order)
        {
            case "3493":
                Debug.Log("Ordered sushi! Right away!");
                break;
        }
    }
}
