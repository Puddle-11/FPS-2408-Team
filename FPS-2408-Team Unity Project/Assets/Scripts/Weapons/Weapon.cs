using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IUsable
{
    protected bool usingItem;
    [SerializeField] protected float coolDown;
    protected bool isAttacking;
    public LayerMask ignoreMask;
    [SerializeField] protected GameObject pickUp;
    [SerializeField] protected ItemType gunDrop;


    public void SetPickup(GameObject _pickup)
    {
        pickUp = _pickup;
    }
    public GameObject GetPickup()
    {
        if (pickUp != null)
        {
            return pickUp;
        }
        else if (gunDrop.Pickup != null)
        {
            return gunDrop.Pickup;
        }
        else
        {
            Debug.LogWarning("No pickup available on " + gameObject.name + "\nWith an Item type of + " + gunDrop.name);
            return null;
        }
    }

    public bool GetUsingItem()
    {
        return usingItem;
    }
    public void SetUsingItem(bool _val)
    {
        usingItem = _val;
    }
    public void UseItem()
    {
        Attack();
    }
  
    public bool GetIsAttacking()
    {
        return isAttacking;
    }
    public virtual void Attack()
    {

        if (!isAttacking)
        {
            StartCoroutine(AttackDelay()); 
        }
    }
    private void OnDisable()
    {
        isAttacking = false;
    }
    public virtual IEnumerator AttackDelay()
    {
        isAttacking = true;
        //begin attack
        yield return new WaitForSeconds(coolDown);
        isAttacking = false;
    }
}
