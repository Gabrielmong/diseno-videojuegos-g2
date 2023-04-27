using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance;

    [SerializeField]
    GameObject player; // to detect if tile is in range of player (3 tiles)
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

    [SerializeField]
    private int availableFlowers;

    [SerializeField]
    private int availableGrass;

    [SerializeField]
    private TextMeshProUGUI seedsText;

    [SerializeField]
    private TextMeshProUGUI grassText;


    void Awake()
    {
        Instance = this;
    }

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
        
        seedsText.text = availableFlowers.ToString();
        grassText.text = availableGrass.ToString();
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
                if (creatingFields == true)
                {
                    if (checkIfInRange()) { return; }

                    if (_Hit.transform.tag == "grid"
                        && (_Hit.transform.gameObject.name == "Flowers(Clone)"
                        || _Hit.transform.gameObject.name == "Grass(Clone)"))
                    {
                        
                        hitted = _Hit.transform.gameObject;
                        Instantiate(field, hitted.transform.position, Quaternion.identity);
                        Destroy(hitted);
                    }
                    else if (_Hit.transform.tag == "grid"
                        && _Hit.transform.gameObject.name == "Field(Clone)")
                    {
                        hitted = _Hit.transform.gameObject;
                        if (availableGrass > 0)
                        {
                            Instantiate(grass, hitted.transform.position, Quaternion.identity);
                            availableGrass--;
                            grassText.text = availableGrass.ToString();
                            Destroy(hitted);
                        }

                    }
                }

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _Hit))
            {
                if (checkIfInRange()) { return; }

                if (_Hit.transform.tag == "grid"
                    && _Hit.transform.gameObject.name == "Field(Clone)") // NEW
                {
                    hitted = _Hit.transform.gameObject;
                    if (availableFlowers > 0)
                    {
                        Instantiate(flowers, hitted.transform.position, Quaternion.identity);
                        availableFlowers--;
                        seedsText.text = availableFlowers.ToString();
                        Destroy(hitted);
                    }
                }
            }
        }
    }

    public bool checkIfInRange()
    {
        return Vector3.Distance(player.transform.position, _Hit.transform.position) > 3;
    }

    public void createFields()
    {
        creatingFields = true;
    }

    public void returnToNormality()
    {
        creatingFields = false;
    }

    public void AddFlowers(int amount)
    {
        availableFlowers += amount;
        seedsText.text = availableFlowers.ToString();
    }

    public void AddGrass(int amount)
    {
        availableGrass += amount;
        grassText.text = availableGrass.ToString();
    }
}