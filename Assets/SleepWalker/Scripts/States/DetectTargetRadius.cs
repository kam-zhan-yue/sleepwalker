using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public sealed class DetectTargetRadius : Decision
{
    [BoxGroup("Setup Variables")] public float radius = 3f;
    [BoxGroup("Setup Variables")] public LayerMask targetLayerMask;
    [BoxGroup("Setup Variables")] public bool canTargetSelf;
    [BoxGroup("Setup Variables")] public bool obstacleDetection;
    [BoxGroup("Setup Variables")] public int overlapMaximum = 10;


    private readonly LayerMask obstacleLayerMask = LayerHelper.obstaclesLayerMask;
    private Collider2D[] results;
    private List<Transform> potentialTargets = new();
    private RaycastHit2D[] hits = new RaycastHit2D[10];
    
    protected override void Awake()
    {
        base.Awake();
        results = new Collider2D[overlapMaximum];
    }
    
    protected override bool CanActivate()
    {
        potentialTargets.Clear();
        int numberOfResults = Physics2D.OverlapCircleNonAlloc(transform.position, radius, results, targetLayerMask);
        // Debug.Log(numberOfResults);
        if (numberOfResults <= 0)
            return false;
        int min = Mathf.Min(overlapMaximum, numberOfResults);
        for (int i = 0; i < min; ++i)
        {
            if (results[i] == null)
            {
                continue;
            }
            if (!canTargetSelf)
            {
                if ((results[i].gameObject == gameObject) || (results[i].transform.IsChildOf(transform)))
                {
                    continue;
                }    
            }
            potentialTargets.Add(results[i].gameObject.transform);
        }
        
        //If there are no targets, then return false
        if (potentialTargets.Count <= 0)
            return false;
        
        
        // we sort our targets by distance
        potentialTargets.Sort(delegate(Transform _a, Transform _b)
        {
            if (_a == null || _b == null)
            {
                return 0;
            }
                
            return Vector2.Distance(transform.position,_a.transform.position)
                .CompareTo(
                    Vector2.Distance(transform.position,_b.transform.position) );
        });
        
        //If there is an obstacle, then go through all potential targets
        if (obstacleDetection)
        {
            for (int i = 0; i < potentialTargets.Count; ++i)
            {
                //Shoot a ray to each target to see if there are any obstacles
                Vector3 startPosition = transform.position;
                Vector3 endPosition = potentialTargets[i].position;
                int hitCount = Physics2D.RaycastNonAlloc(startPosition, endPosition - startPosition, hits, Vector2.Distance(startPosition, endPosition), obstacleLayerMask);
                // Debug.Log("Hit Count: "+hitCount);
                //If there are no hits, then return the target
                if (hitCount == 0)
                {
                    float distance = transform.DistanceToObject(potentialTargets[i].gameObject.transform);
                    // Debug.Log("Hit with Distance: "+distance);
                    brain.target = potentialTargets[i].gameObject.transform;
                    return true;
                }
                // else
                // {
                //     Debug.Log(hits[0].collider.gameObject.name);
                // }
            }
        }
        //If there is no obstacle detection and there is a target, then can decide
        else if (potentialTargets[0] != null)
        {
            brain.target = potentialTargets[0].gameObject.transform;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Draws gizmos for the detection circle
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
