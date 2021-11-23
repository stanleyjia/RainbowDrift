using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
#endif
#if (UNITY_IOS|| UNITY_ANDROID) && !UNITY_EDITOR
        public class DragHandler : MonoBehaviour {
#endif
            private Vector3 startPosition;
            float returnSpeed = 0.05f;
            public bool dragging = false;
            //bool startedDrag = false;
            float startalpha;
            float startcolor;
            Image image;
            public float turn;
            public static DragHandler instance;
            bool tutorial;
            bool firstTime = true;
            bool firstTime2 = true;
            float sensitivity;
            Vector3 shiftedPosition;
            public RectTransform canvas;
            public Animator DragInstructionsText;
            public Animator DragInstructionsLeftArrow;
            public Animator DragInstructionsRightArrow;
            //public GameObject pauseButton;
            void Start () {
                instance = this;
                transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (canvas.rect.width * 3, canvas.rect.width);
                startPosition = transform.position;
                image = GetComponent<Image> ();
                startalpha = image.color.a;
                Cursor.lockState = CursorLockMode.Confined;
                if (SceneManager.GetActiveScene ().name == "TutorialScene") {
                    tutorial = true;
                    image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
                    //StartCoroutine(FadeImage(0));
                } else {
                    tutorial = false;
                    //pauseButton.SetActive (false);
                }
                //  carScreenPosition = cam.WorldToScreenPoint (car.transform.position);
                //transform.position = carScreenPosition;
            }
            void Update () {
                if (tutorial) {
                    if (CarVariables.instance.gameOn == false) {
                        if (firstTime) {
                            StartCoroutine (FadeImage ());
                            firstTime = false;
                        }
                    } else {
                        if (AIController.instance.AIControllerOn == false) {
                            image.raycastTarget = true;
                            image.color = new Color (image.color.r, image.color.g, image.color.b, 0f);
                        } else {
                            image.raycastTarget = false;
                            image.color = new Color (image.color.r, image.color.g, image.color.b, 0f);
                        }
                    }
                } else {
                    if (CarVariables.instance.gameOn == false) {
                        if (firstTime) {
                            StartCoroutine (FadeImage ());
                            DragInstructionsText.SetTrigger ("Hide");
                            DragInstructionsLeftArrow.SetTrigger ("Hide");
                            DragInstructionsRightArrow.SetTrigger ("Hide");
                            firstTime = false;
                        }
                    }
                }
#if (UNITY_IOS|| UNITY_ANDROID) && !UNITY_EDITOR
                if (Input.touchCount > 0) {
                    if (Input.touches[0].phase == TouchPhase.Began) {
                        dragging = true;
                        if (tutorial) {
                            if (TutorialController.instance.stage == 5) {
                                TutorialController.instance.stage = 6;
                            }
                        } else {
                            if (firstTime2) {
                                firstTime2 = false;
                                DragInstructionsText.SetTrigger ("Hide");
                                DragInstructionsLeftArrow.SetTrigger ("Hide");
                                DragInstructionsRightArrow.SetTrigger ("Hide");
                                //pauseButton.SetActive (true);
                            }
                        }
                        /* if (startedDrag == false) {
                             startedDrag = true;
                         }*/
                    } else if ((Input.touches[0].phase == TouchPhase.Moved) || (Input.touches[0].phase == TouchPhase.Stationary)) {
                        dragging = true;
                        if (tutorial) {
                            if (AIController.instance.AIControllerOn == false) {
                                /*  if (Input.touches[0].position.x < 0) {
                                     transform.position = new Vector3 (0, transform.position.y, transform.position.z);
                                 } else if (Input.touches[0].position.x > Screen.width) {
                                     transform.position = new Vector3 (Screen.width, transform.position.y, transform.position.z);
                                 } else {
                                     transform.position = Input.touches[0].position;
                                 }*/
                                if (Input.touches[0].position.y < Screen.height / 2f) {
                                    transform.position = Input.touches[0].position;
                                } else {
                                    // transform.position = new Vector3 (Input.touches[0].position.x, transform.position.y, transform.position.z);
                                }
                            }
                            if (dragging == false) {
                                dragging = true;
                            }
                        } else {
                            /*  if (Input.touches[0].position.x < 0) {
                                 transform.position = new Vector3 (0, transform.position.y, transform.position.z);
                             } else if (Input.touches[0].position.x > Screen.width) {
                                 transform.position = new Vector3 (Screen.width, transform.position.y, transform.position.z);
                             } else {
                                 transform.position = Input.touches[0].position;
                             }*/
                            if (Input.touches[0].position.y < Screen.height / 2f) {
                                transform.position = Input.touches[0].position;
                            } else {
                                //transform.position = new Vector3 (Input.touches[0].position.x, transform.position.y, transform.position.z);
                            }
                            if (dragging == false) {
                                dragging = true;
                            }
                        }
                    } else if ((Input.touches[0].phase == TouchPhase.Ended) || (Input.touches[0].phase == TouchPhase.Canceled)) {
                        dragging = false;
                        StartCoroutine (ReturnToPosition ());
                    }
                }
#endif
                //turn = -(transform.position.x - (Screen.width / 2)) / (Screen.width / 2f);
            }
#if UNITY_EDITOR
            public void OnBeginDrag (PointerEventData eventData) {
                dragging = true;
                if (tutorial) {
                    if (TutorialController.instance.stage == 5) {
                        TutorialController.instance.stage = 6;
                    }
                } else {
                    if (firstTime2) {
                        firstTime2 = false;
                        DragInstructionsText.SetTrigger ("Hide");
                        DragInstructionsLeftArrow.SetTrigger ("Hide");
                        DragInstructionsRightArrow.SetTrigger ("Hide");
                    }
                }
                /* if (startedDrag == false) {
                     startedDrag = true;
                 }*/
            }
            public void OnDrag (PointerEventData eventData) {
                dragging = true;
                if (tutorial) {
                    if (AIController.instance.AIControllerOn == false) {
                        /* if (Input.mousePosition.x < 0) {
                            transform.position = new Vector3 (0, transform.position.y, transform.position.z);
                        } else if (Input.mousePosition.x > Screen.width) {
                            transform.position = new Vector3 (Screen.width, transform.position.y, transform.position.z);
                        } else {
                            transform.position = Input.mousePosition;
                        }*/
                        if (Input.mousePosition.y < Screen.height / 2f) {
                            transform.position = Input.mousePosition;
                        }
                        if (dragging == false) {
                            dragging = true;
                        }
                    }
                } else {
                    /*  if (Input.mousePosition.x < 0) {
                         transform.position = new Vector3 (0, transform.position.y, transform.position.z);
                     } else if (Input.mousePosition.x > Screen.width) {
                         transform.position = new Vector3 (Screen.width, transform.position.y, transform.position.z);
                     } else {
                         transform.position = Input.mousePosition;
                     }*/
                    if (Input.mousePosition.y < Screen.height / 2f) {
                        transform.position = Input.mousePosition;
                    }
                    if (dragging == false) {
                        dragging = true;
                    }
                }
            }
            public void OnEndDrag (PointerEventData eventData) {
                dragging = false;
                StartCoroutine (ReturnToPosition ());
            }
#endif
            private void FixedUpdate () {
                //Between -1 and 1, right is negative
                // turn = -(transform.position.x - (Screen.width / 2)) / (Screen.width / 2f);
            }
            private void LateUpdate () {
                turn = -(transform.position.x - (Screen.width / 2)) / (Screen.width / 2f);
            }
            public void ShowJoystick () {
                image.color = new Color (image.color.r, image.color.g, image.color.b, startalpha);
            }
            IEnumerator ReturnToPosition () {
                while ((dragging == false) && (transform.position != startPosition)) {
                    transform.position = transform.position + ((startPosition - transform.position) * returnSpeed);
                    if (Vector3.Distance (transform.position, startPosition) < 1f) {
                        transform.position = startPosition;
                    }
                    yield return new WaitForSeconds (0.01f);
                }
            }
            IEnumerator FadeImage (float alpha = 0) {
                startcolor = image.color.a;
                for (float i = 0; i <= 1; i += Time.deltaTime / 1.0f) {
                    image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.Lerp (startcolor, alpha, i));
                    yield return null;
                }
                image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
                //gameObject.SetActive(false);
            }
        }