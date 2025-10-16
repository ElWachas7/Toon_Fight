using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyGoToBaseState<T> : State<T>
{
       protected List<Vector3> _waypoints = new List<Vector3>(); // inicializado
    int _index = 0;
    bool _isFinishPath = true;
    bool _pathSet = false;

    protected EnemyController _enemy;

    public Node _start;
    public Node _goal;
    public Transform _user;
    public Transform _target;
    public float _distanceToPoint = 0.2f; // valor por defecto razonable
    public Vector3 LastTargetPosition { get; private set; }

    public EnemyGoToBaseState(Transform user, Transform target, Node start, Node goal, float distanceToPoint = 0.2f)
    {
        _user = user;
        _target = target;
        _start = start;
        _goal = goal;
        _distanceToPoint = distanceToPoint;

        if (_user != null)
            _enemy = _user.GetComponent<EnemyController>();
    }

    public override void Enter()
    {
        Debug.Log($"Entering GoToEnemyBase State for {_user?.name} | start:{_start} goal:{_goal}");
        if (_enemy == null)
            Debug.LogWarning($"{_user?.name}: _enemy es null en Enter()");

        SetPathAStar();
    }

    public override void Execute()
    {
        base.Execute();
        Run();
    }

    protected void Run()
    {
        if (_isFinishPath) return;

        // Protección: enemy/user no nulos
        if (_enemy == null || _user == null)
        {
            Debug.LogWarning($"Run(): _enemy o _user es null. _enemy={_enemy} _user={_user}");
            _isFinishPath = true;
            OnFinishPath();
            return;
        }

        if (_waypoints == null || _waypoints.Count == 0)
        {
            Debug.LogWarning($"{_user.name}: Run detectó waypoints vacíos. Fin de path.");
            _isFinishPath = true;
            OnFinishPath();
            return;
        }

        if (_index < 0) _index = 0;
        if (_index >= _waypoints.Count)
        {
            Debug.LogWarning($"{_user.name}: index fuera de rango ({_index}) / waypoints.Count {_waypoints.Count}. Finalizando path.");
            _isFinishPath = true;
            OnFinishPath();
            return;
        }

        Vector3 point = _waypoints[_index];
        point.y = _user.position.y;
        Vector3 dir = point - _user.position;

        // Si estamos lo suficientemente cerca, pasar al siguiente
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

        // Mover/rotar
        OnMove(dir.normalized);
    }

    public virtual void OnMove(Vector3 dir)
    {
        if (_enemy == null) return;
        _enemy.Move(dir);
        _enemy.LookDir(dir);
    }

    public virtual void OnStartPath()
    {
        _pathSet = true;
        _isFinishPath = false;
        _index = 0;
        Debug.Log($"{_user?.name}: OnStartPath - waypoints count {_waypoints?.Count}");
    }

    public virtual void OnFinishPath()
    {
        _pathSet = false;
        _isFinishPath = true;
        _enemy?.Move(Vector3.zero);
        Debug.Log($"{_user?.name}: OnFinishPath triggered");
    }

    public Node GetNearNode(Vector3 position)
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
        return nearNode;
    }

    public void SetPathAStar()
    {
        Debug.Log($"{_user?.name} - SetPathAStar start: {_start?.name} | goal: {_goal?.name}");

        if (_start == null || _goal == null)
        {
            Debug.LogWarning($"{_user?.name}: Start o Goal es null. No se calcula path.");
            _waypoints = new List<Vector3>();
            _isFinishPath = true;
            OnFinishPath();
            return;
        }

        LastTargetPosition = _target != null ? _target.position : Vector3.zero;

        List<Node> path = AStar.Run<Node>(_start, IsSatisfied, GetConnections, GetCost, Heuristic);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning($"{_user?.name}: AStar devolvió path nulo o vacío.");
            _waypoints = new List<Vector3>();
            _isFinishPath = true;
            OnFinishPath();
            return;
        }

        Debug.Log($"{_user?.name}: AStar path.Count = {path.Count}");
        List<Vector3> pathVector = new List<Vector3>(path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] == null)
            {
                Debug.LogWarning($"{_user?.name}: path contiene node nulo en index {i}");
                continue;
            }
            pathVector.Add(path[i].transform.position);
            Debug.Log($"{_user?.name}: Path node[{i}] -> {path[i].name} id {path[i].GetInstanceID()}");
        }

        SetWaypoints(pathVector);
    }

    public void SetWaypoints(List<Vector3> newPoints)
    {
        if (newPoints == null || newPoints.Count == 0)
        {
            Debug.LogWarning($"{_user?.name}: newPoints es nulo o vacío en SetWaypoints.");
            _waypoints = new List<Vector3>();
            _isFinishPath = true;
            OnFinishPath();
            return;
        }
        _waypoints = newPoints;
        _index = 0;
        _isFinishPath = false;
        OnStartPath();
    }

    float Heuristic(Node current)
    {
        if (current == null)
        {
            Debug.LogWarning($"{_user?.name}: Heuristic recibido current == null");
            return float.MaxValue / 2f;
        }
        if (_goal == null)
        {
            Debug.LogWarning($"{_user?.name}: Heuristic con _goal == null");
            return float.MaxValue / 2f;
        }
        return Vector3.Distance(current.transform.position, _goal.transform.position);
    }

    float GetCost(Node parent, Node child)
    {
        if (parent == null || child == null) return float.MaxValue / 2f;
        return Vector3.Distance(parent.transform.position, child.transform.position);
    }

    bool IsSatisfied(Node curr)
    {
        return curr == _goal;
    }

    List<Node> GetConnections(Node curr)
    {
        // Preservá tu DebugGetConnections aquí si querés más detalles.
        return curr?.neighbors ?? new List<Node>();
    }

    public override void Exit()
    {
        base.Exit();
        _isFinishPath = true;
        if (_enemy != null) _enemy.speed = 0f;
        _enemy?.SetPosition(_enemy.transform.position);
    }
}

