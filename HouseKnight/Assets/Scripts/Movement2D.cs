using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Movement2D : MonoBehaviour
{
    // // Start is called before the first frame update
    // private Rigidbody rb;
    // private Vector3 moveVec;
    // public float speed = 9f;
    // public CharacterController controller;
    // public SpriteRenderer spriteRenderer;
    // public Vector3 direction;
    // private Vector3 directiony;
    // public float jumpHeight;
    // bool jumping = false;
    // bool ducking = false;

    // public float ydir = 0f;

    // public Sprite[] nSprite;


    // Vector3 moveVelocity;

    // // Update is called once per frame
    // void Update()
    // {
    //     float horizontal = Input.GetAxisRaw("Horizontal");
    //     float vertical = Input.GetAxisRaw("Vertical");

    //     if(Input.GetButtonDown("Jump")&&jumping==false)
    //         jumping=true;
    //         Vector3 jumpVec = new Vector3(0,0.05f,0);
    //         GetComponent<Rigidbody>().AddForce(jumpVec*Time.deltaTime);
    //         ydir = jumpHeight;
        
    //     ydir += -9.81f * Time.deltaTime;

    //     // if(CameraZone.zone)
    //     //     direction = new Vector3(vertical*-1f, 0f, horizontal).normalized;
    //     // else
    //     //     direction = new Vector3(horizontal, 0f, vertical).normalized;
    //     // direction.y = ydir;

    //     // if(direction.magnitude >= 0.1f)
    //     // {
    //     //     controller.Move(direction * speed * Time.deltaTime);
    //     // }

    //     if(Input.GetButtonDown("Sneak"))
    //         ducking = true;
    //     if(Input.GetButtonUp("Sneak"))
    //         ducking = false;
    // }
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     transform.position = new Vector3(0,2,0);
    // }
    
    // // void OnMove(InputValue moveVal)
	// // {
	// // 	moveVec = moveVal.Get<Vector3>();
	// // }

    // // void FixedUpdate()
	// // {
    // //     moveVec.x = Input.GetAxisRaw ("Horizontal");
    // //     moveVec.y = Input.GetAxisRaw ("Vertical");
    // //     moveVec.z=0;
    // //     rb.AddForce(moveVec * 1 * Time.fixedDeltaTime);
    // //     float angle = 90 + Mathf.Atan2(moveVec.y, moveVec.x)*180/Mathf.PI;
    // //     if(moveVec!=new Vector3(0,0,0)){
    // //     // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,angle), rotateDeg);
    // //     }
    // // }


	private Rigidbody rb;
	private float moveX;
	private float moveZ;
		
	public float speed=1;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnMove(InputValue moveVal)
	{
		Vector2 moveVec = moveVal.Get<Vector2>();
		
		moveX = moveVec.x;
		moveZ = moveVec.y;
	}
	
	void FixedUpdate()
	{
		rb.AddForce(new Vector3(moveX, 0.0f, moveZ) * speed);
	}
	

}
