using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using System;

public class EnemySpawner : MonoBehaviour
{

	
	[Serializable]
	public struct EnemySpawnData
    {
		public string name;
		public string start;
		public string end;
		public float time;
		public int wave;
    }

	public List<EnemySpawnData> enemySpawnDatas = new List<EnemySpawnData>();

	public float spawnTimer;

    public void ReadEnemySpawnData()
    {
		TextAsset textFile = Resources.Load("Datas/EnemySpawnInfo/EnemySpawnInfo_Stage1") as TextAsset;
		StringReader stringReader = new StringReader(textFile.text);
		string line = stringReader.ReadLine();

		while (stringReader != null)
		{
			line = stringReader.ReadLine();
			if (line == null)
			{
				break;
			}

			EnemySpawnData enemyData = new EnemySpawnData();

			enemyData.name = line.Split(',')[0];
			enemyData.start = line.Split(',')[1];
			enemyData.end = line.Split(',')[2];
			enemyData.time = float.Parse(line.Split(',')[3]);
			enemyData.wave = int.Parse(line.Split(',')[4]);

			enemySpawnDatas.Add(enemyData);
		}

		enemySpawnDatas.OrderBy(e => e.time);
	}

    // Start is called before the first frame update
    void Start()
    {
		spawnTimer = 0;
		ReadEnemySpawnData();
    }

    // Update is called once per frame
    void Update()
    {
	
    }

	public IEnumerator Spawn()
    {
		if (GameManager.Instance.state == State.BATTLE)
		{
			while (enemySpawnDatas.Count > 0)
			{
				spawnTimer += Time.deltaTime;
				yield return null;




				if (enemySpawnDatas[0].time <= spawnTimer && GameManager.Instance.currentWave == enemySpawnDatas[0].wave)
				{
					GameObject enemy = ObjectPool.Instance.PopFromPool(enemySpawnDatas[0].name);
					GameManager.Instance.enemiesList.Add(enemy);
					Vector3 pos = BoardManager.Instance.startTile.transform.position;
					enemy.transform.position = pos;
					enemy.SetActive(true);
					enemySpawnDatas.RemoveAt(0);

				}

				if (GameManager.Instance.enemiesList.Count == 0 && enemySpawnDatas[0].wave != GameManager.Instance.currentWave)
				{
					Debug.Log(enemySpawnDatas[0].wave);
					spawnTimer = 0;
					GameManager.Instance.currentWave++;
				}
			}

			
		}


    }
}
