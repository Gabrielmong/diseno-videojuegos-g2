using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Contains the animator controller for scene transitions.")]
    [SerializeField]
    Animator animator;

    [Tooltip("Amount of time in seconds to wait before start scene transition")]
    [SerializeField]
    float transitionTime = 1.0F;

    /// <summary>
    /// Loads next scene from current one.
    /// </summary>
    public void NextScene(int level)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex + level));
    }

    /// <summary>
    /// Load first scene.
    /// </summary>
    public void FirstScene()
    {
        StartCoroutine(LoadLevel(1));
    }

    /// <summary>
    /// Loads an scene in Async mode.
    /// </summary>
    /// <param name="sceneIndex">Scene to be loaded</param>
    /// <returns>Coroutine.s</returns>
    IEnumerator LoadLevel(int sceneIndex)
    {
        animator.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        // Action to be executed in parallel mode.
        // Action to be checked to quit.
        // Action to be executed after parallelism.

    }

}
