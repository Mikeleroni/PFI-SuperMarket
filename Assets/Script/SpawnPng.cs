using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPng : MonoBehaviour
{
    [SerializeField] float tempsEntreSpawn = 10;
    [SerializeField] GameObject[] png;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Tirer());
        InvokeRepeating(nameof(Spawn), 0, tempsEntreSpawn);
    }

    void Spawn()
    {
        int i = UnityEngine.Random.Range(0, png.Length);
        GameObject obj = ObjectPool.objectPoolInstance.GetPooledObject(png[i]);

        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
        }
    }
}
