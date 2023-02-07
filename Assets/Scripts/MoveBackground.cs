using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    private float startPos;
    private GameObject cam;
    [SerializeField] private float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("CM vcam1");
        startPos = transform.position.x;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        float distance = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        */
    }
}
