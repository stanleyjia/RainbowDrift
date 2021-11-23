using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartScene : MonoBehaviour {
    public void RestartGame () {
        Time.timeScale = 1;
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name); // loads current scene
    }
}