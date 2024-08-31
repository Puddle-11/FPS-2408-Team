using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : BaseEntity
{
    [SerializeField] private float RespawnTime;
    private Collider collRef;
    public override void Awake()
    {
        base.Awake();
        if (!TryGetComponent<Collider>(out collRef))
        {
            Debug.LogWarning("Failed to fetch Collider from " + gameObject.name);
        }
    }
    public override void Death()
    {
        StartCoroutine(RespawnDelay());
    }
    public IEnumerator RespawnDelay()
    {
        if (collRef == null || rendRef == null)
        {
            Debug.LogWarning("Target Respawn delay failed \nCollider or Renderer were unassigned\nDestroying object...");
            base.Death();
            yield break;
        }
        collRef.enabled = false;
        for (int i = 0; i < rendRef.Length; i++)
        {
            rendRef[i].currRenderer.enabled = false;
        }
        yield return new WaitForSeconds(RespawnTime);
        ResetHealth();
        collRef.enabled = true;
        for (int i = 0; i < rendRef.Length; i++)
        {
            rendRef[i].currRenderer.enabled = true;
        }
    }

}
