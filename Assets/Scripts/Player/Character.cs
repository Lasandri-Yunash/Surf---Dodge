using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SIDE { Left, Mid, Right }

public class Character : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;
    public float NewXpos;
    public float laneOffset;  // distance between lanes
    private float centerX;    // middle lane position

    [HideInInspector] public bool SwipeLeft;
    [HideInInspector] public bool SwipeRight;
    [HideInInspector] public bool SwipeUp;
    [HideInInspector] public bool SwipeDown;

    private CharacterController m_char;

    // Swipe detection
    private Vector2 startTouchPos;
    private bool isDragging = false;
    public float swipeThreshold = 50f;

    private Animator animator;
    private float x;
    private float y;

    public float SpeedDodge;
    public float JumpPower = 7f;
    public bool InJump;
    public bool InRoll;

    public GameObject surfBoad;

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
        // Reset swipes
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;

        // Keyboard input
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) SwipeLeft = true;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) SwipeRight = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) SwipeUp = true;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) SwipeDown = true;

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
                    if (swipeDelta.x > 0) SwipeRight = true;
                    else SwipeLeft = true;
                }
                else if (Mathf.Abs(swipeDelta.y) > swipeThreshold)
                {
                    if (swipeDelta.y > 0) SwipeUp = true;
                    else SwipeDown = true;
                }
            }
            isDragging = false;
        }

        // Movement logic
        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX - laneOffset;
                m_Side = SIDE.Left;
                PlayMoveSound();
                animator.Play(InRoll ? "Swim" : "dodgetLeft");
            }
            else if (m_Side == SIDE.Right)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;
                PlayMoveSound();
                animator.Play(InRoll ? "Swim" : "dodgetLeft");
            }
        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXpos = centerX + laneOffset;
                m_Side = SIDE.Right;
                PlayMoveSound();
                animator.Play(InRoll ? "Swim" : "dodgetRight");
            }
            else if (m_Side == SIDE.Left)
            {
                NewXpos = centerX;
                m_Side = SIDE.Mid;
                PlayMoveSound();
                animator.Play(InRoll ? "Swim" : "dodgetRight");
            }
        }

        // Character controller movement
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 0);
        x = Mathf.Lerp(x, NewXpos, Time.deltaTime * SpeedDodge);
        m_char.Move(moveVector);

        Jump();
        Swim();
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

                // 🔊 Play jump sound
                UISoundManager.Instance.PlayJump();
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
        if (SwipeDown && !InRoll)
        {
            InRoll = true;
            animator.CrossFadeInFixedTime("Swim", 0.1f);

            if (surfBoad != null) surfBoad.SetActive(false);

            // 🔊 Play swim sound
            UISoundManager.Instance.PlaySwim();

            Invoke("EndSwim", 1.0f);
        }
    }

    void EndSwim()
    {
        //InRoll = false;
        //if (surfBoad != null) surfBoad.SetActive(true);
    }

    private void PlayMoveSound()
    {
        UISoundManager.Instance.PlayMove();
    }
}
