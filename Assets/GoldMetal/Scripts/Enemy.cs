using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D, E}; // enum���� Ÿ���� ������ �װ��� ������ ������ ����
    public Type enemyType;
    public int maxhealth;
    public int curhealth;
    public int score;
    public GameManager manager;
    public Transform target;
    public BoxCollider meleeArea; //���ݹ��� ����
    public GameObject bullet;
    public GameObject[] coins;
    public bool isChase; //������ �����ϴ� ����
    public bool isAttack;
    public bool isDead;
    

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs; //�ǰ� ����Ʈ�� �÷��̾�ó�� ��� ���׸���� ����
    public NavMeshAgent nav;
    public Animator anim;
    public RectTransform[] enemyHealthBars = new RectTransform[3];
    //NavMesh : NavAgent�� ��θ� �׸��� ���� ����(Mesh) Static ������Ʈ�� Bake ����
    private void Awake() //Awake�Լ��� �ڽ� ��ũ��Ʈ�� �Լ� ����
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (enemyType != Type.D && enemyType != Type.E)
        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    private void Update()
    {
        if (nav.enabled && enemyType != Type.D && enemyType != Type.E)
        {
            nav.SetDestination(target.position); //SetDestination() : ������ ��ǥ ��ġ ���� �Լ�
            nav.isStopped = !isChase; //isStopped�� ����Ͽ� �Ϻ��ϰ� ���ߵ��� �ۼ�
        }                        
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero; //angularVelocity : ���� ȸ�� �ӵ�
        }
        
    }
    void Targerting()
    {
        if (!isDead && enemyType != Type.D && enemyType != Type.E)
        {
            float targetRadius = 0f; //SphereCast ()�� ������, ���̸� ������ ���� ����
            float targetRange = 0f;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;

            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                                      targetRadius,
                                      transform.forward,
                                      targetRange,
                                      LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack) //rayHit ������ �����Ͱ� ������ �ڷ�ƾ ����
            {
                StartCoroutine(Attack());
            }
        }
        
    }

    IEnumerator Attack()
    {
        isChase = false; //���� ������ �� ����, �ִϸ��̼ǰ� �Բ� ���ݹ��� Ȱ��ȭ
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;

            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.1f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;

            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
     

        }
       
        isChase = true; 
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curhealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;           
            StartCoroutine(OnDamage(reactVec, false));
            
            
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curhealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, true));
            
        }

    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curhealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs)       
            mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curhealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else if (curhealth <= 0 && !isDead)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;
                
            gameObject.layer = 14;
            isDead = true;
            isChase = false;
            if (manager.stage != 10)//����� X
            {
                nav.enabled = false; //��� ���׼��� �����ϱ� ���� NavAgent�� ��Ȱ��
                anim.SetTrigger("doDie");

                int ranCoin = Random.Range(0, 3);
                Instantiate(coins[ranCoin], transform.position, Quaternion.identity);
            }
            GameObject obj = GameObject.Find("Player"); //�����ɶ� ������(Player)�� ã�´�
            target = obj.transform; //��ġ �Ҵ�
            PlayerCont player = target.GetComponent<PlayerCont>();
            player.score += score;
            
            
                switch (enemyType)
            {
                case Type.A:
                    manager.enemyCounts[0]--;
                    break;
                case Type.B:
                    manager.enemyCounts[1]--;
                    break;
                case Type.C:
                    manager.enemyCounts[2]--;
                    break;
                case Type.D:
                    manager.enemyCounts[3]--;
                    break;
                case Type.E:
                    manager.enemyCounts[4]--;
                    break;
            }
            
            

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse); //AddForce �Լ��� �˹� ���ϱ�
                rigid.AddTorque(reactVec * 5, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse); //AddForce �Լ��� �˹� ���ϱ�
            }
            Destroy(gameObject, 4);           
        }

    }
    private void LateUpdate() //���� ������ ��� �ּ�ó��
    {
        switch (enemyType)
        {
            case Type.A:
                enemyHealthBars[0].localScale = new Vector3((float)curhealth / maxhealth, 1, 1);
                break;
            case Type.B:
                enemyHealthBars[1].localScale = new Vector3((float)curhealth / maxhealth, 1, 1);
                break;
            case Type.C:
                enemyHealthBars[2].localScale = new Vector3((float)curhealth / maxhealth, 1, 1);
                break;
        }
    }
}
