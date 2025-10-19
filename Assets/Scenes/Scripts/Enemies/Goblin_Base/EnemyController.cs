using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody _rb;
    public ObstacleAvoidance _obs;
    public EnemyData enemyData;
    public PlayerMovement player;

    [Header("Spawn Points & End Points")]
    public List<Node> spawnPoints;
    public List<Node> endPoints;
    Node _start;
    Node _goal;
    [SerializeField] private Health _health;

    [Header("Components")]
    public float currentHealth;
    public float speed;
    public float speedRot;
    public int damage;
    public float attackdelay;
    public int range;

    [Header("Behavior Tree")]
    ITreeNode _tree;


    [Header("States")]
    FSM<StateEnum> _fsm;
    private EnemyDeathState<StateEnum> _deathState;
    private EnemyAttackState<StateEnum> _attackState;
    private EnemyGoToBaseState<StateEnum> _goToBaseState;
    private EnemyReachedBase<StateEnum> _reachedBaseState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
        player = FindObjectOfType<PlayerMovement>();
        spawnPoints.Add(FindAnyObjectByType<GameManager>().spawnPoint1);
        spawnPoints.Add(FindAnyObjectByType<GameManager>().spawnPoint2);
        endPoints.Add(FindAnyObjectByType<GameManager>().endPoint);
        endPoints.Add(FindAnyObjectByType<GameManager>().endPoint2);
        _start = spawnPoints[Random.Range(0, spawnPoints.Count)];
        _goal = endPoints[Random.Range(0, endPoints.Count)];
        

    }
    private void Start()
    {
        currentHealth = _health.EnemyHealth;
        speed = enemyData.speed;
        speedRot = enemyData.rotationSpeed;
        damage = enemyData.damage;
        attackdelay = enemyData.attackDelay;
        range = enemyData.range;

        InitializeFSM();
        InitializeTree();
    }


    public void Move(Vector3 dir)
    {
        dir = _obs.GetDir(dir);
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void LookDir(Vector3 dir)
    {
        if (Vector3.Angle(transform.forward, dir) > (Mathf.PI * Mathf.Rad2Deg) / 2)
        {
            transform.forward = dir;
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, speedRot * Time.deltaTime);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    void InitializeFSM()
    {
        _fsm = new FSM<StateEnum>();

        _deathState = new EnemyDeathState<StateEnum>(this);
        _attackState = new EnemyAttackState<StateEnum>(this, player, damage, attackdelay);
        _goToBaseState = new EnemyGoToBaseState<StateEnum>(this.transform, player.transform, _start, _goal);
        _reachedBaseState = new EnemyReachedBase<StateEnum>(this);

        _goToBaseState.AddTransition(StateEnum.Attack, _attackState);
        _goToBaseState.AddTransition(StateEnum.Death, _deathState);
        _goToBaseState.AddTransition(StateEnum.ReachedBase, _reachedBaseState);
        _attackState.AddTransition(StateEnum.GoToBase, _goToBaseState);
        _attackState.AddTransition(StateEnum.Death, _deathState);
        

        _fsm.SetInit(_goToBaseState);
    }

    void InitializeTree()
    {
        var death = new ActionNode(() => _fsm.Transition(StateEnum.Death));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));
        var goToBase = new ActionNode(() => _fsm.Transition(StateEnum.GoToBase));
        var reachedBase = new ActionNode(() => _fsm.Transition(StateEnum.ReachedBase));

        var isEnemyAtBase = new QuestionNode(QuestionIsEnemyAtBase, reachedBase, goToBase);
        var isPlayerInRange = new QuestionNode(QuestionIsPlayerInRange, attack, isEnemyAtBase);
        var isDead = new QuestionNode(QuestionIsHealth0, death, isPlayerInRange);

        _tree = isDead;
    }


    bool QuestionIsHealth0()
    {
        if (currentHealth <= 0)
        {
            
            return true;

        }
        return false;
    }
    bool QuestionIsPlayerInRange()
    {
        //float distance = Vector3.Distance(transform.position, player.transform.position);
        //if (distance <= enemyData.range)
        //{
        //    return true;
        //}
        return false;
    }

    bool QuestionIsEnemyAtBase()
    {
        float distance = Vector3.Distance(transform.position, _goal.transform.position);
        if (distance <= 1.5f)
        {
            return true;
        }
        return false;
    }


    void Update()
    {
        _fsm.OnUpdate();
        _tree.Execute();

    }
}

