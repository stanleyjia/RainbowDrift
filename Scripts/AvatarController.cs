using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class AvatarController : MonoBehaviour {
    public static AvatarController instance;
    public Image avatarImg;
    List<Image> pics = new List<Image> ();
    public Image one;
    public Image two;
    public Image three;
    public Image four;
    public Image five;
    public Image six;
    public Image seven;
    public Image eight;
    bool SelectScreenOn = true;
    int avatarInd = 0;
    public GameObject SelectAvatarModal;
    public GameObject ChangeAvatarText;
    public Animator noPress;
    public Dictionary<string, string> avatarUrls = new Dictionary<string, string> ();
    Texture2D texture;
    public string currentURL;
    public int currentIndex;
    public Animator SelectAvatarAnimator;
    public Dictionary<string, string> avatarPaths = new Dictionary<string, string> ();
    bool chal7;
    List<string> tagsToDelete = new List<string> ();
    List<string> tagsToAdd = new List<string> ();
    string avatarKey;
    bool containsAvatarUrl;
    bool addedOnce = true;
    bool canRun = true;
    public Animator error;
    public Animator ErrorNoPress;
    int indexToUpdate = 0;
    // Use this for initialization
    private void Start () {
        avatarUrls.Add ("fox", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/fox.png");
        avatarUrls.Add ("panda", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/panda.png");
        avatarUrls.Add ("walrus", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/walrus.png");
        avatarUrls.Add ("penguin", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/penguin.png");
        avatarUrls.Add ("tiger", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/tiger.png");
        avatarUrls.Add ("pig", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/pig.png");
        avatarUrls.Add ("cobra", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/cobra.png");
        avatarUrls.Add ("rhino", "https://res.cloudinary.com/moosepark/image/upload/v1534403658/rhino.png");
        //Avatar Paths in local project
        avatarPaths.Add ("fox", "avatars/fox");
        avatarPaths.Add ("panda", "avatars/panda");
        avatarPaths.Add ("walrus", "avatars/walrus");
        avatarPaths.Add ("penguin", "avatars/penguin");
        avatarPaths.Add ("tiger", "avatars/tiger");
        avatarPaths.Add ("pig", "avatars/pig");
        avatarPaths.Add ("cobra", "avatars/cobra");
        avatarPaths.Add ("rhino", "avatars/rhino");
        pics.Add (one);
        pics.Add (two);
        pics.Add (three);
        pics.Add (four);
        pics.Add (five);
        pics.Add (six);
        pics.Add (seven);
        pics.Add (eight);
        avatarInd = DataEntry.instance.AvatarInd;
        SetAvatarPicture (ImagesContainer.instance.GetName (avatarInd));
        SetChoosePics (avatarInd);
        ChangeAvatarText.SetActive (true);
        noPress.gameObject.SetActive (false);
        SelectScreenOn = false;
        chal7 = ChallengesController.ChallengeDone (7);
    }
    private void Awake () {
        instance = this;
    }
    Texture2D GetAvatarFromLocalProject (string key) {
        return (Resources.Load<Texture2D> (avatarPaths[key]));
        // return (Texture2D) AssetDatabase.LoadAssetAtPath (avatarPaths[key], typeof (Texture2D));
    }
    // Update is called once per frame
    void Update () { }
    IEnumerator SetCanRun () {
        yield return new WaitForSeconds (2);
        canRun = true;
    }
    public void SetChoosePics (int index) {
        //print ("choose pic run");
        currentIndex = index;
        for (int i = 0; i < pics.Count; i++) {
            string picName = ImagesContainer.instance.GetName (i);
            if (i == index) {
                pics[i].transform.Find ("stroke").gameObject.SetActive (true);
            } else {
                pics[i].transform.Find ("stroke").gameObject.SetActive (false);
            }
            texture = GetAvatarFromLocalProject (picName);
            pics[i].sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
        }
    }
    IEnumerator ErrorAnim () {
        ErrorNoPress.gameObject.SetActive (true);
        error.SetTrigger ("Show");
        if (DataEntry.instance.hapticOn) {
            iOSHapticFeedback.Instance.Trigger ((iOSHapticFeedback.iOSFeedbackType) 6);
        }
        yield return new WaitForSeconds (1f);
        error.SetTrigger ("Hide");
        ErrorNoPress.SetTrigger ("FadeOut");
        yield return new WaitForSeconds (0.5f);
    }
    public void SetAvatarPicture (string key) {
        currentURL = avatarUrls[key];
        texture = GetAvatarFromLocalProject (key);
        avatarImg.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0, 0));
    }
    public void AvatarClicked () {
        if (SelectScreenOn == false) {
            ChangeAvatarText.SetActive (false);
            //  SelectAvatarModal.SetActive (true);
            SelectAvatarAnimator.SetTrigger ("Show");
            noPress.gameObject.SetActive (true);
            SelectScreenOn = true;
        } else {
            ChangeAvatarText.SetActive (true);
            // SelectAvatarModal.SetActive (false);
            SelectAvatarAnimator.SetTrigger ("Hide");
            noPress.SetTrigger ("FadeOut");
            SelectScreenOn = false;
            UpdateAvatarTags (indexToUpdate);
        }
    }
    public void ChooseAvatar (int ind) {
        if (ind != currentIndex) {
            if (chal7) {
                ChallengesController.CompleteChallenge (7);
                chal7 = false;
            }
            for (int i = 0; i < pics.Count; i++) {
                pics[i].transform.Find ("stroke").gameObject.SetActive ((i == ind));
            }
            SetAvatarPicture (ImagesContainer.instance.GetName (ind));
            InfoController.instance.SetAvatarUrl (ind, avatarUrls[ImagesContainer.instance.GetName (ind)]);
            indexToUpdate = ind;
            //UpdateAvatarTags (ind);
            currentIndex = ind;
        }
    }
    public void UpdateAvatarTags (int ind) {
        avatarKey = ImagesContainer.instance.GetName (ind);
        addedOnce = false;
        tagsToDelete.Clear ();
        tagsToAdd.Clear ();
        containsAvatarUrl = false;
        for (int i = 0; i < UserData.instance.tags.Count; i++) {
            if (UserData.instance.tags[i].Contains ("avatar:") == true) {
                containsAvatarUrl = true;
                if (UserData.instance.tags[i].Replace ("avatar:", "") != avatarKey) {
                    tagsToDelete.Add (UserData.instance.tags[i]);
                    if (addedOnce == false) {
                        tagsToAdd.Add ("avatar:" + avatarKey);
                        addedOnce = true;
                    }
                }
            }
        }
        if (containsAvatarUrl == false) {
            tagsToAdd.Add ("avatar:" + avatarKey);
        }
        if (tagsToDelete.Count > 0) {
            StartCoroutine (DeleteTags ());
        } else if (tagsToAdd.Count > 0) {
            StartCoroutine (AddTags ());
        }
    }
    IEnumerator DeleteTags () {
        //print ("Deleting " + tagsToDelete.Count + " tags");
        for (int i = 0; i < tagsToDelete.Count; i++) {
            //print (i);
            //print ("Deleting " + tagsToDelete[i]);
            PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "RemovePlayerTag",
                    FunctionParameter = new Dictionary<string, string> {
                        {
                            "tagName",
                            tagsToDelete[i]
                        }
                    }
            }, (ExecuteCloudScriptResult res) => {
                for (int k = 0; k < res.Logs.Count; k++) {
                    print (res.Logs[k].Message);
                }
            }, CallFailure);
            tagsToDelete.RemoveAt (i);
            yield return new WaitForSeconds (1f);
        }
        //StartCoroutine (SetCanRun ());
        if (tagsToAdd.Count > 0) {
            StartCoroutine (AddTags ());
        } else {
            canRun = true;
        }
    }
    IEnumerator AddTags () {
        print ("Adding " + tagsToAdd.Count + " tags");
        //print (tagsToAdd.Count);
        for (int i = 0; i < tagsToAdd.Count; i++) {
            PlayFabClientAPI.ExecuteCloudScript (new ExecuteCloudScriptRequest {
                FunctionName = "AddPlayerTag",
                    FunctionParameter = new Dictionary<string, string> {
                        {
                            "tagName",
                            tagsToAdd[i]
                        }
                    }
            }, (ExecuteCloudScriptResult res) => {
                if (Debug.isDebugBuild) {
                    for (int k = 0; k < res.Logs.Count; k++) {
                        print (res.Logs[k].Message);
                    }
                }
            }, CallFailure);
            tagsToAdd.RemoveAt (i);
            yield return new WaitForSeconds (1f);
        }
        canRun = true;
    }
    void CallFailure (PlayFabError error) {
        if (error.Error == PlayFabErrorCode.ServiceUnavailable) {
            //no connection
            ConnectionController.instance.noConnectionDetected ();
        } else {
            if (Debug.isDebugBuild) {
                print (error);
            }
        }
    }
}