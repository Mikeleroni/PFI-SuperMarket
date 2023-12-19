using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnClients : MonoBehaviour
{
    [SerializeField] private float timeToSpawn = 40f;
    [SerializeField] private float timeSinceSpawn;
    [SerializeField] ObjectPool objectPool;
    [SerializeField] Transform exit;
    [SerializeField] Transform[] destinations;


    int typeofClient = 0;
    int typeOfPooledObject = 0;


    private void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn > timeToSpawn)
        {
            typeOfPooledObject = Random.Range(0, 5);

            GameObject client = objectPool.GetPooledObject(objectPool.objectsToPool[typeOfPooledObject]);
            client.transform.position = new Vector3(transform.position.x, transform.position.y);
            client.transform.rotation = transform.rotation;
            client.SetActive(true);
            timeSinceSpawn = 0f;

            typeofClient = Random.Range(1, 4);  

            if(typeofClient == 1) 
            { 
                NormalClientComponent normalClientTree =  client.GetComponent<NormalClientComponent>();
                normalClientTree.destinations = GenerateDestinations();
                normalClientTree.exit = exit;
                normalClientTree.enabled= true;  
            }
            else if(typeofClient == 2)
            {
                StealerComponent stealer = client.GetComponent<StealerComponent>();
                stealer.destinations = GenerateDestinations();
                stealer.exit = exit;
                stealer.enabled= true;
            }
            else
            {
                CompulsiveStealer compulsiveStealer = client.GetComponent<CompulsiveStealer>();
                compulsiveStealer.destinations = GenerateDestinations();
                compulsiveStealer.exit = exit;
                compulsiveStealer.enabled= true;
            }
        }
    }
    public Transform[] GenerateDestinations()
    {
        List<Transform> destinationsTemp = destinations.ToList<Transform>();
        Transform[] newDestinations= new Transform[destinationsTemp.Count];
        
        int destinationRange = Random.Range(2, 6);

        for(int i =0;i<destinationRange;i++) 
        { 
            int destination = Random.Range(0,destinationsTemp.Count);
            newDestinations[i] = destinationsTemp[destination];
            destinationsTemp.RemoveAt(destination);
        }
        return newDestinations;
    }
}
