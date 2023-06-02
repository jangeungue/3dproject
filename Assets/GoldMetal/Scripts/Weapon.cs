using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range }
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bulletPrefab;
    //public List<GameObject> bullet = new List<GameObject>();
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public AudioSource ShotSound;
    public AudioSource SwingSound;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            SwingSound.Play();
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--; //���� ź���� ���ǿ� �߰��ϰ�, �߻����� �� �����ϵ��� �ۼ�
            StartCoroutine("Shot");
            ShotSound.Play();
        }               
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.45f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }
    
    IEnumerator Shot()
    {
        print(bulletPrefab);
        print(bulletPrefab.name);
        //ObjectPool objectPool = ObjectPool.Instance;
        //GameObject bullet = objectPool.PopFromPool(bulletPrefab.name);

        //#1.�Ѿ� �߻�

       GameObject BulletObj = ObjectPoolManager.Instance.BulletGetQueue();
        BulletObj.transform.position = bulletPos.position;
        //GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);// �Ѿ� ����
         Rigidbody bulletRigid = BulletObj.GetComponent<Rigidbody>(); //�Ѿ� ��
        bulletRigid.velocity = bulletPos.forward * 50;//�ٶ󺸴� ���� �տ��� �߻���ġ ���� 

        yield return null;//�� ������ ��� 

        //#2.ź�� ����
        GameObject CaseObj = ObjectPoolManager.Instance.CaseGetQueue();
        CaseObj.transform.position = bulletCasePos.position;
        //ź�� ��
        Rigidbody caseRigid = CaseObj.GetComponent<Rigidbody>();
        //�ν��Ͻ�ȭ �� ź�ǿ� ������ �� ���ϱ�
        Vector3 casevec = bulletCasePos.forward * Mathf.Lerp(-3, -2, 0) + Vector3.up * Mathf.Lerp(2, 3, 0); 
        caseRigid.AddForce(casevec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); 
    }
}
