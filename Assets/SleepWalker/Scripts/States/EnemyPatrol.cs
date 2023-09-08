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
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void EnterState()
    {
        base.EnterState();
        if (nodeList.Count > 0)
            transform.position = nodeList[0].nodeTransform.position;

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < nodeList.Count - 1; ++i)
        {
            AppendToSequence(sequence, nodeList[i], nodeList[i+1]);
        }

        if (CanLoop())
        {
            AppendToSequence(sequence, nodeList[^1], nodeList[0]);
            sequence.SetLoops(-1);
        }
        else
        {
            sequence.SetLoops(-1, LoopType.Yoyo);
        }
        animator.SetFloat(AnimationHelper.SpeedParameter, 1f);
        sequence.Play();
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
        DOTween.KillAll();
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

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
