using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************/
/*********************   Hover Class  *****************/
/******************************************************/


public class Hover : Singleton<Hover>
{
    /******************************************************/
    /******** Hover Class Component ***********************/
    /******************************************************/
    // Hover class component: spriteRenderer
    private SpriteRenderer spriteRenderer = null;

    /******************************************************/
    /******** Hover Basic Functions ***********************/
    /******************************************************/

    // Start is called before the first frame update
    void Start()
    {
        // Initialize hover with the current sprite renderer
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the hover as mouse cursor moves
        FollowMouse();
    }

    /******************************************************/
    /**** Hover Manual Function Controls ******************/
    /******************************************************/

    // Functions for the hover to follow the mouse cursor
    private void FollowMouse()
    {
        if (spriteRenderer.enabled){
            // Set the spirite renderer's position to the mouse cursor position
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Chang the Z component to 0 so that the hover is in plane
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    // Activate hover 
    public void Activate(Sprite sprite)
    {
        // Update hover with the given sprite
        this.spriteRenderer.sprite = sprite;
        // Enable sprite display
        spriteRenderer.enabled = true;
    }

    // Release hover
    public void Deactivate()
    {
        // Disable sprite display
        spriteRenderer.enabled = false;
        // Reset the clicked button
        GameManager.Instance.ClickedBtn = null;
    }
}
