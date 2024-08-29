using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    public static AmmoInventory instance;

    [Header("Inventory Variables")]
    [Space]
    [SerializeField] private int numOfAmmoTypes = 5;
    public int[] ammoCounts = new int[5];
    public Sprite[] typeIcons = new Sprite[5];
    public enum bulletType
    {
        Pistol,
        Assualt,
        Shotgun,
        Sniper,
        Explosive
    }

    public string GetTypeName(bulletType _type)
    {
        switch (_type)
        {
            case bulletType.Pistol:
                return "Pistol";
            case bulletType.Assualt:
                return "Assualt";
            case bulletType.Shotgun:
                return "Shotgun";
            case bulletType.Sniper:
                return "Sniper";
            case bulletType.Explosive:
                return "Explosive";
        }
        return "";
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }
    public int GetAmmoTypeCount()
    {
        return numOfAmmoTypes;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetAmmoAmount(bulletType type)
    {
        return GetAmmoAmount((int)type);
    }
    public int GetAmmoAmount(int _index)
    {
        return ammoCounts[_index];

    }
    public Sprite GetAmmoIcon(bulletType type)
    {
        return GetAmmoIcon((int)type);
    }
    public Sprite GetAmmoIcon(int _index)
    {
        if (_index < typeIcons.Length)
        {
            return typeIcons[_index];
        }
        else
        {
            return null;
        }

    }
    public void UpdateAmmoInventory(bulletType type, int amount)
    {
        ammoCounts[(int)type] += amount;
        ItemType itRef = GameManager.instance.playerControllerRef.GetCurrentItemType();
        if (itRef != null)
        {
            BaseGun basegRef = itRef.Object.GetComponent<BaseGun>();
            if (basegRef.GetAmmoType() == type)
            {
                UIManager.instance.UpdateCurrInvAmmo(type);
            }
        }
    }
}
