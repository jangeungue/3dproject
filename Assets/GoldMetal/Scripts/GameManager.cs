using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�޴�, ���� ī�޶�
    public GameObject MenuCam;
    public GameObject gameCam;
    //�÷��̾�
    public PlayerCont player;
    //���� 
    public Boss boss;
    public SecondBoss secondboss;
    //����������
    public LaserLine laserline;
    //������, ���� ����
    public GameObject itemShop;
    public GameObject weaponShop;
    //��ŸƮ�� ����Ʈ
    public GameObject[] startZones = new GameObject[3];
    //��������
    public int stage;
    //�÷���Ÿ��
    public float playTime;
    //��Ʋ������ bool��
    public bool isBattle;
    //�� ���� Ȯ�� ����Ʈ
    public List<int> enemyCounts;
    //�� ���� ��ġ ����Ʈ
    public Transform[] enemyZones;
    //������ �� ����Ʈ
    public GameObject[] enemies;
    //�� ���� ��ȣ ����Ʈ
    public List<int> enemyList;
    //�޴�, ����, ���ӿ��� �г�
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    //0.�ְ�����MaxScore, 1.����Score, 2.��������Stage, 3.��Žtime, 4.ü��Health, 5.�Ѿ˰���Ammo, 6.����Coin
    public Text[] uiTexts = new Text[7];
    //����1,2,3,R �̹���
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    //���ʹ� A,B,C �ؽ�Ʈ
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    //���� ü�� UI ��ġ
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    //����Ʈ �������� �ؽ�Ʈ
    public Text curScoreText;
    public Text bestText;
    //������ ���� ��ġ
    public Transform Itemposition;
    //������ 1,2,3 ����
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    //�� Hp �ø���
    int HPplus;
    //ESC, �����г� UI ����
    public GameObject ESCMenuSet;
    public GameObject bossPanel;
    //����� �ҽ� �Ŵ���
    [System.Serializable]
    public class AudioSourceManager
    {
        public string SoundName;
        public AudioClip Audio;
    }
    //����� ����Ʈ
   public List<AudioSourceManager> AudioList = new List<AudioSourceManager>();
    AudioSource Audio;

    private void Awake()
    {
        enemyList = new List<int>(); //�� ���� ����Ʈ
        uiTexts[0].text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));  //string.Format �Լ��� ���ڿ� ��� ����
        Audio = GetComponent<AudioSource>(); //����� ������Ʈ ��������
    }
    public void GameStart() //ī�޶�, �г�, �÷��̾�
    {
        //Audio.clip = AudioList[0].Audio;
        MenuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
        
    }
    
    public void GameOver() //�г�, �����ؽ�Ʈ
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = uiTexts[1].text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore) //�ְ� ������� ����, ����Ƽ �⺻ ���� �÷��̾� ������ Ŭ�������� ��������
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }
    public void ReStart()//����� �ε��
    {
        SceneManager.LoadScene(0);
    }
    
    public void StageStart()//����, ��ŸƮ��, isBattle, InBattle
    {       
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZones[0].SetActive(false);
        startZones[1].SetActive(false);
        startZones[2].SetActive(false);
        
        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());      
    }
    public void StageEnd()//�÷��̾� ��ġ, ����, ��ŸƮ��, isBattle, ��������++
    {
        
        player.transform.position = new Vector3(-9f, 0f, 71.6f); //�÷��̾� ����ġ

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZones[0].SetActive(true);
        startZones[1].SetActive(true);
        startZones[2].SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;      
    }
    IEnumerator BossPanel()//�����г�
    {
        bossPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        bossPanel.SetActive(false);
    }
    IEnumerator InBattle()//��Ʋ���̸�
    {       
        if (stage == 6)
        {

        }
        if (stage == 11)
        {

        }
        if (stage == 16)
        {

        }
        if (stage == 5)//ù ��° ����
        {
            StartCoroutine(BossPanel());
            enemyCounts[3]++;
            yield return new WaitForSeconds(2f);
            GameObject instantEnemy = Instantiate(enemies[3],
                                                      enemyZones[0].position,
                                                      enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantEnemy.GetComponent<Boss>();
            enemy.maxhealth += HPplus;
            enemy.curhealth += HPplus;
            HPplus += 50;
        }
        if (stage == 10)//�� ��° ����
        {
            enemyCounts[4]++;
            yield return new WaitForSeconds(2f);
            GameObject instantEnemy = Instantiate(enemies[4],
                                      enemyZones[0].position,
                                      enemyZones[0].rotation);
            GameObject instantEnemy1 = Instantiate(enemies[5],
                                      enemyZones[0].position,
                                      enemyZones[0].rotation);
            //��� Enemy, SecondBoss ��ũ��Ʈ
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            secondboss = instantEnemy.GetComponent<SecondBoss>();
            //�������� LaserBall ��ũ��Ʈ
            LaserBall laserball = instantEnemy1.GetComponent<LaserBall>();
            //���ӸŴ��� laserline ��ũ��Ʈ
            laserline = instantEnemy.GetComponentInChildren<LaserLine>();
            //�������� laserline ��ũ��Ʈ
            laserball.laserline = instantEnemy.GetComponentInChildren<LaserLine>();



            enemy.manager = this;
                    
            enemy.maxhealth += HPplus;
            enemy.curhealth += HPplus;
            HPplus += 50;   
            instantEnemy.SetActive(true);
            instantEnemy1.SetActive(true); //���� Ȱ��ȭ
            laserball.enabled = true; //������Ʈ Ȱ��ȭ
            //laserball.target = player.transform; //�̰� �������� Ÿ���� ���ʹ� Ÿ��X
        }
        
        else
        {          
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCounts[0]++;
                        break;
                    case 1:
                        enemyCounts[1]++;
                        break;
                    case 2:
                        enemyCounts[2]++;
                        break;
                }
            }
            while (enemyList.Count > 0) //�������� ���� ��ȯ
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]],
                                                      enemyZones[ranZone].position,
                                                      enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0); //���� �Ŀ��� ���� �����ʹ� RemoveAt() �Լ��� ����
                yield return new WaitForSeconds(4f); //�����ϰ� while���� ������ ���ؼ� �� yield return ����
            }
            
        }

        while (enemyCounts[0] + enemyCounts[1] + enemyCounts[2] + enemyCounts[3] + enemyCounts[4] > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);


        boss = null;
        StageEnd();       
    }
    private void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
        // ESC ��ư      
        if (Input.GetKeyDown(KeyCode.Escape)) //ESC �ٲٷ��� GetButtonDown
        {
            if (ESCMenuSet.activeSelf)
                ESCMenuSet.SetActive(false);
            else
                ESCMenuSet.SetActive(true);
        }       
    }
    //������ ���� ������ �۾� ��
    public void GameExit()
    {
        Application.Quit();
    }
    private void LateUpdate() //Ui �ȼ����� LateUpdate
    {
        //��� UI
        uiTexts[1].text = string.Format("{0:n0}", player.score);
        uiTexts[2].text = "STAGE" + stage;

        int hour = (int)(playTime / 3600); //�ʴ��� �ð��� 3600, 60���� ������ �ú��ʷ� ���
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        uiTexts[3].text = string.Format("{0:00}", hour) + ":" 
                                        + string.Format("{0:00}", min) + ":" 
                                         + string.Format("{0:00}", second);

        //�÷��̾� UI
        uiTexts[4].text = player.health + " / " + player.maxHealth;
        uiTexts[6].text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
            uiTexts[5].text = "- / " + player.ammo;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            uiTexts[5].text = "- / " + player.ammo;
        else
            uiTexts[5].text = player.equipWeapon.curAmmo + " / " + player.ammo;

        //���� UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);
        
        //���� ���� UI
        enemyATxt.text = enemyCounts[0].ToString();
        enemyBTxt.text = enemyCounts[1].ToString();
        enemyCTxt.text = enemyCounts[2].ToString();

        //���� ü�� UI �̹����� scale�� ���� ü�� ������ ���� ����
        //���� ������ ������� �� UI������Ʈ ���� �ʵ��� ���� �߰�    
        if (boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curhealth / boss.maxhealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200;          
        }           
    }
}
