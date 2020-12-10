using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float recordWaitTime;
    float waitTime;
    PlatformEffector2D effactor;

    void Start()
    {
        effactor = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.Space)) waitTime = recordWaitTime;
        //if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        //{
        //    if (waitTime <= 0)
        //    {
        //        waitTime = recordWaitTime;
        //        effactor.rotationalOffset = 180;
        //    }
        //    else
        //    {
        //        waitTime -= Time.deltaTime;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Space) || target.position.y <= transform.position.y) effactor.rotationalOffset = 0;
    }
}
