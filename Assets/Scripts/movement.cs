using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody2D rb;

    // We can mess with this later on to fine-tune it
    public float jumpHeight = 10f;

    // This will prevent the event from firing a bunch of times per click, instead it will only fire once per click which is what we want
    private bool jumpDebounce = false;

    public GameObject groundChecker;
    bool isGrounded;

    void FixedUpdate()
    {
        // In this line, we're creating an imaginary circle at the player's feet and seeing if it intersects with the ground
        // If it does, that means they're grounded, which means they should be allowed to jump again
        // If it does not intersect with the ground, that means they're in the air and they should not be allowed to jump again until they land
        isGrounded = Physics2D.OverlapCircle(groundChecker.transform.position, 0.1f, LayerMask.GetMask("Ground")) != null;
        if (isGrounded)
        {
            jumpDebounce = false;
        }

        if (Input.touchCount > 0)
        {
            // If the jump debounce is active, we don't want to allow the player to jump again until it's over, so we return out of the function
            if (jumpDebounce == true) return;

            // If we made it this far, that means the jumpDebounce variable must've been set to false, so they are allowed to jump again
            // Let's set it to true to prevent any further jumps until they land from this one
            jumpDebounce = true;

            // Set their velocity aka make them jump
            rb.velocity = new Vector2(0, jumpHeight);

        }
    }
}
