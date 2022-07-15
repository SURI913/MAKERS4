//���� ���� ���� ��ũ��Ʈ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;	//���� �б� namespace

public class GameManager : MonoBehaviour {
	[Header("[ Core ]")]
	public ObjectManager objManager;
	public Player player;

	[Header("[ Spawn Monster ]")]
	public Transform[] spawnPoints; //���� ����Ʈ
	public float nextSpawnDelay;	//���� ���� ������ ��
	public float curSpawnDelay;	//���� ���� ������ ��
	public List<Spawn> spawnList;	//���� ���� ����Ʈ
	public int spawnIndex;	//���� �ε���
	public bool spawnEnd;	//���� ���� �˸�
	string[] monsterObjs;   //���� ���� ����

	[Header("[ Stage ]")]
	public int stageNum;	//���� �������� �ѹ�
	public int maxKillNum;	//�������� ��ǥ óġ ��
	public int curKillNum;  //���� óġ ��
	bool isSpawnBoss;   //���� ��ȯ ����

	[Header("[ UI ]")]
	public RectTransform playerHealthGroup;
	public RectTransform playerHealthBar;
	public RectTransform bossHealthGroup;
	public RectTransform bossHealthBar;
	public RectTransform monsterHealthGroupPrefab;
	Monster bossLogic;

	void Awake() {
		monsterObjs = new string[] { "MonsterS1_A", "MonsterS1_B", "MonsterS1_Boss" };
		spawnList = new List<Spawn>();
		ReadSpawnFile();
		StageStart();
	}

	public void StageStart() {
		curKillNum = 0;

		switch(stageNum) {
			case 1:
				maxKillNum = 15;
				break;
			case 2:
				maxKillNum = 20;
				break;
			case 3:
				maxKillNum = 25;
				break;
			case 4:
				maxKillNum = 30;
				break;
			case 5:
				maxKillNum = 40;
				break;
		}
	}

	public void StageEnd() {
		if (stageNum < 5) {
			stageNum++;
			Invoke("StageStart", 8f);
			return;
		}

		Invoke("GameOver", 4f);
	}

	void GameOver() {
		//���� ���� �Լ�
	}

	void ReadSpawnFile() {
		//���� �ʱ�ȭ
		spawnList.Clear();
		spawnIndex = 0;
		spawnEnd = false;

		//�ؽ�Ʈ ���� �б�
		TextAsset textFile = Resources.Load("Stage 1") as TextAsset;
		StringReader strReader = new StringReader(textFile.text);
		while (strReader != null) {
			string line = strReader.ReadLine();

			if (line == null)
				break;

			//���� ������ ���� �� ����
			Spawn spawnData = new Spawn();
			spawnData.delay = float.Parse(line.Split(',')[0]);
			spawnData.type = line.Split(',')[1];
			spawnData.point = int.Parse(line.Split(',')[2]);
			spawnList.Add(spawnData);
		}

		//�ؽ�Ʈ ���� �ݱ�
		strReader.Close();

		//ù��° ���� ������ ����
		nextSpawnDelay = spawnList[0].delay;
	}

	void Update() {
		if (isSpawnBoss)	//������ �����Ǿ��ִٸ� ������ ���� ����
			return;

		if (maxKillNum == curKillNum)   //��ǥġ�� ��� ä���, ������ ��ȯ���� �ʾҴٸ�
			DestroyMonsters();

		if (maxKillNum != curKillNum && spawnEnd)	//��ǥ�� ��ä��� ���� ������ ������
			ReadSpawnFile();    //�ٽ� ���� ����

		//���� ������ ����
		curSpawnDelay += Time.deltaTime;

		if (curSpawnDelay > nextSpawnDelay && !spawnEnd) {	//������ �ð��� �����Ǹ�
			SpawnMonster();	//���� ����
			curSpawnDelay = 0;	//���� ������ �� �ʱ�ȭ
		}
	}
	
	void DestroyMonsters() {
		//���� ��� �� óġ
		GameObject[] monsterS1_A = objManager.GetPool("MonsterS1_A");
		GameObject[] monsterS1_B = objManager.GetPool("MonsterS1_B");
		for (int index = 0; index < monsterS1_A.Length; index++)
			if (monsterS1_A[index].activeSelf) {
				Monster monsterLogic = monsterS1_A[index].GetComponent<Monster>();
				monsterLogic.OnDamaged(1000);
			}
		for (int index = 0; index < monsterS1_B.Length; index++)
			if (monsterS1_B[index].activeSelf) {
				Monster monsterLogic = monsterS1_B[index].GetComponent<Monster>();
				monsterLogic.OnDamaged(1000);
			}

		//óġ �� ���� ��ȯ
		isSpawnBoss = true;
		SpawnBoss();
	}

	void SpawnMonster() {
		int monsterIndex = 0;	//���� Ÿ�� ����
		switch(spawnList[spawnIndex].type) {
			case "S1_A":
				monsterIndex = 0;
				break;
			case "S1_B":
				monsterIndex = 1;
				break;
		}

		int spawnPoint = spawnList[spawnIndex].point;	//���� ����Ʈ ����
		GameObject monster = objManager.MakeMonster(monsterObjs[monsterIndex]);	//���� �ҷ�����
		monster.transform.position = spawnPoints[spawnPoint].position;  //���� ���� ��ġ ����
		Rigidbody2D rigid = monster.GetComponent<Rigidbody2D>();
		Monster monsterLogic = monster.GetComponent<Monster>();
		monsterLogic.player = player;   //�÷��̾� �Ѱ��ֱ�
		monsterLogic.objManager = objManager;   //������Ʈ �Ŵ��� �Ѱ��ֱ�(�߻�ü ��)
		monsterLogic.gameManager = this;    //���� �Ŵ��� �Ѱ��ֱ�
		monsterLogic.monsterHealthGroupPrefab = monsterHealthGroupPrefab;	//���� ü�¹� �Ѱ��ֱ�

		rigid.velocity = new Vector2(monsterLogic.monsterSpeed * (-1), 0);  //���� �̵�

		spawnIndex++;
		if (spawnIndex == spawnList.Count) {	//������ ���� ���� ��
			spawnEnd = true;	//���� ���� true�� ����
			return;
		}

		//���� ���� ������ ����
		nextSpawnDelay = spawnList[spawnIndex].delay;
	}

	void SpawnBoss() {
		GameObject boss = objManager.MakeMonster("MonsterS1_Boss");
		boss.transform.position = spawnPoints[1].position;
		Rigidbody2D rigid = boss.GetComponent<Rigidbody2D>();
		bossLogic = boss.GetComponent<Monster>();
		bossLogic.player = player;
		bossLogic.objManager = objManager;
		bossLogic.gameManager = this;
		rigid.velocity = new Vector2(bossLogic.monsterSpeed * (-1), 0);
	}

	void LateUpdate() {
		playerHealthBar.localScale = new Vector3((float)player.curHealth / player.maxHealth, 1, 1);
		if (isSpawnBoss) {
			bossHealthGroup.anchoredPosition = Vector3.down * 10;
			bossHealthBar.localScale = new Vector3((float)bossLogic.monsterCurHealth / bossLogic.monsterMaxHealth, 1, 1);
		}
		else
			bossHealthGroup.anchoredPosition = Vector3.up * 200;
	}
}