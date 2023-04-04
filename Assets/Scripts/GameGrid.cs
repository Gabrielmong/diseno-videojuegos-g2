using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public int columnLength, rowLength;

    public float x_Space, z_Space;

    public GameObject grass;

    [SerializeField]
    private GameObject flowers;

    [SerializeField]
    private int flowerChance = 10;

    public GameObject[] currentGrid;

    public bool gotGrid;

    public GameObject hitted;

    public GameObject field;

    private RaycastHit _Hit;

    public bool creatingFields;

    // NEW
    public GameObject goldSystem;
    public int fieldPrice;
    public int plantPrice;
    // NEW END

    void Start()
    {
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            int randInt = Random.Range(0, 100);

            if (randInt < flowerChance)
            {
                Instantiate(flowers, new Vector3(x_Space + (x_Space * (i % columnLength)), 0, z_Space + (z_Space * (i / columnLength))), Quaternion.identity);
            }
            else
            {
                Instantiate(grass, new Vector3(x_Space + (x_Space * (i % columnLength)), 0, z_Space + (z_Space * (i / columnLength))), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        if (gotGrid == false)
        {
            currentGrid = GameObject.FindGameObjectsWithTag("grid");
            gotGrid = true;
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _Hit))
            {

                Debug.Log(_Hit.transform.gameObject.name);
                if (creatingFields == true)
                {
                    if (_Hit.transform.tag == "grid"
                        && (_Hit.transform.gameObject.name == "Flowers(Clone)"
                        || _Hit.transform.gameObject.name == "Grass(Clone)"))
                    {
                        hitted = _Hit.transform.gameObject;
                        Instantiate(field, hitted.transform.position, Quaternion.identity);
                        Destroy(hitted);
                    }
                    else if (_Hit.transform.tag == "grid"
                        && _Hit.transform.gameObject.name == "Field(Clone)"
                        && goldSystem.GetComponent<GoldSystem>().gold >= fieldPrice) // NEW
                    {
                        Debug.Log("Field");
                        hitted = _Hit.transform.gameObject;
                        Instantiate(grass, hitted.transform.position, Quaternion.identity);
                        Destroy(hitted);

                        // NEW
                        goldSystem.GetComponent<GoldSystem>().gold -= fieldPrice;
                        // NEW END
                    }
                }

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _Hit))
            {
                if (_Hit.transform.tag == "grid"
                    && _Hit.transform.gameObject.name == "Field(Clone)"
                    && goldSystem.GetComponent<GoldSystem>().gold >= plantPrice) // NEW
                {
                    hitted = _Hit.transform.gameObject;
                    Instantiate(flowers, hitted.transform.position, Quaternion.identity);
                    Destroy(hitted);

                    // NEW
                    goldSystem.GetComponent<GoldSystem>().gold -= plantPrice;
                    // NEW END
                }
            }
        }
    }

    public void createFields()
    {
        creatingFields = true;
    }

    public void returnToNormality()
    {
        creatingFields = false;
    }
}