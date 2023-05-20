using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    public Transform target;
    NavMeshAgent nav;
    // Monobehaviour�� Bullet���� ��ü�Ͽ� ����ϱ�
    //��ũ��Ʈ�� ����ϸ� ������ �Լ��� �״�� �����ϸ鼭 ���� �߰� ����
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position); // ����
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
            yield return new WaitForSeconds(5f);
            this.gameObject.SetActive(false);  
    }
}
