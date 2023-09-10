using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPatrol : State
{
    private Orientation orientation;
    
    [Serializable]
    public class PatrolNode
    {
        public Transform nodeTransform;
        public float delay;
    }
    
    public enum PatrolType
    {
        BackAndForth,
        Loop
    }

    [BoxGroup("Patrol Variables")]
    public FloatReference speed;
    
    [BoxGroup("Patrol Variables")] 
    public PatrolType patrolType;

    [BoxGroup("Patrol Variables")] 
    [TableList]
    public List<PatrolNode> nodeList = new();

    private Rigidbody2D rb;
    private Animator animator;
    private Sequence patrolSequence;
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        orientation = GetComponent<Orientation>();
        
        if (nodeList.Count > 0)
            transform.position = nodeList[0].nodeTransform.position;

        patrolSequence = DOTween.Sequence();
        for (int i = 0; i < nodeList.Count - 1; ++i)
        {
            AppendToSequence(patrolSequence, nodeList[i], nodeList[i+1]);
        }

        if (CanLoop())
        {
            AppendToSequence(patrolSequence, nodeList[^1], nodeList[0]);
            patrolSequence.SetLoops(-1);
        }
        else
        {
            patrolSequence.SetLoops(-1, LoopType.Yoyo);
        }

        patrolSequence.Pause();
    }

    public override void EnterState()
    {
        base.EnterState();
        orientation.SetFacingMode(Orientation.FacingMode.Automatic);
        animator.SetFloat(AnimationHelper.SpeedParameter, 1f);
        patrolSequence.Play();
    }

    private void AppendToSequence(Sequence _sequence, PatrolNode _nodeA, PatrolNode _nodeB)
    {
        //Append the delay for the first node, if there is one
        if (_nodeA.delay > 0f)
            _sequence.AppendInterval(_nodeA.delay);

        Vector3 pos1 = _nodeA.nodeTransform.position;
        Vector3 pos2 = _nodeB.nodeTransform.position;
        float distance = Vector3.Distance(pos1, pos2);
        float time = distance / speed;
        
        _sequence.Append(rb.DOMove(pos2, time).SetEase(Ease.Linear));
    }

    public override void ExitState()
    {
        base.ExitState();
        patrolSequence.Pause();
        animator.SetFloat(AnimationHelper.SpeedParameter, 0f);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodeList.Count-1; ++i)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(nodeList[i].nodeTransform.position, nodeList[i+1].nodeTransform.position);
        }

        if (CanLoop())
        {
            Gizmos.DrawLine(nodeList[0].nodeTransform.position, nodeList[^1].nodeTransform.position);
        }
    }

    private bool CanLoop()
    {
        return patrolType == PatrolType.Loop && nodeList.Count > 1;
    }

    public override void Deactivate()
    {
        patrolSequence.Kill();
    }

    private void OnDestroy()
    {
        patrolSequence.Kill();
    }
}
