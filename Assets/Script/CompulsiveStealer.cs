using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompulsiveStealer : MonoBehaviour
{
    public Transform[] destinations;
    public float waitTime = 5;
    private NavMeshAgent agent;
    [SerializeField] int speed;
    Node root;
    Animator animator;
    //Behaviour behaviourTree;
    public Transform exit;
    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetupTree();
    }
    private void SetupTree()
    {
        Node l3 = new RunOutOfStore(exit, agent, animator, gameObject, speed);
        Node l1 = new GoAroundStore(destinations, waitTime, agent, animator);
        Node seq1 = new Sequence(new List<Node>() { l1, l3 });
        Node sel1 = new Selector(new List<Node>() { seq1 });
        root = sel1;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }

}
