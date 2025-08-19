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
    public float laneOffset;  // distance between lanes
    private float centerX;            // middle lane position
    [HideInInspector]
    public bool SwipeLeft;
    public bool SwipeRight;
    public bool SwipeUp;
    public bool SwipeDown;
    public float XValue;

    private CharacterController m_char;

    // For swipe detection
    private Vector2 startTouchPos;
    private bool isDragging = false;
    public float swipeThreshold = 50f; // Pixels to trigger swipe

    private Animator animator;
    private float x;
    public float SpeedDodge;

    public float JumpPower = 7f;
    private float y;
    public bool InJump;
    public bool InRoll;
    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position = new Vector3(-4.2f, 3f, 45f);
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
        SwipeDown = false;

        // Keyboard input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            SwipeLeft = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            SwipeRight = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            SwipeUp = true;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            SwipeDown = true;



        // Mouse/touch swipe input
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

        // Movement logic
        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX - laneOffset; ;
                m_Side = SIDE.Left;
                animator.Play("dodgetLeft");
            }
            else if (m_Side == SIDE.Right)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;
                animator.Play("dodgetLeft");
            }
        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX + laneOffset;
                m_Side = SIDE.Right;
                animator.Play("dodgetRight");
            }
            else if (m_Side == SIDE.Left)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;
                animator.Play("dodgetRight");
            }
        }
        Vector3 moveVector = new Vector3(x - transform.position.x,y*Time.deltaTime,0);
        x = Mathf.Lerp(x, NewXpos, Time.deltaTime * SpeedDodge);
        m_char.Move(moveVector);
        Jump();
        Swim();
    }

    public void Jump()
    {
        if (m_char.isGrounded)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {
                animator.Play("Landing");
                InJump = false;
            }


            if(SwipeUp)
            {
                y = JumpPower;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
                InJump = true;
                    
            }
        }else
        {
            y -= JumpPower*2 *Time.deltaTime;
            if (m_char.velocity.y<-0.1f)
            animator.Play("Falling");
        }
    }

    public void Swim()
    {
        if (SwipeDown && !InRoll)  
        {
            InRoll = true; 
            animator.CrossFadeInFixedTime("Swim", 0.1f);
            Vector3 targetPos = new Vector3(transform.position.x, -10f, transform.position.z);
            Invoke("EndSwim", 1.0f); 
        }
    }

    void EndSwim()
    {
        InRoll = false;
    }

}
