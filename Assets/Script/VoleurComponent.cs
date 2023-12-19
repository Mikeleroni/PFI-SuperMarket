using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class VoleurComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] destinations;
    [SerializeField] float waitTime = 2;
    [SerializeField] float detectionRange = 10;
    [SerializeField] Transform[] victims;
    NavMeshAgent agent;
    Node root;
    Behaviour behaviourTree;
    // Start is called before the first frame update
    private void Awake()
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
        ;


        Node l1 = new IsWithinRange(target, transform, detectionRange);
        Node l2 = new GoToTarget(target, agent);
        Node l4 = new Steal(target);
        Node l5 = new IsTooManyWithinRange(transform, victims, detectionRange);
        Node seq1 = new Sequence(new List<Node>() { l1, l5, l2,l4 });
        Node l3 = new PatrolTask(destinations, waitTime, agent);
        Node sel1 = new Selector(new List<Node>() { seq1, l3 });
        root = sel1;
    }
    void Start()
    {
        
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
