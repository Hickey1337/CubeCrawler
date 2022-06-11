using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public levelManager level;
    public float startSpeed;
    public float speed;
    public float gravity;
    public float jumpForce;
    public int jumpsLeft;
    public bool onWall;// streach goal to have you slide down a wall slower
    public bool onground;

    private Vector3 lastMotion;
    private Vector3 moveVector;
    private float inputDirection;
    private float verticalvelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = startSpeed;
    }

    void Update()
    {
        onground = isControllerGrounded();
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal") * speed;

        //checks to see if the player if grounded if they are dont apply vertical force else reduce their vertical position until they are grounded
        if (isControllerGrounded())
        {
            speed = startSpeed;
            jumpsLeft = 2;
            //hitPad = false;

            //makes the player jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalvelocity = jumpForce;
                jumpsLeft--;
            }
            //allows you to move while on the ground
            moveVector.x = inputDirection;
            if (verticalvelocity<0)
                verticalvelocity = 0;
        }
        else
        {
            //checks for double jump
            if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft>0)
            {
                verticalvelocity = jumpForce;
                jumpsLeft--;
            }
            
            //locks in motion for air
            moveVector.y = verticalvelocity;
            moveVector.x = lastMotion.x;
            verticalvelocity -= gravity * Time.deltaTime;
        }

        moveVector.y = verticalvelocity;
        controller.Move(moveVector * Time.deltaTime);
        lastMotion = moveVector;
    }

    private bool isControllerGrounded()
    {
        Debug.DrawRay(controller.bounds.center, Vector3.down, Color.green);
        return (Physics.Raycast(controller.bounds.center, Vector3.down, (controller.height / 2) + 0.3f));
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //checks to see which side the player is colliding with the ohter obj
        //eg: sides, top, bottom
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            jumpsLeft = 1;//check if double jump avil for double jump out of it later
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpsLeft--;
                moveVector.x = hit.normal.x * speed;
                moveVector.y = verticalvelocity;
            }
        }

        //Collectables
        switch(hit.gameObject.tag)
        {
            //checks the tag for the name in quotes then runs the code inside
            case "Coin":
                Destroy(hit.gameObject);
                break;
            case "JumpPad":
                jumpsLeft = 1;
                verticalvelocity = jumpForce*2.5f;
                break;
            case "Teleport":
                transform.position = hit.transform.GetChild(0).position;
                break;
            case "Deathwarp":
                transform.position = hit.transform.GetChild(0).position;
                level.takeDmg();
                break;
            default:
                break;
        }
    }
}