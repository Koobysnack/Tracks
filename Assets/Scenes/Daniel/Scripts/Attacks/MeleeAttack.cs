using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float attackDamage;
    public float attackRange;
    public Vector3 meleeBoxCenter;
    public Vector3 meleeBoxHalfExtents;
    

    void Start()
    {
        attackDamage = 0;
        if (meleeBoxCenter==null)
        {
            meleeBoxCenter = transform.position;

        }
        if (meleeBoxHalfExtents==null)
        {
            meleeBoxHalfExtents = new Vector3(1, 1, 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void SetAttack(float damage)
    {
        attackDamage = damage;
    }

    public void ShovelAttack()
    {
        RaycastHit smackedObj;
        EntityController ShotEntity;
        Physics.BoxCast(meleeBoxCenter,meleeBoxHalfExtents,transform.TransformDirection(Vector3.forward), out smackedObj ,Quaternion.identity,attackRange);
        Debug.DrawRay(meleeBoxCenter, transform.TransformDirection(Vector3.forward), Color.blue);
        if (smackedObj.transform == null || smackedObj.transform.parent == null)
            return;
       

        if (smackedObj.transform.parent.TryGetComponent<EntityController>(out ShotEntity))
        {
            ShotEntity.Damage(attackDamage, transform);

        }
    }

}
