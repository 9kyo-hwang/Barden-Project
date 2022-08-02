using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class test : MonoBehaviour
{
    public Transform target;
    public float rand;
    
    // Start is called before the first frame update
    void Start()
    {
        System.Random r = new System.Random();
        rand = (float)r.NextDouble();
        Debug.Log(rand);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.position - this.transform.position;
        var angle = Vector2.Angle(Vector2.right, dir);

        if (dir.y < 0)
        {
            angle *= -1;
        }

        transform.eulerAngles = Vector3.forward * angle;
    }
}
