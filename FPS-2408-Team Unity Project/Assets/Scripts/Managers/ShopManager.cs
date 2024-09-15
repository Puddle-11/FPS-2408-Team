using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    private float warningTimer = 0;
    [SerializeField] private float warningHangTime;
    [SerializeField] private GameObject insuficientFunds;
    [Space]
    [Header("Pistols")]
    [Space]
    [SerializeField] private ItemType deagle;
    [SerializeField] private ItemType glock;
    [SerializeField] private ItemType shotgunPistol;
    [SerializeField] private ItemType classic;
    [SerializeField] private ItemType uzi;
    [Space]
    [Header("Assault Rifles")]
    [Space]
    [SerializeField] private ItemType mp5;
    [SerializeField] private ItemType scar;
    [SerializeField] private ItemType m4a1;
    [SerializeField] private ItemType mp7;
    [Space]
    [Header("Snipers")]
    [Space]
    [SerializeField] private ItemType m16;
    [Space]
    [Header("Items")]
    [Space]
    [SerializeField] private ItemType healthPack;

    private void Update()
    {
        if(warningTimer > 0)
        {
            warningTimer -= Time.unscaledDeltaTime;
            SetWarningState(true);

        }
        else
        {
            warningTimer = 0;
            SetWarningState(false);
        }

    }
    public void ResetWarningTimer()
    {
        SetWarningTimer( warningHangTime);
    }
    private void OnDisable()
    {
        warningTimer = 0;
    }
    public void SetWarningTimer(float _time)
    {
        warningTimer = _time;
    }
    public void SetWarningState(bool _val)
    {
        if(insuficientFunds != null) insuficientFunds.gameObject.SetActive(_val);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void OpenShop()
    {
        UIManager.instance.OpenShop();
    }

    public void CloseShop() 
    {
        UIManager.instance.CloseShop();
    }

    public void GunShop()
    {
        UIManager.instance.GunShop();
    }

    public void AmmoShop()
    {
        UIManager.instance.AmmoShop();
    }

    public void ItemShop()
    {
        UIManager.instance.ItemShop();
    }

    public void PrimaryShop()
    {
        UIManager.instance.PrimaryShop();
    }

    public void SecondaryShop() 
    {
        UIManager.instance.SecondaryShop();
    }

    public void SniperShop()
    {
        UIManager.instance.SniperShop();
    }

    //Ammo
    public void BuyPistolAmmo()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            AmmoInventory.instance.UpdateAmmoInventory(AmmoInventory.bulletType.Pistol, 20);
            ScrapInventory.instance.RemoveScrap(1);
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyAssaultAmmo()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            AmmoInventory.instance.UpdateAmmoInventory(AmmoInventory.bulletType.Assualt, 20);
            ScrapInventory.instance.RemoveScrap(1);
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyShotgunAmmo()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            AmmoInventory.instance.UpdateAmmoInventory(AmmoInventory.bulletType.Shotgun, 20);
            ScrapInventory.instance.RemoveScrap(1);
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuySniperAmmo()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            AmmoInventory.instance.UpdateAmmoInventory(AmmoInventory.bulletType.Sniper, 20);
            ScrapInventory.instance.RemoveScrap(1);
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyExplosiveAmmo()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            AmmoInventory.instance.UpdateAmmoInventory(AmmoInventory.bulletType.Explosive, 20);
            ScrapInventory.instance.RemoveScrap(1);
        }
        else
        {
            ResetWarningTimer();
        }
    }

    //Pistols

    public void BuyDeagle()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(deagle, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyGlock()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(glock, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyShotgunPistol()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(shotgunPistol, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyClassic()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(classic, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyUZI()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(uzi, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    //Snipers

    public void BuyM16()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(m16, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    //Assault Rifles

    public void BuyScar()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(scar, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyM4A1()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(m4a1, result);
                ScrapInventory.instance.RemoveScrap(1);
                Debug.Log(result);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyMP5()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(mp5, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyMP7()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(mp7, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }

    public void BuyHealthPack()
    {
        if (ScrapInventory.instance.currentScrap >= 1)
        {
            if (GeneralInventory.instance.GetNextFreeIndex(out int result))
            {
                GeneralInventory.instance.AddItemToInventory(healthPack, result);
                ScrapInventory.instance.RemoveScrap(1);
            }
        }
        else
        {
            ResetWarningTimer();
        }
    }
}
