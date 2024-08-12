using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private GameObject handAnchor;
    [SerializeField] private float pickUpDist;
    private GameObject CurrentEquiped;
    [SerializeField] private LayerMask ignoreMask;
    private Pickup CurrentPickup;
    [SerializeField] private Vector2 throwSpeed;
    [SerializeField] private Vector2 throwOffset;
    public void SetUseItem(bool _val)
    {
        IUsable itemRef;
        if (CurrentEquiped != null && CurrentEquiped.TryGetComponent<IUsable>(out itemRef))
        {
            itemRef.SetUsingItem(_val);
        }
    }
    public bool GetUseItem()
    {
        IUsable itemRef;
        if (CurrentEquiped != null && CurrentEquiped.TryGetComponent<IUsable>(out itemRef))
        {
            return itemRef.GetUsingItem();
        }
        return false;
    }
    public GameObject GetCurrentHand()
    {

        return CurrentEquiped;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pick Up") && !AttemptPickup())
        {
            AttemptDrop();
        }
       
    }
    public void UseItem()
    {
        IUsable useRef;
        if (CurrentEquiped != null)
        {
            if (CurrentEquiped.TryGetComponent<IUsable>(out useRef))
            {

                useRef.UseItem();
            }
        }
    }
    private bool AttemptDrop()
    {
        if(CurrentEquiped != null)
        {

            Destroy(CurrentEquiped);
            CurrentEquiped = null;
            CurrentPickup.DropItem(transform.position + transform.forward * throwOffset.x + new Vector3(0,throwOffset.y,0), Camera.main.transform.forward *throwSpeed.x+ Vector3.up * throwSpeed.y, 1);
            return true;
        }
        return false;
    }
    private bool AttemptPickup()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDist, ~GameManager.instance.projectileIgnore))
        {
            Pickup objectPickupRef;
            if (hit.transform.TryGetComponent<Pickup>(out objectPickupRef) && objectPickupRef.GetItem() != null)
            {
                AttemptDrop(); //attempt a drop before picking up a new item
                CurrentEquiped = Instantiate(objectPickupRef.PickupItem().Object, handAnchor.transform.position, handAnchor.transform.rotation, handAnchor.transform);
                BaseGun BGref;
                if(CurrentEquiped.TryGetComponent<BaseGun>(out BGref))
                {
                    BGref.SetPlayerGun(true);
                }
                CurrentPickup = objectPickupRef;

                return true;
            }
        }
        return false;
    }
}
