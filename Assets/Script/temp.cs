using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("a"))
        {
            
        }

        if (Input.GetButtonDown("d"))
        {

        }

        if (Input.GetButtonDown("s"))
        {

        }

        if (!Input.GetButtonDown("w"))
        {

        }
    }

    IEnumerator Move()
    {
        yield return Time.deltaTime;
    }
}
