using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineGUI : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerOrder;
    public GameObject sushi;
    public GameObject pepe;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStringInput(string s)
    {
        playerOrder = s;
        Debug.Log(playerOrder);
    }

    public void Sushi()
    {
        GameObject a = Instantiate(sushi) as GameObject;
        a.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);
    }
    public void Pepe()
    {
        GameObject a = Instantiate(pepe) as GameObject;

    }

    public void PurchaseFromVendingMachine()
    {
        Debug.Log(playerOrder);
        switch (playerOrder)
        {
            case "3493":
                Debug.Log("Ordered sushi! Right away!");
                Sushi();
                break;
            case "3289":
                Debug.Log("Banana smoothie! Nice!");
                break;
            case "69420":
                Debug.Log("Secret menu item!");
                Pepe();
                break;
            
        }
    }
}
