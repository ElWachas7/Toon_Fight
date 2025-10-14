using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyGoToBaseState<T> : State<T>
{
    protected List<Vector3> _waypoints;
    int _index;
    bool _isFinishPath;

    protected EnemyController _enemy;

    public Node _start;
    public Node _goal;
    public Transform _user;
    public Transform _target;
    public float _distanceToPoint = 2;
    public Vector3 LastTargetPosition { get; private set; }
    public EnemyGoToBaseState(Transform user, Transform target, Node start, Node goal, float distanceToPoint = 0.2f)
    {
        _user = user;
        this._target = target;
        _start = start;
        _goal = goal;
        _enemy = user.GetComponent<EnemyController>();

    }
    protected void Run()
    {
        if (_isFinishPath) return;
        Vector3 point = _waypoints[_index];
        point.y = _user.position.y;
        Vector3 dir = point - _user.position;

        if (dir.magnitude <= _distanceToPoint)
        {
            if (_index + 1 < _waypoints.Count)
            {
                _index++;

            }
            else
            {
                _isFinishPath = true;

                OnFinishPath();
                return;
            }
        }
        OnMove(dir.normalized);

    }
    public virtual void OnMove(Vector3 dir)
    {
        _enemy.Move(dir);
        _enemy.LookDir(dir);
    }
    public virtual void OnStartPath()
    {
        //Debug.Log("OnStartPath");
    }
    public virtual void OnFinishPath()
    {
        //Debug.Log("OnFinishPath");
        _enemy.Move(Vector3.zero); // Stop movement when finishing the path

    }
    public Node GetNearNode(Vector3 position)    //hacer que el pj encuentre el nodo mas cercano
    {
        Collider[] nodes = Physics.OverlapSphere(position, PathfinfingConstants.nearRadius, PathfinfingConstants.nodeMask);
        Node nearNode = null;
        float nearDistance = Mathf.Infinity;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currNode = nodes[i].GetComponent<Node>();
            if (currNode == null) continue;

            var dir = currNode.transform.position - position;
            var currDistance = dir.magnitude;
            if (Physics.Raycast(position, dir.normalized, currDistance, PathfinfingConstants.obsMask)) continue;
            if (nearNode == null || nearDistance > currDistance)
            {
                nearNode = currNode;
                nearDistance = currDistance;
            }
        }
        //Debug.Log(nearNode);
        return nearNode;
    }
    public void SetPathAStar()
    {
        //var init = GetNearNode(_user.transform.position);
        //Debug.Log("Init " + init);
        //_goal = GetNearNode(_target.transform.position);
        Debug.Log(_user.name + "Goal " + _goal);
        LastTargetPosition = _target.transform.position;
        List<Node> path = AStar.Run<Node>(_start, IsSatisfied, GetConnections, GetCost, Heuristic);
        List<Vector3> pathVector = new List<Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            pathVector.Add(path[i].transform.position);
        }
        //Debug.Log("Path " + path.Count);
        //_move.SetPosition(start.transform.position);

        //Vector3 lastTargetPosition = target.transform.position;
        SetWaypoints(pathVector);
    }
    public void SetWaypoints(List<Vector3> newPoints)
    {
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        _index = 0;
        _isFinishPath = false;
        OnStartPath();
    }
    float Heuristic(Node current)
    {
        float distanceMultiplier = 1;

        float h = 0;
        h += Vector3.Distance(current.transform.position, _goal.transform.position) * distanceMultiplier;
        return h;
    }
    float GetCost(Node parent, Node child)
    {
        float distanceMultiplier = 1;
        //float trapMultiplier = 100;

        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * distanceMultiplier;
        //cost += child.hasTrap ? trapMultiplier : 0;
        return cost;
    }
    bool IsSatisfied(Node curr)
    {
        return curr == _goal;
    }
    List<Node> GetConnections(Node curr)
    {

        return curr.neighbors;
    }
    public override void Exit()
    {
        base.Exit();
        _isFinishPath = true;
        _enemy.speed = 0f; // Stop movement when exiting the state
        _enemy.SetPosition(_enemy.transform.position); // Reset position to the last target position
    }
}

