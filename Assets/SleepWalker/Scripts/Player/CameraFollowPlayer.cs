using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset = Vector3.zero; //for z offset and offset to follow mouse pointer
    [SerializeField] float damp = 0.3f;
    [SerializeField] float dirOffset = 1f;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // fixed update coz update makes it all snappy
    void FixedUpdate()
    {
        //offset ensure player is in front of camera, and that camera follows pointer slightly
        offset = (dirOffset*FindMouseDirection());
        //offset = Vector3.zero; //take this out when you fix the other offset
        offset.z = -10f;

        //transform.position = Vector3.Lerp(transform.position, target.position, damp) + offset;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, damp);
    }

    Vector3 FindMouseDirection()
    {
        Vector3 mouseScreenPosition;
        Vector3 mouseWorldPosition;

        //get mouse pointer position
        mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0;

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        //find direction from current player to mouse
        Vector3 mouseDirection = (mouseWorldPosition - target.transform.position).normalized;

        return mouseDirection;
    }
}
