using TMPro;
using UnityEngine;

public class Chicken_movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool isPlaying = false;
    [Header("Jump Settings")]
    public float jumpDuration = 1f;
    public float jumpScaleFactor = 1.5f;
    private float jumpTimer = 0f;
    private float currentJumpDuration;
    private Vector3 originalScale;
    private Vector3 targetScale;
    public bool isJumping = false;
    private float[] railPositions = new float[] { -3f, 0f, 3f };
    private int currentRail = 1;
    private Vector3 jumpStartPos;
    private Vector3 jumpTargetPos;
    public bool isRailJump = false;
    private Animator animator;




    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /*void Start()
    {

    }*/

    void Update()
    {
        animator.enabled = isPlaying && !isJumping;
        if (isPlaying)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && currentRail > 0 && !isJumping)
            {
                currentRail--;
                StartRailJump();
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && currentRail < 2 && !isJumping)
            {
                currentRail++;
                StartRailJump();
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !isJumping)
                StartJump(jumpDuration, jumpScaleFactor, false);

            if (isJumping)
                AnimateJump();
        }
    }

    void StartJump(float duration, float scaleFactor, bool railJump)
    {
        originalScale = transform.localScale;
        targetScale = originalScale * scaleFactor;
        jumpTimer = 0f;
        currentJumpDuration = duration;
        isRailJump = railJump;
        isJumping = true;

    }

    void StartRailJump()
    {
        jumpStartPos = transform.position;
        jumpTargetPos = new Vector3(railPositions[currentRail], transform.position.y, transform.position.z);
        StartJump(jumpDuration * 0.75f, jumpScaleFactor * 0.75f, true);
    }

    void AnimateJump()
    {
        jumpTimer += Time.deltaTime;
        float t = jumpTimer / currentJumpDuration;
        float halfDuration = currentJumpDuration / 2f;

        if (isRailJump)
        {
            float moveProgress = Mathf.Clamp01(t);
            Vector3 newPos = Vector3.Lerp(jumpStartPos, jumpTargetPos, moveProgress);
            transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
        }

        if (jumpTimer < halfDuration)
            transform.localScale = Vector3.Lerp(originalScale, targetScale, jumpTimer / halfDuration);
        else if (jumpTimer < currentJumpDuration)
            transform.localScale = Vector3.Lerp(targetScale, originalScale, (jumpTimer - halfDuration) / halfDuration);
        else
        {
            transform.localScale = originalScale;
            if (isRailJump)
                transform.position = new Vector3(jumpTargetPos.x, transform.position.y, transform.position.z);

            jumpTimer = 0f;
            isJumping = false;
            isRailJump = false;
            
        }
    }
}
