using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float rotationSpeed = 0.2f;
    public float animationBlendSpeed = 0.2f;
    public float jumpSpeed = 7.0f;

    Animator animator;
    CharacterController controller;
    Camera characterCamera;
    float rotationAngle = 0.0f;
    float targetAnimationSpeed = 0.0f;
    bool isSprint = false;
    bool isDead = false;
    float speedY = 0.0f;
    float gravity = -9.81f;
    bool isJumping = false;
    public static bool isLocked = true;

    public CharacterController Controller
    {
        get { return controller = controller ?? GetComponent<CharacterController>();}
    }
    public Camera CharacterCamera
    {
        get { return characterCamera = characterCamera ?? FindObjectOfType<Camera>();}
    }
    public Animator CharacterAnimator 
    {
        get {return animator = animator ?? GetComponent<Animator>();}
    }
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            CharacterAnimator.SetTrigger("Jump");
            speedY += jumpSpeed;
        }
        if (!Controller.isGrounded)
        {
            speedY += gravity * Time.deltaTime;
        }
        else if (speedY < 0.0f)
        {
            speedY = 0.0f;
        }
        CharacterAnimator.SetFloat("SpeedY", speedY / jumpSpeed);
        if (isJumping && speedY < 0.0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, LayerMask.GetMask("Default")))
            {
                isJumping = false;
                CharacterAnimator.SetTrigger("Land");        
            }
        }


        isSprint = Input.GetKey(KeyCode.LeftShift);
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        Vector3 rotatedMovement = Quaternion.Euler(0.0f, CharacterCamera.transform.rotation.eulerAngles.y, 0.0f) * movement.normalized;
        Vector3 verticalMovement = Vector3.up * speedY;

        float currenSpeed = isSprint ? sprintSpeed : movementSpeed;
        if (!isLocked)
        {
            Controller.Move((verticalMovement + rotatedMovement * currenSpeed) * Time.deltaTime);  
            
            if (rotatedMovement.sqrMagnitude > 0.0f)
            {
                rotationAngle = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
                targetAnimationSpeed = isSprint ? 1.0f : 0.5f;
            } 
            else
            {
                targetAnimationSpeed = 0.0f;
            }
        }
        if (Input.GetKey(KeyCode.Tab) && !isDead)
        {
            CharacterAnimator.SetTrigger("Death");
            isLocked = true;
            isDead = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CharacterAnimator.SetTrigger("Punch");
            CharacterAnimator.SetInteger("PunchID", Random.Range(1,5));
            
        }

        CharacterAnimator.SetFloat("Speed", Mathf.Lerp(CharacterAnimator.GetFloat("Speed"), targetAnimationSpeed, animationBlendSpeed));
        Quaternion currentRotation = Controller.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
        Controller.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed);
    }
}
