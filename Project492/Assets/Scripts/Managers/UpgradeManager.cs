using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeContainer;
    [SerializeField] private Image panelMinerImage;
    [SerializeField] private TextMeshProUGUI panelTitle;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI nextBoost;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Image progressBar;

    [Header("Upgrade Buttons")]
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private Color buttonDisableColor;
    [SerializeField] private Color buttonEnableColor;
    
    [Header("Stat Title")] 
    [SerializeField] private TextMeshProUGUI stat1Title;
    [SerializeField] private TextMeshProUGUI stat2Title;
    [SerializeField] private TextMeshProUGUI stat3Title;
    [SerializeField] private TextMeshProUGUI stat4Title;
    
    [Header("Stat Values")] 
    [SerializeField] private TextMeshProUGUI stat1CurrentValue;
    [SerializeField] private TextMeshProUGUI stat2CurrentValue;
    [SerializeField] private TextMeshProUGUI stat3CurrentValue;
    [SerializeField] private TextMeshProUGUI stat4CurrentValue;

    [Header("Stat Upgrade Values")] 
    [SerializeField] private TextMeshProUGUI stat1UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat2UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat3UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat4UpgradeValue;
    
    [Header("Stat Icon")] 
    [SerializeField] private Image stat1Icon;
    [SerializeField] private Image stat2Icon;
    [SerializeField] private Image stat3Icon;
    [SerializeField] private Image stat4Icon;

    [Header("Panel Info")] 
    [SerializeField] private UpgradePanelInfo shaftMinerInfo;

    public int UpgradeAmount { get; set; }
    
    private Shaft _currentShaft;
    private ShaftUpgrade _currentShaftUpgrade;
    private int _currentActiveButton;
    
    public void OpenCloseUpgradeContainer(bool status)
    {
        UpgradeX1();
        upgradeContainer.SetActive(status);
    }

    public void Upgrade()
    {
        if (GoldManager.Instance.CurrentGold >= _currentShaftUpgrade.UpgradeCost)
        {
            _currentShaftUpgrade.Upgrade(UpgradeAmount);
            UpdateShaftPanelValues();
            RefreshUpgradeAmount();
        }
    }

    #region Upgrade Buttons

    public void UpgradeX1()
    {
        ActivateButton(0);
        UpgradeAmount = CanUpgradeManyTimes(1, _currentShaftUpgrade) ? 1 : 0;
        upgradeCost.text = GetUpgradeCost(1, _currentShaftUpgrade).ToString();
    }

    public void UpgradeX10()
    {
        ActivateButton(1);
        UpgradeAmount = CanUpgradeManyTimes(10, _currentShaftUpgrade) ? 10 : 0;
        upgradeCost.text = GetUpgradeCost(10, _currentShaftUpgrade).ToString();
    }
    
    public void UpgradeX50()
    {
        ActivateButton(2);
        UpgradeAmount = CanUpgradeManyTimes(50, _currentShaftUpgrade) ? 50 : 0;
        upgradeCost.text = GetUpgradeCost(50, _currentShaftUpgrade).ToString();
    }
    
    public void UpgradeMax()
    {
        ActivateButton(3);
        UpgradeAmount = CalculateUpgradeCount(_currentShaftUpgrade);
        upgradeCost.text = GetUpgradeCost(UpgradeAmount, _currentShaftUpgrade).ToString();
    }

    private int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        int count = 0;
        float currentGold = GoldManager.Instance.CurrentGold;
        float currentUpgradeCost = upgrade.UpgradeCost;

        if (GoldManager.Instance.CurrentGold >= currentUpgradeCost)
        {
            for (float i = currentGold; i >= 0; i -= currentUpgradeCost)
            {
                count++;
                currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
            }
        }

        return count;
    }

    private bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        if (count >= upgradeAmount)
        {
            return true;
        }

        return false;
    }

    private float GetUpgradeCost(int amount, BaseUpgrade upgrade)
    {
        float cost = 0f;
        float currentUpgradeCost = upgrade.UpgradeCost;

        for (int i = 0; i < amount; i++)
        {
            cost += currentUpgradeCost;
            currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
        }

        return cost;
    }
    
    private void ActivateButton(int buttonIndex)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
        }

        _currentActiveButton = buttonIndex;
        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;
        upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition +
                                                              new Vector3(0f, -5f, 0f), 0.5f).Play();
    }

    private void RefreshUpgradeAmount()
    {
        switch (_currentActiveButton)
        {
            case 0:
                UpgradeX1();
                break;
            case 1:
                UpgradeX10();
                break;
            case 2:
                UpgradeX50();
                break;
            case 3:
                UpgradeMax();
                break;
        }
    }
    
    #endregion
    
    private void UpdateUpgradeInfo()
    {
        panelTitle.text = shaftMinerInfo.PanelTitle;
        panelMinerImage.sprite = shaftMinerInfo.PanelMinerIcon;

        stat1Title.text = shaftMinerInfo.Stat1Title;
        stat2Title.text = shaftMinerInfo.Stat2Title;
        stat3Title.text = shaftMinerInfo.Stat3Title;
        stat4Title.text = shaftMinerInfo.Stat4Title;

        stat1Icon.sprite = shaftMinerInfo.Stat1Icon;
        stat2Icon.sprite = shaftMinerInfo.Stat2Icon;
        stat3Icon.sprite = shaftMinerInfo.Stat3Icon;
        stat4Icon.sprite = shaftMinerInfo.Stat4Icon;
    }

    private void UpdateShaftPanelValues()
    {
        upgradeCost.text = _currentShaftUpgrade.UpgradeCost.ToString();
        level.text = $"Level {_currentShaftUpgrade.CurrentLevel}";
        progressBar.DOFillAmount(_currentShaftUpgrade.GetNextBoostProgress(), 0.5f).Play();
        nextBoost.text = $"Next Boost at Level {_currentShaftUpgrade.BoostLevel}";
        
        // Current Values
        stat1CurrentValue.text = $"{_currentShaft.Miners.Count}";
        stat2CurrentValue.text = $"{_currentShaft.Miners[0].MoveSpeed}";
        stat3CurrentValue.text = $"{_currentShaft.Miners[0].CollectPerSecond}";
        stat4CurrentValue.text = $"{_currentShaft.Miners[0].CollectCapacity}";
        
        // Miner stat
        stat1UpgradeValue.text = (_currentShaftUpgrade.CurrentLevel + 1) % 10 == 0 ? "+1" : "+0";

        // Move Speed
        float minerMoveSpeed = _currentShaft.Miners[0].MoveSpeed;
        float walkSpeedUpgraded = Mathf.Abs(minerMoveSpeed - (minerMoveSpeed * _currentShaftUpgrade.MoveSpeedMultiplier));
        stat2UpgradeValue.text = (_currentShaftUpgrade.CurrentLevel + 1) % 10 == 0 ? $"+{walkSpeedUpgraded}/s" : "+0";

        // Collect Per Second
        float minerCollectPerSecond = _currentShaft.Miners[0].CollectPerSecond;
        float collectPerSecondUpgraded = Mathf.Abs(minerCollectPerSecond - (minerCollectPerSecond * _currentShaftUpgrade.CollectPerSecondMultiplier));
        stat3UpgradeValue.text = $"+{collectPerSecondUpgraded}";
        
        // Collect Capacity
        float minerCollectCapacity = _currentShaft.Miners[0].CollectCapacity;
        float collectCapacityUpgraded = Mathf.Abs(minerCollectCapacity - (minerCollectCapacity * _currentShaftUpgrade.CollectCapacityMultiplier));
        stat4UpgradeValue.text = $"+{collectCapacityUpgraded}";
    }
    
    private void ShaftUpgradeRequest(Shaft selectedShaft, ShaftUpgrade selectedUpgrade)
    {
        _currentShaft = selectedShaft;
        _currentShaftUpgrade = selectedUpgrade;
        
        UpdateUpgradeInfo();
        UpdateShaftPanelValues();   
        OpenCloseUpgradeContainer(true);
    }
    
    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
    }
}