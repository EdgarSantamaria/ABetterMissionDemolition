using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
    {
        //Can destroy wood and glass 
        if (collision.gameObject.tag == "Material")
        {

            Destroy(collision.gameObject);
        }
    }
}

