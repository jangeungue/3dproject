using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LaserBall : Bullet
{
    //�����������ǵ�
    public float laserballspeed = 3f;
    //isLaserball true �ʱ�ȭ
    public bool isLaserball = true;
    //������
    private Transform target;
    //���������� Ŭ���� ��������
    public LaserLine laserline;
    //���
    NavMeshAgent agent;
    //secondboss Ŭ���� ��������
    //SecondBoss secondboss;
    void Awake()
    {
        //������� �������� �˷��ش�.
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = target.transform.position;
    }
    Transform playerTrans; //�÷��̾� ��ġ
    private void Start()
    { 
        StartCoroutine(isLaser()); //�̰� ���ָ� ���ڸ��� ��
        //secondboss = FindObjectOfType<SecondBoss>(); //SecondBoss������Ʈ�� ���� ����� ã�� ������ �Ҵ���
        GameObject obj  = GameObject.Find("Player"); //�����ɶ� ������(Player)�� ã�´�
        playerTrans = obj.transform; //��ġ �Ҵ�
    }
    private void Update()
    {
        //���������� �����ǰ� ������������ �÷��̾� ������ x��z�� ���ؼ� isLaserball�� true�� �� 
        if (isLaserball == true && 
            transform.localPosition.x + 0.5f > laserline.LookPlayer.x && 
            transform.localPosition.x - 0.5f < laserline.LookPlayer.x && 
            transform.localPosition.z + 0.5f > laserline.LookPlayer.z && 
            transform.localPosition.z - 0.5f < laserline.LookPlayer.z)
        {
            isLaserball = false; //false�� ����� ����
            laserline.LookPlayer.x = 9999;
            laserline.LookPlayer.z = -9999;
        }
        else if (laserline.islook == true)
        {
            chu();           
        }
        if (isLaserball == false)
        {
            laserballspeed = 3f; 
        }
    }
    IEnumerator isLaser() //�ʹ� 2�� navmash�� �Ÿ� ��ٰ� ������ �ǵ��� ��µ� �� �ȵ�
    {
        agent.stoppingDistance = 15.0f; // ���ϴ� �Ÿ��� �����մϴ�      
        yield return new WaitForSeconds(2f);
        agent.enabled = false;   
    }
    void chu()
    {
        if (isLaserball) //�÷��̾� ��ġ ���� �̵�
        {
            transform.position = Vector3.Lerp(transform.position, playerTrans.position, laserballspeed * Time.deltaTime);
            //laserballspeed += Time.deltaTime * 2f;
        }            
    }    
}
