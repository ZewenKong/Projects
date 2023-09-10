using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Doodler : MonoBehaviour
{
    public float movementSpeed = 5f;
    float movement = 0f;  // Initial input
    Rigidbody2D rb;  // Doodler

    private float topScore = 0.0f;
    public Text scoreText;

    void Start()
    {
        // Save the rigidbody2D on Doodler
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input
        movement = Input.GetAxisRaw("Horizontal");

        // Update the x velocity (keep the y velocity)
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);

        // Let Doodler face to left when move to left
        if (movement != 0)  // 当有输入时
        {
            transform.localScale = new Vector3(movement, 1, 1);  // set Scale = 1
        }

        // Update score
        if (rb.velocity.y > 0 && transform.position.y > topScore)
        {
            topScore = transform.position.y;
        }
        scoreText.text = Mathf.Round(topScore).ToString();
    }
}

// - - - - - - - - - -
 
//[RequireComponent(typeof(Rigidbody2D))]  // make sure there is always a Rigidbody2D along this script

//public class Doodler : MonoBehaviour
//{
//    public float movementSpeed = 10f;

//    Rigidbody2D doodler;
//    float movement = 0f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        doodler = GetComponent<Rigidbody2D>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        movement = Input.GetAxisRaw("Horizontal") * movementSpeed;
//    }

//    void FixedUpdate()
//    {
//        Vector2 velocity = doodler.velocity;
//        velocity.x = movement;
//        doodler.velocity = velocity;
//    }
//}
