using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour {
    Fade fade;
    Animator transitionAnim;
    public static ChangeScene instance;
    bool done = true;
    AsyncOperation asyncLoad;
    void Start () {
        instance = this;
        //print (GameController.instance.previousScene);
        transitionAnim = GameObject.FindGameObjectWithTag ("Fade").GetComponent<Animator> ();
        switch (GameController.instance.previousScene) {
            case "GameScene":
                switch (SceneManager.GetActiveScene ().name) {
                    case "GameScene":
                        transitionAnim.SetTrigger ("goDown");
                        break;
                    case "CombinedScene":
                        transitionAnim.SetTrigger ("goUp");
                        break;
                }
                break;
            case "TutorialScene":
                switch (SceneManager.GetActiveScene ().name) {
                    case "RegisterScene":
                        transitionAnim.SetTrigger ("goUp");
                        break;
                    case "CombinedScene":
                        transitionAnim.SetTrigger ("goUp");
                        break;
                    case "TutorialScene":
                        transitionAnim.SetTrigger ("goDown");
                        break;
                }
                break;
            case "RegisterScene":
                transitionAnim.SetTrigger ("goDown");
                break;
            case "Persistent":
                break;
            case "CombinedScene":
                switch (SceneManager.GetActiveScene ().name) {
                    case "StoreScene":
                        transitionAnim.SetTrigger ("centerToLeft");
                        break;
                    case "InfoScene":
                        transitionAnim.SetTrigger ("centerToRight");
                        break;
                    case "GameScene":
                        transitionAnim.SetTrigger ("goDown");
                        break;
                    case "RegisterScene":
                        transitionAnim.SetTrigger ("goUp");
                        break;
                    case "TutorialScene":
                        transitionAnim.SetTrigger ("goDown");
                        break;
                    default:
                        break;
                }
                break;
            case "StoreScene":
                transitionAnim.SetTrigger ("centerToRight");
                break;
            case "InfoScene":
                transitionAnim.SetTrigger ("centerToLeft");
                break;
            default:
                break;
        }
    }
    public void changeScene (string scene) {
        Time.timeScale = 1;
        transitionAnim = GameObject.FindGameObjectWithTag ("Fade").GetComponent<Animator> ();
        GameController.instance.previousScene = SceneManager.GetActiveScene ().name;
        // SceneManager.LoadScene (scene, LoadSceneMode.Single);
        if (done) {
            StartCoroutine (LoadAfterFade (scene));
        }
        //  StartCoroutine(LoadAfterFade(0.1f, scene));
    }
    IEnumerator LoadAfterFade (string scene) {
        done = false;
        if (SceneManager.GetActiveScene ().name != "Persistent") {
            switch (scene) {
                case "GameScene":
                    transitionAnim.SetTrigger ("comeDown");
                    break;
                case "CombinedScene":
                    switch (SceneManager.GetActiveScene ().name) {
                        case "GameScene":
                            UserData.instance.combinedIndex = 1;
                            transitionAnim.SetTrigger ("comeUp");
                            break;
                        case "RegisterScene":
                            transitionAnim.SetTrigger ("comeDown");
                            break;
                        case "TutorialScene":
                            transitionAnim.SetTrigger ("comeUp");
                            break;
                    }
                    break;
                case "StoreScene":
                    scene = "CombinedScene";
                    UserData.instance.combinedIndex = 2;
                    transitionAnim.SetTrigger ("comeUp");
                    break;
                case "TutorialScene":
                    if (SceneManager.GetActiveScene ().name != "TutorialScene") {
                        UserData.instance.tutorialStage = 0;
                        //print ("RESET");
                    }
                    transitionAnim.SetTrigger ("comeDown");
                    break;
                case "RegisterScene":
                    transitionAnim.SetTrigger ("comeUp");
                    break;
            }
        }
        asyncLoad = SceneManager.LoadSceneAsync (scene);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        yield return new WaitForSecondsRealtime (0.3f);
        System.GC.Collect ();
        asyncLoad.allowSceneActivation = true;
        done = true;
    }
}