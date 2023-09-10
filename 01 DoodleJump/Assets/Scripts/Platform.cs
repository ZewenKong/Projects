//using System.Collections;
//using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    // Platform type
    public PlatformType platformType;

    // Bounce speed (interact with platform)
    public float bounceSpeed = 10f;

    // Score
    //private int score = 0;

    //public Text scoreText;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检测碰撞的法线
        if (collision.relativeVelocity.y <= -1f)  // 如果碰撞接触点法线的方向是向下的话  // collision.contacts[0].normal == Vector2.down
        {
            // Get collider rigidbody
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            // If is rigidbody
            if (rb != null)
            {
                // Gives a velocity in top to this body
                Vector2 velocity = rb.velocity;
                velocity.y = bounceSpeed;
                rb.velocity = velocity;
                // - - - - - - - - - -
                //rb.velocity = Vector2.up * bounceSpeed;

                // Score();
            }

            // If collide with Brittle platform
            if (platformType == PlatformType.brittle)
            {
                if (GetComponent<Animator>() != null)  // Animator trigger
                {
                    // Play animation (Platform destroy)
                    GetComponent<Animator>().SetTrigger("Trigger");

                    Invoke("HideGameObject", 0.4f);  // 隐藏 platform

                    // Score();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            gameObject.SetActive(false);
        }
    }

    void HideGameObject()
    {
        gameObject.SetActive(false);
    }

    //void Score()
    //{
    //    // Score 
    //    score++;
    //    // Renew UI
    //    scoreText.text = score.ToString();
    //}
}

public enum PlatformType
{
    // Normal & Brittle
    normal, brittle
}

// - - - - - - - - - -

//using UnityEngine;

//public class Platform : MonoBehaviour
//{
//    // 3.2) set the y-velocity after doodler hit the platform
//    public float jumpForce = 10f;

//    void OnCollisionEnter2D(Collision2D collision)  // variable - collision 
//    {
//        // 4) check the object is from below or above
//        // "collision.relativeVelocity" gives the relative velocity between two objects
//        // if force is from below, doodler will trigger twice
//        if (collision.relativeVelocity.y <= 0f)
//        {
//            // 1) get collider (rigidbody2D is the component)
//            Rigidbody2D platform = collision.collider.GetComponent<Rigidbody2D>();

//            // 2) check the platform interact with player or not
//            if (platform != null)
//            {
//                // platform interact with sth -> add force
//                // platform.AddForce();
//                // generally, add force is related with the falling speed -> higher speed, force smaller, vice versa
//                // however, we want a constant bouncing force/velocity -> specify the velocity

//                // 3) modify one direction velocity (should modify one dimension of a variable in three steps)
//                // 3.1) get the velocity of the rigid body
//                Vector2 velocity = platform.velocity;
//                // 3.3) set the y-velocity after doodler hit the platform
//                velocity.y = jumpForce;
//                platform.velocity = velocity;
//            }
//        }
//    }
//}
