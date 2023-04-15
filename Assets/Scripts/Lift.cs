using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lift : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Transform spawner;
    public float delay;
    public bool titleScreen;
    void Start()
    {
        //set rotation and scale
        
    }
    private void OnEnable() {
        if (!titleScreen) {
            transform.DORotate(new Vector3(0, 0, Random.Range(-15, 15)), 0);
            transform.DOScaleX((Random.Range(0, 1) * 2 - 1), 0);
        }
        StartCoroutine(Spawn());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    void Update() {
        if (!titleScreen) {
            if (transform.position.y > Controller.Instance.player.transform.position.y + 8) {
                Destroy(gameObject);
            }
        }
    }
    IEnumerator Spawn() {
        while (true) {
            Instantiate(obj, spawner.position, transform.rotation, transform);
            yield return new WaitForSeconds(delay);
        }
    }
}
