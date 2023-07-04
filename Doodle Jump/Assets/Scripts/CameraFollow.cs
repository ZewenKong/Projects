using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.3f;
    Vector3 speed;

    // LateUpdate() 用于追踪 (因为先运行 Update(), 再运行 LateUpdate())
    private void LateUpdate()
    {
        // 当目标位置的 y 分量大于自身位置的 y 分量, 进行追踪
        if (target.position.y > transform.position.y)
        {
            // 确认所移动的位置
            Vector3 targetPosition = new Vector3(0f, target.position.y, -10f);

            // 修改位置
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref speed, smoothSpeed * Time.deltaTime);
            // *追踪常用函数 - SmoothDamp() - SmoothDamp(当前位置, 目标位置, 速度 current speed, 平滑速度 smooth speed)
        }
    }
}

// - - - - - - - - - -

//public class CameraFollow : MonoBehaviour
//{
//    public Transform target;
//    public float smoothSpeed = 0.25f;

//    private Vector3 currentVelocity;

//    // Update is called once per frame 
//    void LateUpdate()
//    { 
//        // check targets y value is greater than the camera y value
//        if (target.position.y > transform.position.y)
//        {
//            Vector3 newPosition = new Vector3(0f, target.position.y, -10f);
//            // make camera move smooth
//            //transform.position = newPosition;
//            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref currentVelocity, smoothSpeed);
//        }
//    }
//}
