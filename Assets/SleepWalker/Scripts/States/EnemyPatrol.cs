using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
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
    public float speed = 0f;
    
    [BoxGroup("Patrol Variables")] 
    public PatrolType patrolType;

    [BoxGroup("Patrol Variables")] 
    [TableList]
    public List<PatrolNode> nodeList = new();

    private Rigidbody2D rb;
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        
        sequence.Play();
    }

    private void AppendToSequence(Sequence sequence, PatrolNode nodeA, PatrolNode nodeB)
    {
        //Append the delay for the first node, if there is one
        if (nodeA.delay > 0f)
            sequence.AppendInterval(nodeA.delay);

        Vector3 pos1 = nodeA.nodeTransform.position;
        Vector3 pos2 = nodeB.nodeTransform.position;
        float distance = Vector3.Distance(pos1, pos2);
        float time = distance / speed;
        
        sequence.Append(rb.DOMove(pos2, time).SetEase(Ease.Linear));
    }

    public override void ExitState()
    {
        base.ExitState();
        DOTween.KillAll();
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
}
