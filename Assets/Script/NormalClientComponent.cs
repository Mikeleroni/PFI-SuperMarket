using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;

public class NormalClientComponent : MonoBehaviour
{
    public Transform[] destinations;
    public float waitTime = 5;
    public NavMeshAgent agent;
    Node root;
    Animator animator;
    Node l2;
    Node l4;
    Node l5;
    Node l6;
    //Behaviour behaviourTree;
    public Transform exit;
    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();    
        agent = GetComponent<NavMeshAgent>();
        l2 = new CheckPockets(animator);
        l4 = new IsCheckingPocketsFinished(animator);
        l5 = new HandingMoney(animator);
        l6= new IsHandingFinished(animator);
        l2.SetData("Check", true);
        l4.SetData("Check", true);
        l5.SetData("Handing", true);
        l6.SetData("Handing", true);


        SetupTree();
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        l2 = new CheckPockets(animator);
        l4 = new IsCheckingPocketsFinished(animator);
        l5 = new HandingMoney(animator);
        l6 = new IsHandingFinished(animator);
        l2.SetData("Check", true);
        l4.SetData("Check", true);
        l5.SetData("Handing", true);
        l6.SetData("Handing", true);


        SetupTree();
    }
    private void SetupTree()
    {
        Node l3 = new ExitStore(exit, agent, animator, gameObject);
        Node l1 = new GoAroundStore(destinations, waitTime, agent, animator);
        Node seq1 = new Sequence(new List<Node>() { l1,l2,l4,l5,l6,l3});
        Node sel1 = new Selector(new List<Node>() { seq1});
        root = sel1;
    }
    void Start()
    {

    }
    public void AnimationEnd()
    {
        l2.SetData("Check", false);
        l4.SetData("Check", false);

        animator.SetBool("Searching", false);
    }
    public void HandingFinished()
    {
        l5.SetData("Handing", false);
        l6.SetData("Handing", false);
        
        animator.SetBool("Handing", false);
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }
}
