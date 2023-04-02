using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Deposit elevatorDeposit;

    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform DepositLocation => depositLocation;
}
