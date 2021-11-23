using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour {
    // PlayerController player;
    //TutorialCar tutorialPlayer;
    GameObject player;
    private Vector3 offset;
    private Vector3 playerDirection;
    //s
    private Vector3 startPosition;
    void Start () {
        player = GameObject.FindWithTag ("Player");
        //rotation 50 degrees
        offset = new Vector3 (0, -12, -10);
        // Quaternion rotR = Quaternion.AngleAxis(-55, Vector3.right);
        //transform.rotation = transform.rotation * rotR;
        Quaternion rotR = Quaternion.AngleAxis (-55, Vector3.right);
        Quaternion rotU = player.transform.rotation * rotR;
        transform.rotation = rotU;
        offset = new Vector3 (-10f * player.transform.up.x, -10f * player.transform.up.y, -10);
        transform.position = player.transform.position + offset;
    }
    void FixedUpdate () {
        // if (SceneManager.GetActiveScene().name == "GameScene")
        // {
        if (CarVariables.instance.gameOn == true) {
            Quaternion rotR = Quaternion.AngleAxis (-55, Vector3.right);
            Quaternion rotU = player.transform.rotation * rotR;
            transform.rotation = Quaternion.Slerp (transform.rotation, rotU, 20 * Time.deltaTime);
            offset = new Vector3 (-8f * player.transform.up.x, -8f * player.transform.up.y, -10);
            transform.position = player.transform.position + offset;
        } else {
            playerDirection = 0.6f * player.transform.up.normalized;
            transform.position = player.transform.position + offset + playerDirection;
            //}
        }
    }
}