using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveShrinker : MonoBehaviour
{
    float myScale;
    private void Start() {
        myScale = transform.localScale.x;
    }

    void Update()
    {
        float bottom = Controller.Instance.BottomOfScreen;
        float top = bottom + 8;
        float scaleFactor = ((transform.position.y - bottom) * .2f) / (top - bottom);

        float scale = Mathf.Min(myScale,myScale - (scaleFactor*myScale));

        //transform.localScale = Vector3.one * scale;
    }
}
