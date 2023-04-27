using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject sceneManagerObject;

    public void OnPressEmpezar()
    {
        sceneManagerObject.GetComponent<SceneController>().LoadScene(1);
    }
}
