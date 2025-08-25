using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

[System.Serializable]
public enum SIDE { Left, Mid, Right }

public class Character : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;
    public float NewXpos;
    public float laneOffset;
    private float centerX;
    [HideInInspector]
    public bool SwipeLeft;
    public bool SwipeRight;
    public bool SwipeUp;
    //public bool SwipeDown;
    public float XValue;

    private CharacterController m_char;
    public CameraIntro cameraIntro;


    private Vector2 startTouchPos;
    private bool isDragging = false;
    public float swipeThreshold = 50f;

    private Animator animator;
    private float x;
    public float SpeedDodge;

    public float JumpPower = 7f;
    private float y;
    public bool InJump;
    public bool InRoll;
    public GameObject surfBoad;

    private Vector3 originalPlayerPos;
    private float originalCameraY;

    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position = new Vector3(-4.2f, 2f, 45f);
        animator = GetComponent<Animator>();

        centerX = transform.position.x;

        m_Side = SIDE.Mid;
        NewXpos = centerX;
    }

    void Update()
    {

        SwipeLeft = false;
        SwipeRight = false;
        SwipeUp = false;
        //SwipeDown = false;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            SwipeLeft = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            SwipeRight = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            SwipeUp = true;
        /*if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            SwipeDown = true;
*/


        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                Vector2 endTouchPos = Input.mousePosition;
                Vector2 swipeDelta = endTouchPos - startTouchPos;

                if (Mathf.Abs(swipeDelta.x) > swipeThreshold)
                {
                    if (swipeDelta.x > 0)
                        SwipeRight = true;
                    else
                        SwipeLeft = true;
                }
            }
            isDragging = false;
        }

        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX - laneOffset; ;
                m_Side = SIDE.Left;
                if (InRoll)
                    animator.Play("Swim");
                else
                    animator.Play("dodgetLeft");
            }
            else if (m_Side == SIDE.Right)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;

                if (InRoll)
                    animator.Play("Swim");
                else
                    animator.Play("dodgetLeft");
            }
        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX + laneOffset;
                m_Side = SIDE.Right;

                if (InRoll)
                    animator.Play("Swim");
                else
                    animator.Play("dodgetRight");
            }
            else if (m_Side == SIDE.Left)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;

                if (InRoll)
                    animator.Play("Swim");
                else
                    animator.Play("dodgetRight");
            }
        }

        /*Vector3 moveVector = new Vector3(x - transform.position.x,y*Time.deltaTime,0);
        x = Mathf.Lerp(x, NewXpos, Time.deltaTime * SpeedDodge);
        m_char.Move(moveVector);
        Jump();*/
        //Swim();


        Vector3 moveVector;

        if (InRoll) // swimming mode
        {
            // force constant Y while swimming
            moveVector = new Vector3(x - transform.position.x, 0, 0);
            transform.position = new Vector3(transform.position.x, -16.04f, transform.position.z);
        }
        else
        {
            // normal movement (with jump/gravity)
            moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 0);
            Jump();
        }

        x = Mathf.Lerp(x, NewXpos, Time.deltaTime * SpeedDodge);
        m_char.Move(moveVector);


    }

    public void Jump()
    {
        if (m_char.isGrounded)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {
                animator.Play("Landing");
                InJump = false;
            }

            if (SwipeUp)
            {
                y = JumpPower;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
                InJump = true;
                InRoll = false;


                if (surfBoad != null) surfBoad.SetActive(true);
            }
        }
        else
        {
            y -= JumpPower * 2 * Time.deltaTime;
            if (m_char.velocity.y < -0.1f)
                animator.Play("Falling");
        }
    }

    public void Swim()
    {
        if (!InRoll)
        {
            InRoll = true;
            animator.CrossFadeInFixedTime("Swim", 0.1f);

            // Store original positions
            originalPlayerPos = transform.position;
            if (cameraIntro != null)
                originalCameraY = cameraIntro.targetPosition.position.y;

            // Snap player to swim depth
            Vector3 pos = transform.position;
            pos.y = -16.04f;
            transform.position = pos;

            //y = 0;

            if (surfBoad != null) surfBoad.SetActive(false);

            // Move camera target when swimming
            if (cameraIntro != null)
            {
                cameraIntro.SetTargetY(-5f);
            }

            // Start 5-second swim
            StartCoroutine(SwimDuration(5f));
        }
    }

    private IEnumerator SwimDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // End swim
        InRoll = false;

        // Reset player position
        transform.position = originalPlayerPos;

        // Reset camera target
        if (cameraIntro != null)
        {
            cameraIntro.SetTargetY(originalCameraY);
        }

        if (surfBoad != null)
            surfBoad.SetActive(true);

        animator.Play("Landing"); // or another default animation
    }



    void EndSwim()
    {
        //InRoll = false;


        // if (surfBoad != null) surfBoad.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OxygenTank"))
        {
            Swim();
            Destroy(other.gameObject);
        }
    }
}
