using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftObject : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.localPosition += -Vector3.right * speed * Time.deltaTime;

        if (Mathf.Abs(transform.localPosition.x) > 8) {
            Destroy(gameObject);
        }
    }
}
