using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestController : MonoBehaviour
{
    [Header("Forest Objects")]
    [SerializeField]
    private GameObject[] trees;

    [SerializeField]
    private Material missingMaterial;

    [SerializeField]
    private GameObject[] bushes;

    [SerializeField]
    private GameObject[] rocks;

    [SerializeField]
    private GameObject[] flowers;

    [Header("Tree Spawn Points")]
    [SerializeField]
    Transform topRight;

    [SerializeField]
    Transform topLeft;

    [SerializeField]
    Transform bottomRight;

    [SerializeField]
    Transform bottomLeft;

    [Header("Asset Count")]
    [SerializeField]
    private int treeCount;

    [SerializeField]
    private int bushCount;

    [SerializeField]
    private int rockCount;

    [SerializeField]
    private int flowerCount;

    private void Awake()
    {
        SpawnFlowers();
        SpawnBushes();
        SpawnRocks();
        SpawnTrees();
    }

    private string[] missingMaterials = {
        "rpgpp_lt_tree_pine_01",
        "rpgpp_lt_bush_01",
        "rpgpp_lt_flower_01",
        "rpgpp_lt_flower_02",
        "rpgpp_lt_flower_03",
        "rpgpp_lt_plant_01",
        "rpgpp_lt_plant_02",
        "rpgpp_lt_rocks_tiny_01",
    };

    public void SpawnTrees()
    {
        while (treeCount > 0)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(Random.Range(topLeft.position.x, topRight.position.x), 100, Random.Range(topLeft.position.z, bottomLeft.position.z)), Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000))
            {

                // if place where it hit is tagged as ground, then continue
                if (hit.collider.gameObject.tag != "Ground")
                {
                    continue;
                }


                var tree = Instantiate(trees[Random.Range(0, trees.Length)], hit.point, Quaternion.identity);

                tree.transform.Rotate(0, Random.Range(0, 360), 0);

                tree.AddComponent<MeshCollider>();


                foreach (var missingMaterial in missingMaterials)
                {
                    if (tree.name.Contains(missingMaterial))
                    {
                        tree.GetComponent<MeshRenderer>().material = this.missingMaterial;
                    }
                }

                treeCount -= 1;
            }

        }
    }

    public void SpawnBushes()
    {
        while (bushCount > 0)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(Random.Range(topLeft.position.x, topRight.position.x), 100, Random.Range(topLeft.position.z, bottomLeft.position.z)), Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.tag != "Ground")
                {
                    continue;
                }

                var bush = Instantiate(bushes[Random.Range(0, bushes.Length)], hit.point, Quaternion.identity);

                bush.transform.Rotate(0, Random.Range(0, 360), 0);

                bush.AddComponent<MeshCollider>();

                foreach (var missingMaterial in missingMaterials)
                {
                    if (bush.name.Contains(missingMaterial))
                    {
                        bush.GetComponent<MeshRenderer>().material = this.missingMaterial;
                    }
                }

                bushCount -= 1;
            }
        }
    }

    public void SpawnRocks()
    {
        for (int i = 0; i < rockCount; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(Random.Range(topLeft.position.x, topRight.position.x), 100, Random.Range(topLeft.position.z, bottomLeft.position.z)), Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.tag != "Ground")
                {
                    continue;
                }

                var rock = Instantiate(rocks[Random.Range(0, rocks.Length)], hit.point, Quaternion.identity);

                rock.transform.Rotate(0, Random.Range(0, 360), 0);

                rock.AddComponent<MeshCollider>();

                foreach (var missingMaterial in missingMaterials)
                {
                    if (rock.name.Contains(missingMaterial))
                    {
                        rock.GetComponent<MeshRenderer>().material = this.missingMaterial;
                    }
                }

                rockCount -= 1;
            }
        }
    }

    public void SpawnFlowers()
    {
        for (int i = 0; i < flowerCount; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(Random.Range(topLeft.position.x, topRight.position.x), 100, Random.Range(topLeft.position.z, bottomLeft.position.z)), Vector3.down);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.tag != "Ground")
                {
                    continue;
                }

                var flower = Instantiate(flowers[Random.Range(0, flowers.Length)], hit.point, Quaternion.identity);

                flower.transform.Rotate(0, Random.Range(0, 360), 0);

                foreach (var missingMaterial in missingMaterials)
                {
                    if (flower.name.Contains(missingMaterial))
                    {
                        flower.GetComponent<MeshRenderer>().material = this.missingMaterial;
                    }
                }

                flowerCount -= 1;
            }
        }
    }

}

