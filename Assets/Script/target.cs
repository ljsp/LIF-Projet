using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        CrawlerModified cm = transform.parent.GetComponent<CrawlerModified>();

        if (other.gameObject.tag == "ground")
        {
            cm.reward(1f);
        }
    }
}
