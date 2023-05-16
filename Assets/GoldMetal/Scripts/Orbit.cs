using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; //�÷��̾�
    public float orbitspeed;
    Vector3 offset;


    void Start()
    {
        offset = transform.position - target.position; 
    }

    void Update()
    {
        transform.position = target.position + offset; //�÷��̾���� �Ÿ� ��
        transform.RotateAround(target.position, Vector3.up, orbitspeed * Time.deltaTime); //RotateAround()�� ��ǥ�� �����̸� �ϱ׷����� ������ ����
    }
}
