using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachineGUI : MonoBehaviour
{
    // Start is called before the first frame update
    public string playerOrder;
    public GameObject sushi;
    public GameObject pepe;
    public GameObject player;
    public GameObject InputField;
    public GameObject VendingGUI;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        InputField.GetComponent<Text>().text = playerOrder;
    }

    public void ReadStringInput(string s)
    {
        playerOrder = s;
        Debug.Log(playerOrder);
    }
    public void DeleteCharacter()
    {
        if(playerOrder.Length >= 1)
        {
            this.playerOrder = playerOrder.Remove(playerOrder.Length - 1);
        }
    }

    public void Sushi()
    {
        GameObject a = Instantiate(sushi) as GameObject;
        a.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);
    }
    public void Pepe()
    {
        GameObject a = Instantiate(pepe) as GameObject;
        a.transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);

    }

    public void One()
    {
        playerOrder += "1";
        
    }
    public void Two()
    {
        playerOrder += "2";
    }
    public void Three()
    {
        playerOrder += "3";
    }
    public void Four()
    {
        playerOrder += "4";
    }
    public void Five()
    {
        playerOrder += "5";
    }
    public void Six()
    {
        playerOrder += "6";
    }
    public void Seven()
    {
        playerOrder += "7";
    }
    public void Eight()
    {
        playerOrder += "8";
    }
    public void Nine()
    {
        playerOrder += "9";
    }
    public void Zero()
    {
        playerOrder += "0";
    }

    public void PurchaseFromVendingMachine()
    {
        Debug.Log(playerOrder);
        switch (playerOrder)
        {
            case "3493":
                Debug.Log("Ordered sushi! Right away!");
                Sushi();
                playerOrder = "";
                this.VendingGUI.SetActive(false);
                break;
            case "3289":
                Debug.Log("Banana smoothie! Nice!");
                playerOrder = "";
                this.VendingGUI.SetActive(false);
                break;
            case "69420":
                Debug.Log("Secret menu item!");
                playerOrder = "";
                this.VendingGUI.SetActive(false);
                Pepe();
                break;
            
        }
    }
}
