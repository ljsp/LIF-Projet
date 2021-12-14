using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.gameObject.GetComponent<CarAgent>().AddReward(-0.1f);
            this.gameObject.GetComponent<CarAgent>().EndEpisode();
        }
    }
}
