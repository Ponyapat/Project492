using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftMiner : BaseMiner
{
    public Shaft CurrentShaft { get; set; }

    public Vector3 DepositLocation => new Vector3(CurrentShaft.DepositLocation.position.x, transform.position.y);
    public Vector3 MiningLocation => new Vector3(CurrentShaft.MiningLocation.position.x, transform.position.y);
    
    private int walkAnimation = Animator.StringToHash("Walk");
    private int miningAnimation = Animator.StringToHash("Mining");

    public override void OnClick()
    {
        MoveMiner(MiningLocation);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkAnimation);
    }

    protected override void CollectGold()
    {
        _animator.SetTrigger(miningAnimation);
        float collectTime = CollectCapacity / CollectPerSecond;
        StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        CurrentGold = gold;
        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(DepositLocation);
    }

    protected override void DepositGold()
    {
        CurrentShaft.ShaftDeposit.DepositGold(CurrentGold);
        
        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(MiningLocation);
    }
}
