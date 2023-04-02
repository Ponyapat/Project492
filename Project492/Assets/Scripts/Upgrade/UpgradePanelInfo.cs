using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Info")]
public class UpgradePanelInfo : ScriptableObject
{
    public string PanelTitle;
    public Sprite PanelMinerIcon;

    [Header("Stat Title")] 
    public string Stat1Title;
    public string Stat2Title;
    public string Stat3Title;
    public string Stat4Title;
    
    [Header("Stat Icon")] 
    public Sprite Stat1Icon;
    public Sprite Stat2Icon;
    public Sprite Stat3Icon;
    public Sprite Stat4Icon;
}
