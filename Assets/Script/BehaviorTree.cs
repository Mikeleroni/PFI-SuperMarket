
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.ProBuilder;
using static UnityEngine.GraphicsBuffer;

public enum NodeState { Running, Success, Failure }
public abstract class Node
{
    protected NodeState state { get; set; }
    public Node parent = null;
    protected List<Node> children = new();

    Dictionary<string, object> data = new Dictionary<string, object>();

    public void SetData(string key, object value)
    {
        data[key] = value;
    }

    public object GetData(string key)
    {
        if (data.TryGetValue(key, out object value))
            return value;
        if (parent != null)
            return parent.GetData(key);

        return null;
    }

    public bool RemoveData(string key)
    {
        if (data.Remove(key))
        {
            return true;
        }
        if (parent != null)
            return parent.RemoveData(key);

        return false;
    }



    public Node()
    {
        state = NodeState.Running;
        parent = null;
    }

    public Node(List<Node> pChildren)
    {
        state = NodeState.Running;
        parent = null;
        foreach (Node child in pChildren)
            Attach(child);
    }

    protected void Attach(Node n)
    {
        children.Add(n);
        n.parent = this;
    }
    public abstract NodeState Evaluate();
}

public class Selector : Node
{
    public Selector(List<Node> n) : base(n) { }

    public override NodeState Evaluate()
    {
        foreach (Node n in children)
        {
            NodeState localstate = n.Evaluate();
            switch (localstate)
            {
                case NodeState.Failure:
                    continue;
                case NodeState.Success:
                case NodeState.Running:
                    state = localstate;
                    return state;
            }
        }
        state = NodeState.Failure;
        return state;
    }
}

public class Sequence : Node
{
    public Sequence(List<Node> n) : base(n) { }

    public override NodeState Evaluate()
    {
        foreach (Node n in children)
        {
            NodeState localstate = n.Evaluate();
            switch (localstate)
            {
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                case NodeState.Running:
                    state = localstate;
                    return state;
            }
        }
        state = NodeState.Success;
        return state;
    }
}
public class XOR : Node
{
    public XOR(Node n) : base()
    {
        Attach(n);
    }
    public override NodeState Evaluate()
    {
        //Si un enfant est running , il retourne running
        int successCount = 0;
        int failureCount = 0;
        foreach (Node n in children)
        {
            NodeState localstate = n.Evaluate();

            if (localstate == NodeState.Failure)
            {
                ++failureCount;
                continue;
            }
            else if (localstate == NodeState.Success)
            {
                ++successCount;
                continue;
            }
            else if (localstate == NodeState.Running)
            {
                return NodeState.Running;
            }

        }
        if (failureCount == children.Count)
        {
            return NodeState.Failure;
        }
        if (successCount == 1)
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }
}

public class Inverter : Node
{
    public Inverter(Node n) : base()
    {
        Attach(n);
    }

    public override NodeState Evaluate()
    {
        switch (children[0].Evaluate())
        {
            case NodeState.Success:
                state = NodeState.Failure;
                break;
            case NodeState.Failure:
                state = NodeState.Success;
                break;
            case NodeState.Running:
                state = NodeState.Running;
                break;
        }
        return state;
    }
}

public class PatrolTask : Node
{
    Transform[] destinations;
    int destinationIndex = 0;

    NavMeshAgent agent;
    float waitTime;
    float elapsedTime = 0;
    bool isWaiting = false;

    public PatrolTask(Transform[] destinations, float waitTime, NavMeshAgent agent)
    {
        this.destinations = destinations;
        this.waitTime = waitTime;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (isWaiting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                isWaiting = false;
            }
        }
        else
        {
            if (Vector3.Distance(agent.transform.position, destinations[destinationIndex].position) < /*agent.stoppingDistance*/1)
            {
                destinationIndex = (destinationIndex + 1) % destinations.Length;
                isWaiting = true;
                elapsedTime = 0;
            }
            else
            {
                agent.destination = destinations[destinationIndex].position;
            }
        }

        state = NodeState.Running;
        return state;
    }
}

public class GoAroundStore : Node
{
    Transform[] destinations;
    int destinationIndex = 0;

    NavMeshAgent agent;
    float waitTime;
    float elapsedTime = 0;
    bool isWaiting = false;
    Animator animator;
    public GoAroundStore(Transform[] destinations, float waitTime, NavMeshAgent agent, Animator animator)
    {
        this.destinations = destinations;
        this.waitTime = waitTime;
        this.agent = agent;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        if (isWaiting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                isWaiting = false;
            }
        }
        else
        {
            if (destinationIndex < destinations.Length)
            {
                if (!isWaiting)
                {
                    if (Vector3.Distance(agent.transform.position, destinations[destinationIndex].position) < /*agent.stoppingDistance*/1)
                    {
                        destinationIndex++;
                        isWaiting = true;
                        elapsedTime = 0;
                        animator.SetFloat("Speed", 0);
                    }
                    else
                    {
                        agent.speed = 2;
                        animator.SetFloat("Speed", 0.5f);
                        agent.destination = destinations[destinationIndex].position;
                    }
                }
                return NodeState.Failure;
            }
            else
            {
                agent.transform.rotation = Quaternion.Euler(0,0,0);
                animator.SetBool("Searching", true);
                return NodeState.Success;
            }
        }

        return state;
    }
}
public class CheckPockets : Node
{
    Animator animator;
   // public bool animationEnded = false;
    public bool animationStarted = false;
    bool animationTriggered = false;
    public CheckPockets(Animator animator)
    {     
      this.animator = animator;
     
    }

    public override NodeState Evaluate()
    {
       // Debug.Log(GetData("Check"));
        if ((bool)GetData("Check"))
        {
          animator.SetBool("Searching",true);
            return NodeState.Failure;
        }
          return NodeState.Success;
    }
}

public class IsCheckingPocketsFinished : Node
{
    Animator animator;

    public IsCheckingPocketsFinished(Animator animator)
    {
        this.animator = animator;
    }
    public override NodeState Evaluate()
    {
       // Debug.Log(animator.GetBool("Searching"));
        if (!((bool)GetData("Check")))
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
public class IsHandingFinished : Node
{
    Animator animator;

    public IsHandingFinished(Animator animator)
    {
        this.animator = animator;
    }
    public override NodeState Evaluate()
    {
        if (!((bool)GetData("Handing")))
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}

public class HandingMoney : Node
{
    Animator animator;
    public HandingMoney(Animator animator)
    {
        this.animator = animator;
    }
    public override NodeState Evaluate()
    {
        if ((bool)GetData("Handing"))
        {
            animator.SetBool("Handing", true);
            return NodeState.Failure;
        }
        return NodeState.Success;
    }
}

public class GoToTarget : Node
{
    Transform target;
    NavMeshAgent agent;

    public GoToTarget(Transform target, NavMeshAgent agent)
    {
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        agent.destination = target.position;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            state = NodeState.Success;
            return state;
        }
        state = NodeState.Running;
        return state;
    }
}
public class ExitStore : Node
{
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    GameObject gameObject;

    public ExitStore(Transform target, NavMeshAgent agent, Animator animator, GameObject gameObject)
    {
        this.target = target;
        this.agent = agent;
        this.animator = animator;
        this.gameObject = gameObject;
    }

    public override NodeState Evaluate()
    {
        animator.SetBool("Searching",false);
        animator.SetFloat("Speed", 0.7f);
        agent.destination = target.position;
        if (Vector3.Distance(agent.transform.position,target.position) < /*agent.stoppingDistance*/1)
        {
            state = NodeState.Success;
            NormalClientComponent normal = gameObject.GetComponent<NormalClientComponent>();
            normal.enabled= false;
            gameObject.SetActive(false);//Object Pool
            return state;
        }
        state = NodeState.Running;
        return state;
    }
}
public class RunOutOfStore : Node 
{
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    GameObject gameObject;
    int speed;

    public RunOutOfStore(Transform target, NavMeshAgent agent, Animator animator, GameObject gameObject,int speed)
    {
        this.target = target;
        this.agent = agent;
        this.animator = animator;
        this.gameObject = gameObject;
        this.speed = speed;
    }

    public override NodeState Evaluate()
    {
        animator.SetBool("Searching", false);
        animator.SetBool("Sprint", true);
        animator.SetFloat("Speed", 1.2f);
        agent.speed = speed;
        agent.destination = target.position;
        gameObject.tag = "Voleur";
        if (Vector3.Distance(agent.transform.position, target.position) < /*agent.stoppingDistance*/1)
        {
            state = NodeState.Success;
            StealerComponent stealer = gameObject.GetComponent<StealerComponent>();
            stealer.enabled = false;
            CompulsiveStealer compulsiveStealer = gameObject.GetComponent<CompulsiveStealer>();
            compulsiveStealer.enabled = false;


            gameObject.SetActive(false);//Object Pool
            return state;
        }
        state = NodeState.Running;
        return state;
    }
}


public class IsWithinRange : Node
{
    Transform target;
    Transform self;
    float detectionRange;

    public IsWithinRange(Transform target, Transform self, float detectionRange)
    {
        this.target = target;
        this.self = self;
        this.detectionRange = detectionRange;
    }

    public override NodeState Evaluate()
    {
        state = NodeState.Failure;
        if (Vector3.Distance(self.position, target.position) <= detectionRange)
        {
            state = NodeState.Success;
        }
        return state;
    }
}
public class Steal : Node
{
    Transform target;

    public Steal(Transform target)
    {
        this.target = target;
        SetData(target.gameObject.name, target);
    }

    public override NodeState Evaluate()
    {
        Debug.Log($"le voleur a volé {GetData(target.gameObject.name)}");
        return NodeState.Success;
    }
}
public class IsTooManyWithinRange : Node
{

    float detectionRange = 10;
    bool TooManyWithinRange = false;
    Transform self;
    Transform[] victims;

    public IsTooManyWithinRange(Transform self, Transform[] victims, float detectionRange)
    {
        this.victims = victims;
        this.self = self;
        this.victims = victims;
        this.detectionRange = detectionRange;
    }
    public override NodeState Evaluate()
    {
        foreach (Transform t in victims)
        {
            if (Vector3.Distance(self.position, t.position) <= detectionRange)
            {
                TooManyWithinRange = true;
            }
            else
            {
                TooManyWithinRange = false;
            }
        }
        if (TooManyWithinRange)
        {
            return NodeState.Failure;
        }
        return NodeState.Success;
    }
}

