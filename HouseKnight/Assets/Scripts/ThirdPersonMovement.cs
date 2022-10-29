using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 9f;
    public CharacterController controller;
    public SpriteRenderer spriteRenderer;
    public Vector3 direction;

    public Sprite[] nSprite;
    public Sprite[] neSprite;
    public Sprite[] eSprite;
    public Sprite[] seSprite;
    public Sprite[] sSprite;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);


            controller.Move(direction * speed * Time.deltaTime);
        }

        HandleSpriteFlip();

        Sprite[] directionSprites = SetSprite();
        if(directionSprites != null)
        {
            spriteRenderer.sprite = directionSprites[0];
        }
        else
        {

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
