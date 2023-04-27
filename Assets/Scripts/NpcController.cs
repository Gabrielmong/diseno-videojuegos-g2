using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI vendorTitle;

    [SerializeField]
    TextMeshProUGUI vendorText;

    [SerializeField]
    GameObject vendorPanel;

    [SerializeField]
    Button vendorButton;

    [SerializeField]
    private string vendorName;

    [SerializeField]
    private string itemToSell;

    [SerializeField]
    private int itemCost;

    [SerializeField]
    private int itemsAvailable;

    private float buyDelay = 0.5f;
    
    private float nextBuy = 0.0f;

    private bool inBuyRange = false;


    void Start()
    {


    }

    void Update()
    {
        if (inBuyRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time > nextBuy)
            {
                nextBuy = Time.time + buyDelay;
                BuyItem();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseVendor();
            }
        }
    }

    public void OpenVendor()
    {
        vendorPanel.SetActive(true);

        inBuyRange = true;
        vendorTitle.text = vendorName;
        vendorText.text = "Hello, I'm " + vendorName + ". I have " + itemsAvailable + " " + itemToSell + "s available for " + itemCost + " gold each.";

        vendorButton.onClick.RemoveAllListeners();

        vendorButton.onClick.AddListener(() => BuyItem());

    }

    public void CloseVendor()
    {
        vendorPanel.SetActive(false);

        inBuyRange = false;

        vendorButton.onClick.RemoveAllListeners();
    }

    public void BuyItem()
    {
        if (itemsAvailable > 0)
        {
            if (GoldSystem.Instance.HasEnoughGold(itemCost))
            {
                GoldSystem.Instance.RemoveGold(itemCost);
                itemsAvailable--;
                vendorText.text = "Hello, I'm " + vendorName + ". I have " + itemsAvailable + " " + itemToSell + " available for " + itemCost + " gold each.";

                switch (itemToSell)
                {
                    case "Flowers":
                        GameGrid.Instance.AddFlowers(1);
                        break;
                    case "Grass":
                        GameGrid.Instance.AddGrass(1);
                        break;
                    case "Health Potions":
                        PlayerController.Instance.AddHealthPotions(1);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                vendorText.text = "You don't have enough gold to buy this item.";
            }
        }
        else
        {
            vendorText.text = "I'm sorry, I'm out of stock.";
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            OpenVendor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloseVendor();
        }
    }
}
