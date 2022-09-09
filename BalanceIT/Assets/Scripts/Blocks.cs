using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blocks : MonoBehaviour
{

    [Header("Scripts")]
    GameManager gameManager;

    Rigidbody rb;
    Vector3 lastTapPos;
    Vector3 currentTapPos;

    [SerializeField] private float speed = 0.00005f;
    public static float blockFallSpeed = 1.5f;
    public static bool canPlay = false;

    public bool isTouchPlatform;
    private bool move;
    private bool stopInstantiate;

    float xPos;
    float rot = 0;

    void Start()
    {
        isTouchPlatform = false;
        stopInstantiate = false;
        move = true;
        CameraControl.runOnce = false;
        GameManager.instance.isFell = false;

        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();

        //        rb.isKinematic = true;

    }

    void FixedUpdate()
    {
        Debug.Log("Lastpos " + lastTapPos);

        if (canPlay)
        {
            if (move)
            {
                // rb.velocity = Time.deltaTime * 2 * Vector3.down;
                rb.AddForce(Vector3.down * Time.deltaTime * 2);
                #region ComputerController
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    rot += 45;
                }

                if (Input.GetMouseButton(0))
                {
                    currentTapPos = Input.mousePosition;

                    if (lastTapPos == Vector3.zero)
                    {
                        lastTapPos = currentTapPos;
                    }

                    xPos = currentTapPos.x - lastTapPos.x;
                    lastTapPos = currentTapPos;

                    transform.position += new Vector3(0.008f * Time.deltaTime * xPos, 0, 0);

                }

                if (!Input.GetMouseButton(0))
                {
                    lastTapPos = Vector3.zero;
                }
                #endregion

                #region PhoneController
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Ended)
                    {
                        rot += 45;
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        currentTapPos = Input.mousePosition;

                        if (lastTapPos == Vector3.zero)
                        {
                            lastTapPos = currentTapPos;
                        }

                        xPos = currentTapPos.x - lastTapPos.x;
                        lastTapPos = currentTapPos;

                        transform.position += new Vector3(0.0009f * Time.fixedDeltaTime * xPos, 0, 0);
                    }

                    if (touch.phase != TouchPhase.Moved)
                    {
                        lastTapPos = Vector3.zero;
                    }
                }

                #endregion

                transform.rotation = Quaternion.Euler(0, 0, rot); // Block rotasyon deðiþtirme 

                if (transform.position.x >= 0.053f) // Bu if, blocklarýn ekran dýþýna taþmamasý için
                {
                    transform.position = new Vector3(0.053f, transform.position.y, transform.position.z);
                }
                else if (transform.position.x <= -0.09f)
                {
                    transform.position = new Vector3(-0.09f, transform.position.y, transform.position.z);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        string objectName = other.gameObject.name;
        GameManager.instance.isFell = true;
        switch (objectName)
        {
            case "RemoveFreeze":
                RbConstraint();
                move = false;
                if (!stopInstantiate)
                {
                    Invoke("CallOtherFunct", 1f);
                    stopInstantiate = true;
                }

                GameManager.instance.calculate += 100 / GameManager.instance.objectCount;
                if (GameManager.instance.calculate >= 100)
                {
                    GameManager.instance.calculate = 100;
                    GameManager.instance.percentile.text = "% " + GameManager.instance.calculate.ToString();
                    StartCoroutine(WaitForNextLevelButton());

                }
                GameManager.instance.percentile.text = "% " + GameManager.instance.calculate.ToString();
                break;
            case "Fail":
                GameManager.instance.OpenRestart();
                GameManager.instance.cameraBack = true;
                Time.timeScale = 0;
                break;

        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            isTouchPlatform = true;
        }
    }

    private void RbConstraint()
    {
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

        rb.AddForce(Vector3.down);
        rb.useGravity = true;

    }

    private void CallOtherFunct()
    {
        GameManager.instance.blockList.Remove(GameManager.instance.blockList[0]);
        GameManager.instance.PrepareObjects();
        GameManager.instance.SpawnBlock();
    }

    IEnumerator WaitForNextLevelButton()
    {
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.OpenNextLevelPanel();

    }
}
