using UnityEngine;
using UnityEngine.AI;

public class BatAI : MonoBehaviour
{
    // The brain states of our bat (it has moods too)
    public enum BatState
    {
        Idle,
        Alert,
        Chase,
        Search,
        Respawn
    }
    public ItemPickup item;
    public BatState currentState;

    private NavMeshAgent agent;
    private Transform player;

    [Header("Vision Settings")]
    public float viewDistance = 10f;
    public float viewAngle = 60f;

    [Header("Hearing Settings")]
    public float hearingRange = 8f;

    [Header("Search Settings")]
    private Vector3 lastKnownPosition;
    public float searchTime = 5f;
    private float searchTimer;

    [Header("Respawn")]
    private Vector3 spawnPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoint = transform.position;

        currentState = BatState.Idle;
    }

    void Update()
    {

        {
            // State machine magic happens here
            switch (currentState)
            {
                case BatState.Idle:
                    Idle();
                    break;

                case BatState.Alert:
                    Alert();
                    break;

                case BatState.Chase:
                    Chase();
                    break;

                case BatState.Search:
                    Search();
                    break;

                case BatState.Respawn:
                    Respawn();
                    break;
            }
        }
        void Idle()
        {
            // Just vibing... until something suspicious happens
            agent.SetDestination(transform.position);

            if (CanSeePlayer() || CanHearPlayer())
            {
                currentState = BatState.Alert;
            }
        }

        void Alert()
        {
           
                // "I heard something..." — moving to investigate
                agent.SetDestination(player.position);

            if (CanSeePlayer())
            {
                currentState = BatState.Chase;
            }
            else
            {
                lastKnownPosition = player.position;
                currentState = BatState.Search;
            }
        }

        void Chase()
        {
            // Target locked. No chill.
            agent.SetDestination(player.position);

            if (!CanSeePlayer())
            {
                lastKnownPosition = player.position;
                currentState = BatState.Search;
            }
        }

        void Search()
        {
            // "I swear they were right here..."
            agent.SetDestination(lastKnownPosition);

            searchTimer += Time.deltaTime;

            if (CanSeePlayer())
            {
                currentState = BatState.Chase;
            }
            else if (searchTimer >= searchTime)
            {
                searchTimer = 0;
                currentState = BatState.Idle;
            }
        }

        void Respawn()
        {
            // Teleporting back like nothing happened
            agent.ResetPath(); // stops movement instantly
            transform.position = spawnPoint;
            currentState = BatState.Idle;
        }

        bool CanSeePlayer()
        {
            // Check distance first so we’re not doing extra work
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);

            // Make sure the player is actually in front of us (no eyes in the back of the head)
            if (distance < viewDistance)
            {
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle < viewAngle)
                {
                    // Raycast = "is there a wall in the way?"
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, direction, out hit, viewDistance))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool CanHearPlayer()
        {
            float distance = Vector3.Distance(transform.position, player.position);
            return distance < hearingRange;
        }
    }

    public void OnPlayerNoise(Vector3 noisePosition)
    {
        lastKnownPosition = noisePosition;
        currentState = BatState.Alert;
    }

    public void Die()
    {
        currentState = BatState.Respawn;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (item != null && item.IsHeld())
            {
                item.ResetItem();
            }

            currentState = BatState.Respawn;
        }
    }

}