using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float speed = 0.0f; //Float for player speed
    [SerializeField] Transform cam;
    [SerializeField] CharacterController controller;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] GameObject cameraObj;
    [SerializeField] GameObject thirdPersonCam;
    [SerializeField] GameObject player;

    private Rigidbody rb; //Enable player hitbox
    private bool jumpKeyPressed = false;
    private float v, h;

    
    float turnSmoothVelocity;
   
    AudioSource[] sounds;

    PhotonView PV;




    // Start is called before the first frame update (ie., When the game starts)
    void Awake()
    {
        //Get jump and landing sound effects in an array of sounds
        sounds = GetComponents<AudioSource>();
        //Establish player hitbox/collision box
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // If Photon View is not of the owner of the instance,
        // Delete any objects that cause collisions between players
        if (!PV.IsMine)
        {
            Destroy(cameraObj);
            Destroy(thirdPersonCam);
            Destroy(rb);
            Destroy(player.GetComponent("Player Movement Script"));
        }
    }

    // Update is called once per frame (Two versions of Update, one is tied to framerate, this one isn't)
    void Update()
    {

        // If Photon View is not of the owner of the instance,
        // return and do not allow that player to access Jump() method
        if (!PV.IsMine)
            return;

        Jump();

    }

    //Changes jump mechanic to be based off of jump input rather than if they are grounded or not.
    //This allows the player to jump midair to help with mobility issues.
    //Once jumped, jumpKeyPressed is now true and will not become false again until player impacts ground.
    void Jump()
    {
        if (!jumpKeyPressed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpKeyPressed = true;
                //Add force to player object in direction of button presses
                rb.AddForce(Vector3.up * 7.5f, ForceMode.VelocityChange);
                sounds[0].Play();
            }
        }
       
    }

    private void FixedUpdate()
    {
        // If Photon View is not of the owner of the instance,
        // return and do not allow that player to access Movement() method
        if (!PV.IsMine)
            return;

        Movement();
        
    }



    void Movement()
    {
        h = Input.GetAxisRaw("Horizontal"); //Input.GetAxis reads input from WASD and arrow keys
        v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0f, v);


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * speed * Time.deltaTime);
            //rb.MovePosition(transform.position + moveDir * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ground")
        {
            jumpKeyPressed = false;
            sounds[1].Play();
        }

        if (collision.gameObject.tag == "Wall")
        {
            sounds[2].Play();
        }

    }

}
