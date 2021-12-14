using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Transform head;
    public Transform lower1;
    public Transform lower2;
    float speed = 0.005f;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle = head.transform.localRotation.eulerAngles.y;

        if (Input.GetKey(KeyCode.A))
        {
            angle -= 1f;
            head.transform.rotation = Quaternion.Euler(head.transform.localRotation.eulerAngles.x, angle, head.transform.localRotation.eulerAngles.z);
            move(angle);
        }
        if (Input.GetKey(KeyCode.E))
        {
            angle += 1f;
            head.transform.rotation = Quaternion.Euler(head.transform.localRotation.eulerAngles.x, angle, head.transform.localRotation.eulerAngles.z);
            move(angle);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            lower1.transform.rotation = Quaternion.Euler(lower1.transform.localRotation.eulerAngles.x, lower1.transform.localRotation.eulerAngles.y - 1f, lower1.transform.localRotation.eulerAngles.z);
            move(angle);
        }
        if (Input.GetKey(KeyCode.D))
        {
            lower1.transform.rotation = Quaternion.Euler(lower1.transform.localRotation.eulerAngles.x, lower1.transform.localRotation.eulerAngles.y + 1f, lower1.transform.localRotation.eulerAngles.z);
            move(angle);
        }
        if (Input.GetKey(KeyCode.W))
        {
            lower2.transform.rotation = Quaternion.Euler(lower2.transform.localRotation.eulerAngles.x, lower2.transform.localRotation.eulerAngles.y - 1f, lower2.transform.localRotation.eulerAngles.z);
            move(angle);
        }
        if (Input.GetKey(KeyCode.C))
        {
            lower2.transform.rotation = Quaternion.Euler(lower2.transform.localRotation.eulerAngles.x, lower2.transform.localRotation.eulerAngles.y + 1f, lower2.transform.localRotation.eulerAngles.z);
            move(angle);
        }
    }

    void move(float angle)
    {
        if(angle > 180)
        {
            while(angle > 180)
            {
                angle -= 180;
            }
        }
        if(angle < -180)
        {
            while (angle < 180)
            {
                angle += 180;
            }
        }

        float xR;
        float zR;
        if (angle < 90 && angle >= 0)
        {
            xR = -(angle / 90);
            zR = 1 - (-xR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR* speed, head.transform.localPosition.y, head.transform.localPosition.z + zR*speed);
        }
        if (angle >= 90 && angle <= 180)
        {
            zR = -((angle - 90) / 90);
            xR = 1 - (-zR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
        if (angle < 0 && angle >= -90)
        {
            xR = -angle / 90;
            zR = 1 - xR;
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
        if (angle < -90 && angle >= -180)
        {
            zR = -(-(angle + 90f) / 90);
            xR = 1 - (-zR);
            head.transform.localPosition = new Vector3(head.transform.localPosition.x + xR * speed, head.transform.localPosition.y, head.transform.localPosition.z + zR * speed);
        }
    }
}
