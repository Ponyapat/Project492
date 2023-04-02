using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldManager : Singleton<GoldManager>
{
    [SerializeField] private float testGold = 0;
    [SerializeField] private float mintRate = 0;
    [SerializeField] private string URL;

    [Header("Button")]
    [SerializeField] private Button mintButton;

    public float CurrentGold { get; set; }
    private readonly string GOLD_KEY = "MY_GOLD";

    private void Start()
    {
        mintButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = mintRate.ToString();
        PlayerPrefs.DeleteAll();
        LoadGold();
    }

    private void LoadGold()
    {
        CurrentGold = PlayerPrefs.GetFloat(GOLD_KEY, testGold);
        CheckGoldRate();
    }
    
    public void AddGold(float amount)
    {
        CurrentGold += amount;
        CheckGoldRate();
        PlayerPrefs.SetFloat(GOLD_KEY, CurrentGold);
        PlayerPrefs.Save();
    }

    public void RemoveGold(float amount)
    {
        if (amount <= CurrentGold)
        {
            CurrentGold -= amount;
            PlayerPrefs.SetFloat(GOLD_KEY, CurrentGold);
            PlayerPrefs.Save();
        }
        CheckGoldRate();
    }

    void CheckGoldRate()
    {
        if (CurrentGold >= mintRate)
        {
            mintButton.onClick.AddListener(MintGold);
            mintButton.interactable = true;
        }
        else
        {
            mintButton.onClick.RemoveListener(MintGold);
            mintButton.interactable = false;
        }
    }

    private void MintGold()
    {
        if (URL != "")
        {
            RemoveGold(mintRate);
            Application.OpenURL(URL);
        }
    }
}