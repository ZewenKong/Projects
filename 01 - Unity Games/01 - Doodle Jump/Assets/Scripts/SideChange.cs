using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideChange : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Get the Transform component from the target object
        Transform target = collision.gameObject.transform;
          
        // Adjust the position (调成相反的 x 值)
        target.position = new Vector3((-target.position.x) / 0.9f, target.position.y, target.position.z);
        // (-target.position.x) / 0.9f - 将 Doodler 往中心挪动, 以防止 Doodler 在两边不停穿越
    }
}
