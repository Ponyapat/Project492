using System;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;
    
    [SerializeField] private TextMeshProUGUI depositGold;
    [SerializeField] private TextMeshProUGUI shaftID;
    [SerializeField] private TextMeshProUGUI shaftLevel;
    [SerializeField] private TextMeshProUGUI newShaftCost;
    [SerializeField] private GameObject newShaftButton;
    
    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;
    
    private void Awake()
    {
        _shaft = GetComponent<Shaft>();
        _shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    private void Update()
    {
        depositGold.text = _shaft.ShaftDeposit.CurrentGold.ToString();
    }

    public void AddShaft()
    {
        if (GoldManager.Instance.CurrentGold >= ShaftManager.Instance.ShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.ShaftCost);
            ShaftManager.Instance.AddShaft();
            newShaftButton.SetActive(false);
        }
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade);
    }
    
    public void SetShaftUI(int ID)
    {
        _shaft.ShaftID = ID;
        shaftID.text = "B-" + (ID + 1).ToString();
    }

    public void SetNewShaftCost(float newCost)
    {
        newShaftCost.text = newCost.ToString();
    }

    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_shaftUpgrade == upgrade)
        {
            shaftLevel.text = upgrade.CurrentLevel.ToString();
        }
    }
    
    private void OnEnable()
    {
        ShaftUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    private void OnDisable()
    {
        ShaftUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
