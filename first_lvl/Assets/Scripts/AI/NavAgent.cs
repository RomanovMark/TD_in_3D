using UnityEngine;
using UnityEngine.AI;

enum CurrentState
{
    Moving,
    Done
}

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{
    // Variables in inspector
    public AIWaypointNetwork WaypointNetwork;
    public int CurrentIndex = 0;
    public bool HasPath;
    public bool PathPending;
    public bool PathStale;
    public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;
    CurrentState currentState;

    // Buffer the NavMeshAgent
    private NavMeshAgent _navAgent;

    // In Start get all needed values
    private void Start()
    {
        // Get current character NavMeshAgent
        _navAgent = GetComponent<NavMeshAgent>();

        // Check current waypoint network is assigmet
        if (WaypointNetwork == null)
            return;

        // Call method next destination
        SetNextDestination(false);
    }

    /// <summary>
    /// Set next destination point.
    /// Increments the current waypoint index and set the next destination for the agent
    /// </summary>
    /// <param name="increment">Need to increment?</param>
    private void SetNextDestination(bool increment)
    {
        // If network is not exists, stop method
        if (!WaypointNetwork)
            return;

        // Calculate how much the current waypoints index needs to be incremented
        int incrementStep = (increment) ? 1 : 0;
        int nextWaypoint;

        // Calculate index of next waypoint
         if (CurrentIndex + incrementStep >= WaypointNetwork.waypoints.Count)
        {
            nextWaypoint = CurrentIndex;
            currentState = CurrentState.Done;
        }
         else
        {
            nextWaypoint = CurrentIndex + incrementStep;
        }
         
        Transform nextWaypointTransform = WaypointNetwork.waypoints[nextWaypoint];

        // Assigne a valid waypoint transform
        if (nextWaypointTransform != null)
        {
            // Update current position
            CurrentIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }

        // If not valid waypoint, increment to next
        CurrentIndex++;
    }

    private void Update()
    {
        // Set values to inspector from agent
        HasPath = _navAgent.hasPath;
        PathPending = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        PathStatus = _navAgent.pathStatus;

        // If don't have a path - pending then set the next
        // otherwise if path is stale regenerate path
        if (!currentState.Equals(CurrentState.Done) && !GameManager.instance.IsPaused && !GameManager.instance.IsGameOver)
        {
            if (_navAgent.isStopped)
                _navAgent.isStopped = false;

            if ((_navAgent.remainingDistance <= _navAgent.stoppingDistance && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid)
                SetNextDestination(true);
            else if (_navAgent.isPathStale)
                SetNextDestination(false);
        }
        else
        {
            _navAgent.isStopped = true;
        }
    }
}
