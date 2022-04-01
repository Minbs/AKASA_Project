using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	struct EnemySpawnData
    {
		public string name;
		public string start;
		public string end;
		public float time;
    }

	List<EnemySpawnData> enemySpawnDatas = new List<EnemySpawnData>();

	float timer = 0;

    public void ReadEnemySpawnData()
    {
		TextAsset textFile = Resources.Load("Stage1") as TextAsset;
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

			enemySpawnDatas.Add(enemyData);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
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
				timer += Time.deltaTime;
				yield return null;

				if (enemySpawnDatas[0].time <= timer)
				{

					GameObject enemy = ObjectPool.Instance.PopFromPool("Enemy1");
					GameManager.Instance.enemiesList.Add(enemy);
					Vector3 pos = BoardManager.Instance.startTile.transform.position;
					pos.y = 0.2f;
					enemy.transform.position = pos;
					enemy.SetActive(true);
					enemySpawnDatas.RemoveAt(0);

				}
			}
		}


    }
}
