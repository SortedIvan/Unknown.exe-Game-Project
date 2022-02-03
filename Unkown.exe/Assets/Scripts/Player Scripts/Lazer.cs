using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float speed = 5;

    void Update()
    {
        transform.Translate((transform.forward * speed * Time.deltaTime));
    }
}