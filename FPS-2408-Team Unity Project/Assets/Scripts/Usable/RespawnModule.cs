using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;


public class RespawnModule : UsableItemBase     
{
    [SerializeField] private float healthRegenPercent;

    public override string GetItemStats()
    {
        return "Regenerate " + healthRegenPercent * 100 + "% Upon death";
    }
    public override void UseItem()
    {
    }

}
