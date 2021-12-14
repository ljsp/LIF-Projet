using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 6;
    private float i;
    private float j;

    private void Start()
    {
        i = this.transform.localPosition.x;
        j = this.transform.localPosition.z;

        this.transform.localPosition = new Vector3(i, 1, Random.Range(j-2, j+2));
    }
    private void Update()
    {
        if(i > -4f)
        {
            i -= speed/100;
            this.transform.localPosition = new Vector3(i, 1, this.transform.localPosition.z);
        }
        else
        {
            i = 100;
            this.transform.localPosition = new Vector3(100, 1, Random.Range(j-2, j+2));
        }
        
    }

}
