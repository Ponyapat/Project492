using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI elevatorDepositGoldText;

    private Elevator _elevator;
    
    private void Start()
    {
        _elevator = GetComponent<Elevator>();
    }

    
    private void Update()
    {
        elevatorDepositGoldText.text = _elevator.ElevatorDeposit.CurrentGold.ToString();
    }
}
