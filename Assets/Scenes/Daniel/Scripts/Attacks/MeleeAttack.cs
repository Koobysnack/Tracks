using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float attackDamage;

    void Start()
    {
        attackDamage = 0;
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
       
    }

}
