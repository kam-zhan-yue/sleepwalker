using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    //this will be all changed



    [SerializeField, ReadOnly] Vector3 desiredPos;

    
    [SerializeField] Vector3 zOffset = new Vector3(0f,0f,-20f);
    bool shaking = false;

    [SerializeField] float damp = 0.3f;
    [SerializeField] float maxShakeDistance = 1f;
    [SerializeField] float thresholdDistance = 0.1f;
    [SerializeField] float defaultShakeDuration = 0.75f;

    Vector2[] debugLineStarts;
    Vector2[] debugLineEnds;
    int currentDebugLine = 0;

    // Start is called before the first frame update
    void Start()
    {
        //initialise debug lines
        int noOfLines = 5;
        debugLineStarts = new Vector2[noOfLines];
        debugLineEnds = new Vector2[noOfLines];

        for (int i = 0; i < debugLineStarts.Length; i++)
        {
            debugLineStarts[i] = Vector3.zero;
            debugLineEnds[i] = Vector3.zero;
        }

        desiredPos = transform.parent.position;
        shaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            //find next desired position
            if (Vector2.Distance(transform.parent.position, desiredPos) < thresholdDistance)
            {
                desiredPos = FindNextShakePosition();
            }
        } else
        {
            //reset to centre
            desiredPos = transform.parent.position + zOffset;
        }

        //update position
        transform.position = Vector3.Lerp(transform.parent.position, desiredPos + zOffset, damp);

        //draw debug lines
        for (int i = 0; i < debugLineStarts.Length; i++)
        {
            Debug.DrawLine(debugLineStarts[i], debugLineEnds[i], Color.yellow);
        }
    }

    Vector3 FindNextShakePosition()
    {
        //random for now, if I had more time I'd make it do it across from the past one
        Vector3 nextPos = transform.parent.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
        nextPos = nextPos.normalized;
        nextPos *= Random.Range(maxShakeDistance/2f, maxShakeDistance);

        //debug lines
        debugLineStarts[currentDebugLine] = desiredPos;
        debugLineEnds[currentDebugLine] = nextPos;

        currentDebugLine++;
        if (currentDebugLine >= debugLineStarts.Length)
        {
            currentDebugLine = 0;
        }

        return nextPos;
    }

    [Button]
    public void StartShake(float shakeDuration = -1)
    {
        if (shakeDuration < 0)
        {
            shakeDuration = defaultShakeDuration;
        }

        IEnumerator coroutine = DuringShake(shakeDuration);
        StartCoroutine(coroutine);
    }

    IEnumerator DuringShake(float waitTime)
    {
        shaking = true;
        Debug.Log($"Shaking: {shaking}");

        yield return new WaitForSeconds(waitTime);

        shaking = false;
        Debug.Log($"Shaking: {shaking}");
    }
}
