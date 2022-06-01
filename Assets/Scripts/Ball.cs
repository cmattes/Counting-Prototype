using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject cameraView;
    public GameObject ballHeldPos;
    public GameObject dogBallHeldPos;
    public bool held;
    public bool ballOnGround = false;
    public bool dogHeld;

    private Renderer ballRenderer;
    private Rigidbody ballRb;
    private int throwForce = 125;
    
    // Start is called before the first frame update
    void Start()
    {
        ballRenderer = GetComponent<Renderer>();
        ballRb = GetComponent<Rigidbody>();
        held = false;
        dogHeld = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (held)
        {
            transform.position = ballHeldPos.gameObject.transform.position;
            transform.RotateAround(cameraView.gameObject.transform.position, cameraView.gameObject.transform.right, cameraView.gameObject.GetComponent<CameraController>().xRotation);
        }
        else if(dogHeld)
        {
            transform.position = dogBallHeldPos.gameObject.transform.position;
        }
    }

    private void OnMouseEnter()
    {
        Material material = ballRenderer.material;

        material.color = Color.red;
    }

    private void OnMouseExit()
    {
        Material material = ballRenderer.material;

        material.color = Color.white;
    }

    private void OnMouseDown()
    {
        if(held)
        {
            ThrowBall();
            return;
        }

        PickupBall();
    }

    private void ThrowBall()
    {
        ballHeldPos.GetComponent<AudioSource>().Play();
        held = false;

        Vector3 throwDirection = (transform.position - cameraView.gameObject.transform.position).normalized;

        ballRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        ballRb.freezeRotation = false;
        ballRb.useGravity = true;
        ballOnGround = true;

    }

    private void PickupBall()
    {
        held = true;

        ballRb.useGravity = false;
        ballRb.freezeRotation = true;
        ballRb.velocity = Vector3.zero;

        transform.position = ballHeldPos.gameObject.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dog"))
        {
            if (string.Equals(collision.gameObject.GetComponent<Dog>().ballOnGround.name, gameObject.name))
            {
                dogHeld = true;

                ballRb.useGravity = false;
                ballRb.freezeRotation = true;
                ballRb.velocity = Vector3.zero;

                transform.position = dogBallHeldPos.gameObject.transform.position;
            }
        }
    }

    /* private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Dog"))
        {
            if (-0.02 < ballRb.velocity.x && ballRb.velocity.x < 0.02
                && -0.02 < ballRb.velocity.y && ballRb.velocity.y < 0.02
                && -0.02 < ballRb.velocity.z && ballRb.velocity.z < 0.02)
            {
                ballOnGround = true;
            }
        }
    } */

    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ballOnGround = false;
        }
    }*/
}
