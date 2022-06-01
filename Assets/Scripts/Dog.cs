using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject retrievalBox;
    private List<GameObject> balls;
    private bool retrievingBall = false;
    public GameObject ballOnGround;
    private bool returningBall = false;
    [SerializeField] private float speed = 5;
    private Animator dogAnim;

    // Start is called before the first frame update
    void Start()
    {
        balls = new List<GameObject>();
        dogAnim = gameObject.GetComponent<Animator>();
        Invoke("FindAllBalls", 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!retrievingBall && !returningBall && balls.Exists(b => b.gameObject.GetComponent<Ball>().ballOnGround))
        {
            retrievingBall = true;
            ballOnGround = balls.Find(ball => ball.gameObject.GetComponent<Ball>().ballOnGround).gameObject;
            dogAnim.SetFloat("Speed_f", 1);
        }
        else if(retrievingBall)
        {
            if(dogAnim.GetFloat("Speed_f") != 1)
            {
                dogAnim.SetFloat("Speed_f", 1);
            }

            var ballDirection = (ballOnGround.gameObject.transform.position - transform.position).normalized;
            ballDirection.y = 0;
            var ballLookRotation = Quaternion.LookRotation(ballDirection);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, ballLookRotation, Time.deltaTime * speed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }else if(returningBall)
        {
            var destinationDirection = (retrievalBox.gameObject.transform.position - transform.position).normalized;
            destinationDirection.y = 0;
            var lookRotation = Quaternion.LookRotation(destinationDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if(balls.Exists(b => b.gameObject.GetComponent<Ball>().held))
        {
            dogAnim.SetBool("Sit_b", false);
        }
        else if(!dogAnim.GetBool("Sit_b"))
        {
            dogAnim.SetBool("Sit_b", true);
        }
    }

    private void FindAllBalls()
    {
        var allBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach(var b in allBalls)
        {
            balls.Add(b);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            if (string.Equals(collision.gameObject.name, ballOnGround.gameObject.name))
            {
                returningBall = true;
                retrievingBall = false;
                ballOnGround.gameObject.GetComponent<Ball>().ballOnGround = false;
            }
        }

        if (collision.gameObject.CompareTag("EndBox"))
        {
            dogAnim.SetFloat("Speed_f", 0);
            returningBall = false;
            ballOnGround.gameObject.GetComponent<Ball>().dogHeld = false;
            var ballRb = ballOnGround.gameObject.GetComponent<Rigidbody>();
            ballRb.freezeRotation = false;
            ballRb.useGravity = true;
        }
    }
}
