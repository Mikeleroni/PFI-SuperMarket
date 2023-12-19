using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam : MonoBehaviour
{
    public Material[] material;
    private Renderer renderer;
    private int camNb = 0;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        renderer.sharedMaterial = material[camNb];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            camNb++;
            renderer.sharedMaterial = material[camNb];
        }
        if(camNb >= material.Length)
        {
            camNb= 0;
            renderer.sharedMaterial = material[camNb];
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            camNb--;
            renderer.sharedMaterial = material[camNb];
        }
        if (camNb < 0)
        {
            camNb = material.Length - 1;
            renderer.sharedMaterial = material[camNb];
        }
    }
}
