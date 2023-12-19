using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolTreeComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] destinations;
    [SerializeField] float waitTime = 2;
    [SerializeField] float detectionRange = 3;
    NavMeshAgent agent;
    Node root;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupTree();
    }

    private void SetupTree()
    {
        
        root = new Selector(new List<Node>()
        {
            ChasingTree(),
            new PatrolTask(destinations, waitTime, agent)
        });

        
        Node l1 = new IsWithinRange(target, transform, detectionRange);
        Node l2 = new GoToTarget(target, agent);
        Node seq1 = new Sequence(new List<Node>() { l1, l2 });
        Node l3 = new PatrolTask(destinations, waitTime, agent);
        Node sel1 = new Selector(new List<Node>() { seq1, l3 });
        root = sel1;
        
    }

    private Node ChasingTree()
    {
        return new Sequence(new List<Node>()
        {
            new IsWithinRange(target,transform,detectionRange),
            new GoToTarget(target, agent)
        });
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }
}
