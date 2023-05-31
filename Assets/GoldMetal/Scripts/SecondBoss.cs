using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SecondBoss : Enemy
{
    public LineRenderer lr;
    public Transform LaserBallTransform;
    public GameObject laserBall;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        
    }
    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        LaserBallTransform = GameObject.Find("LaserBall(Clone)").transform;
        laserBall = GameObject.Find("LaserBall(Clone)");

    }

    void Update()
    {
        lr.SetPosition(0, transform.position); //�������
        transform.LookAt(LaserBallTransform); //�ٶ󺸱�
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit)) //�浹 ��ü ����ĳ��Ʈ�� ���� ���
        {
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(1, transform.forward * 5000); //�浹 ������ ���ڷ� �׸���
        }
        if (isDead == true && laserBall.gameObject != null)
            Destroy(laserBall.gameObject);
    }  
}

