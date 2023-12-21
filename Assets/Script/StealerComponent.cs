using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StealerComponent : MonoBehaviour
{
    public Transform[] destinations;
    public float waitTime = 5;
    private NavMeshAgent agent;
    [SerializeField] int speed;
    Node root;
    Animator animator;
    Node l2;
    Node l4;
    //Behaviour behaviourTree;
    public Transform exit;
    // Start is called before the first frame update
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        l2 = new CheckPockets(animator);
        l4 = new IsCheckingPocketsFinished(animator);
        l2.SetData("Check", true);
        l4.SetData("Check", true);



        SetupTree();
    }
     void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        l2 = new CheckPockets(animator);
        l4 = new IsCheckingPocketsFinished(animator);
        l2.SetData("Check", true);
        l4.SetData("Check", true);



        SetupTree();
    }
    private void SetupTree()
    {
        Node l3 = new RunOutOfStore(exit, agent, animator, gameObject,speed);
        Node l1 = new GoAroundStore(destinations, waitTime, agent, animator);
        Node seq1 = new Sequence(new List<Node>() { l1, l2, l4,l3 });
        Node sel1 = new Selector(new List<Node>() { seq1 });
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
    

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Sprint"))
        {
            gameObject.tag = "Voleur";
        }
        root.Evaluate();
    }
}
