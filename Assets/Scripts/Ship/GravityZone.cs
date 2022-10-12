using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class GravityZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Ship>() == null) { return; }

        other.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ship>() == null) { return; }

        other.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
}
