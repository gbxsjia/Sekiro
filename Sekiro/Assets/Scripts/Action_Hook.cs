using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Hook : Action_Base
{
    private float ShotSpeed; // 抛出的速度
    public float time = 1;          // A-B的时间
    public Transform pointA;     // 起点
    public Transform pointB;     // 终点
    public float g = -10;        // 重力加速度
    private Vector3 speed;       // 初速度向量
    private Vector3 Gravity;             // 重力向量
    private Vector3 currentAngle;// 当前角度

    private Vector3[] Positions;

    private float dTime = 0;

    private int index = 0;

    private bool StartPulling;

    void FixedUpdate()
    {
        if (StartPulling)
        {
            Caster.transform.position = Positions[index];
            index++;
            if (index >= Positions.Length)
            {
                OnActionEnd();
            }
        }
        else
        {
            Caster.SetVelocity(Vector3.zero, true);
        }
    }

    protected override void Effect()
    {
        base.Effect();
   
        pointA = Caster.transform;       

        // 时间=距离/速度
        ShotSpeed = Vector3.Distance(pointA.position, pointB.position) / time;


        // 通过一个式子计算初速度

        speed = new Vector3((pointB.position.x - pointA.position.x) / time,
      (pointB.position.y - pointA.position.y) / time - 0.5f * g * time, (pointB.position.z - pointA.position.z) / time);


        // 重力初始速度为0

        Gravity = Vector3.zero;

        int positionCount = Mathf.FloorToInt(time / Time.fixedDeltaTime);
        Positions = new Vector3[positionCount];
        Vector3 position = Caster.transform.position;
        for (int i = 0; i < positionCount; i++)
        {
            // v=gt

            Gravity.y = g * (dTime += Time.fixedDeltaTime);


            //模拟位移

            position += (speed + Gravity) * Time.fixedDeltaTime;


            // 弧度转度：Mathf.Rad2Deg

            currentAngle.x = -Mathf.Atan((speed.y + Gravity.y) / speed.z) * Mathf.Rad2Deg;


            // 设置当前角度

            //transform.eulerAngles = currentAngle;

            Positions[i] = position;
        }

        StartPulling = true;
    }
    public override void OnActionStart(Character_Base character)
    {  
        base.OnActionStart(character); 

        Character_Player player = Caster as Character_Player;
        pointB = player.HooKTarget.transform;
        player.FacePosition(player.HooKTarget.transform.position);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnActionEnd()
    {
        base.OnActionEnd();
        Caster.ClearSpeed();
    }
}
