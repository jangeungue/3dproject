using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public GameObject MenuCam;
    public GameObject gameCam;
    public PlayerCont player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startzone1;
    public GameObject startzone2;
    public GameObject startzone3;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;

    public Text maxScoreTxt;
    public Text ScoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    
    public Text curScoreText;
    public Text bestText;

    public Transform Itemposition;
    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;

    int HPplus;

    public GameObject ESCMenuSet;
    private void Awake()
    {
        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));  //string.Format �Լ��� ���ڿ� ��� ����      
    }
    public void GameStart()
    {
        
        MenuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
        
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = ScoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
    
    public void StageStart()
    {
        

        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startzone1.SetActive(false);
        startzone2.SetActive(false);
        startzone3.SetActive(false);
        
        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);


        isBattle = true;
        StartCoroutine(InBattle());
        
    }

    public void StageEnd()
    {
        
        player.transform.position = new Vector3(-9f, 0f, 71.6f); //�÷��̾� ����ġ

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startzone1.SetActive(true);
        startzone2.SetActive(true);
        startzone3.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;

        
    }

    IEnumerator InBattle()
    {
        if (stage % 5 == 0)
        {
            enemyCntD++;
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
        
        else
        {
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
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

        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
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
        ScoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE" + stage;

        int hour = (int)(playTime / 3600); //�ʴ��� �ð��� 3600, 60���� ������ �ú��ʷ� ���
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" 
                                        + string.Format("{0:00}", min) + ":" 
                                         + string.Format("{0:00}", second);

        //�÷��̾� UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
            playerAmmoTxt.text = "- / " + player.ammo;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = "- / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        //���� UI
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);
        
        //���� ���� UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

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
