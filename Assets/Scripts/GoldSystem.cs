using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldSystem : MonoBehaviour
{
    public static GoldSystem Instance;
    public int gold;

    public Text goldText;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else  {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = gold + " $";
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void RemoveGold(int amount)
    {
        gold -= amount;
    }

    public bool HasEnoughGold(int amount)
    {
        return gold >= amount;
    }

    public int GetGold()
    {
        return gold;
    }
}
