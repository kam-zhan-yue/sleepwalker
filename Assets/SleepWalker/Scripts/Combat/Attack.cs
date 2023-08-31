using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [BoxGroup("Setup Variables")] 
    public Damage.Direction damageDirection = Damage.Direction.None;
    
    [BoxGroup("Setup Variables")] 
    public float initialDelay = 0f;

    [BoxGroup("Setup Variables")] 
    public LayerMask targetLayerMask;
    
    [BoxGroup("Setup Variables")] 
    public Vector2 areaOffset;
    
    [BoxGroup("Setup Variables")] 
    public Vector2 areaSize;
    
    [BoxGroup("Setup Variables")] 
    public float damage = 0f;
    
    //Damage Area Variables
    private GameObject damageArea;
    private BoxCollider2D damageAreaCollider;
    private DamageBox damageBox;
    
    private void Awake()
    {
        damageArea = new GameObject();
        damageArea.name = name + "DamageArea";
        Transform transform1 = transform;
        damageArea.transform.position = transform1.position;
        damageArea.transform.rotation = transform1.rotation;
        damageArea.transform.SetParent(transform1);
        damageArea.transform.localScale = Vector3.one;
        damageArea.layer = gameObject.layer;

        damageAreaCollider = damageArea.AddComponent<BoxCollider2D>();
        damageAreaCollider.offset = areaOffset;
        damageAreaCollider.size = areaSize;
        damageAreaCollider.isTrigger = true;
        
        Rigidbody2D rb = damageArea.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        damageBox = damageArea.AddComponent<DamageBox>();
        damageBox.targetLayerMask = targetLayerMask;
        damageBox.damage = damage;
        damageBox.owner = gameObject;
        damageBox.boxCollider2D = damageAreaCollider;
        damageBox.direction = damageDirection;
    }

    private void OnDrawGizmos()
    {
        StaticHelper.DrawGizmoRectangle(transform.position + (Vector3)areaOffset, areaSize, Color.red);
    }
}
