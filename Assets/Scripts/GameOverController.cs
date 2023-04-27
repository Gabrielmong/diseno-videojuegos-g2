using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    GameObject sceneManagerObject;

    public void OnPressRetry()
    {
        sceneManagerObject.GetComponent<SceneController>().LoadScene(0);
    }
}
