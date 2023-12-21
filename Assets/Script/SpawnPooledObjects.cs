using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class SpawnPooledObjects : MonoBehaviour
{

    [SerializeField] private float timeToSpawn = 40f;
    [SerializeField] private float timeSinceSpawn;
    [SerializeField] ObjectPool objectPool;
    [SerializeField] Transform exit;
    [SerializeField] Transform[] destinations;
    [SerializeField] Transform caisse;


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

            typeofClient = Random.Range(1, 5);

            if (typeofClient == 1)
            {
                client.tag = "Untagged";
                CompulsiveStealer compulsiveStealer = client.GetComponent<CompulsiveStealer>();
                client.GetComponent<NavMeshAgent>().enabled = true;
                compulsiveStealer.destinations = GenerateDestinations(typeofClient);
                compulsiveStealer.exit = exit;
                compulsiveStealer.enabled = true;
            }
            else if (typeofClient == 2)
            {
                client.tag = "Untagged";
                StealerComponent stealer = client.GetComponent<StealerComponent>();
                client.GetComponent<NavMeshAgent>().enabled = true;
                stealer.destinations = GenerateDestinations(typeofClient);
                stealer.exit = exit;
                stealer.enabled = true;
            }
            else
            {
                client.tag = "Untagged";
                NormalClientComponent normalClientTree = client.GetComponent<NormalClientComponent>();
                client.GetComponent<NavMeshAgent>().enabled = true;
                normalClientTree.destinations = GenerateDestinations(typeofClient);
                normalClientTree.exit = exit;
                normalClientTree.enabled = true;
            }
        }
    }
    public Transform[] GenerateDestinations(int typeOfClient)
    {
        List<Transform> destinationsTemp = destinations.ToList<Transform>();

        int destinationRange = Random.Range(2, 6);
        Transform[] newDestinations = new Transform[destinationRange];

        for (int i = 0; i < destinationRange; i++)
        {
            if (typeOfClient != 1)
            {
                if (i < destinationRange - 1)
                {
                    int destination = Random.Range(0, destinationsTemp.Count);
                    newDestinations[i] = destinationsTemp[destination];
                    destinationsTemp.RemoveAt(destination);
                }
                else
                {
                    newDestinations[i] = caisse;
                }
            }
            else
            {
                int destination = Random.Range(0, destinationsTemp.Count);
                newDestinations[i] = destinationsTemp[destination];
                destinationsTemp.RemoveAt(destination);
            }
        }
        return newDestinations;
    }
}

