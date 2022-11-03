using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 9f;
    public CharacterController controller;
    public SpriteRenderer spriteRenderer;
    public Vector3 direction;
    private Vector3 directiony;
    public float jumpHeight;
    bool sneaking = false;

    public float ydir = 0f;

    public Sprite[] nSprite;
    public Sprite[] neSprite;
    public Sprite[] eSprite;
    public Sprite[] seSprite;
    public Sprite[] sSprite;


    Vector3 moveVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump"))
            ydir = jumpHeight;
        
        ydir += -9.81f * Time.deltaTime;

        if(CameraZone.zone)
            direction = new Vector3(vertical*-1f, 0f, horizontal).normalized;
        else
            direction = new Vector3(horizontal, 0f, vertical).normalized;
        direction.y = ydir;

        if(direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Sneak"))
            if(sneaking)
            {
                sneaking = false;
                speed = 9f;
            }
            else
            {
                sneaking = true;
                speed = 4.5f;
            }
        if(!sneaking)
            if(Input.GetButton("Sprint"))
                speed = 12f;
            else
                speed = 9f;

        HandleSpriteFlip();

        Sprite[] directionSprites = SetSprite();
        if(directionSprites != null)
        {
            spriteRenderer.sprite = directionSprites[0];
        }
    }

    void HandleSpriteFlip()
    {
        if(!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if(spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;
    }

    Sprite[] SetSprite()
    {
        Sprite[] selectedSprite = null;
        if(direction.z > 0) //north
        {
            if(Mathf.Abs(direction.x) > 0)
                selectedSprite = neSprite;
            else
                selectedSprite = nSprite;
        }
        else if(direction.z < 0) //south
        {
            if(Mathf.Abs(direction.x) > 0)
                selectedSprite = seSprite;
            else
                selectedSprite = sSprite;
        }
        else //neutral
        {
            if(Mathf.Abs(direction.x) > 0)
                selectedSprite = eSprite;
        }

        return selectedSprite;
    }
}
