using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject bodySnake;
    public GameObject cubeWall;
    public GameObject finish;
    public GameObject rightWall;
    public GameObject leftWall;
    public GameObject downWall;

    public int sizeBetwenWallMin = 15;
    public int sizeBetwenWallMax = 30;

    public float[] probs = new float[6];

    private int _maxLengthLevel;

    private void Start()
    {
        GenerateWall(1);
        GenerateLevel();
    }

    public void ReCreateLevel(int numLvl)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateWall(numLvl);
        GenerateLevel();
    }

    private void GenerateWall(int tekLevel)
    {
        _maxLengthLevel = 100 + tekLevel * 20;

        rightWall.transform.localScale += new Vector3(_maxLengthLevel, 0, 0);
        rightWall.transform.position += new Vector3(_maxLengthLevel/2, 0, 0);
        leftWall.transform.localScale += new Vector3(_maxLengthLevel, 0, 0);
        leftWall.transform.position += new Vector3(_maxLengthLevel / 2, 0, 0);
        downWall.transform.localScale += new Vector3(_maxLengthLevel, 0, 0);
        downWall.transform.position += new Vector3(_maxLengthLevel / 2, 0, 0);
    }

    public void GenerateLevel()
    {
        int tempProbs;
        // first redWall
        int rndmPosCubeWall = Random.Range(sizeBetwenWallMin, sizeBetwenWallMax) + sizeBetwenWallMin;

        for (int x = 2; x < _maxLengthLevel; x++)
        {
            if (x > rndmPosCubeWall) rndmPosCubeWall = Random.Range(sizeBetwenWallMin + x, sizeBetwenWallMax + x);
            for (int z = 0; z <= 13; z++)
                for (int y = 1; y <= 5; y++)
                {
                    if (x == rndmPosCubeWall)
                    {
                        Instantiate(cubeWall, new Vector3(x, y, z - 6f), Quaternion.identity, transform);
                    }
                    else
                    {
                        tempProbs = Choose(probs);
                        switch (tempProbs)
                        {
                            case 1:
                                Instantiate(bodySnake, new Vector3(x, y, z - 6f), Quaternion.identity, transform);
                                break;
                            case 2:
                                Instantiate(wall, new Vector3(x - 0.5f, y, z - 7.5f), Quaternion.identity, transform);
                                break;
                            case 3:
                                Instantiate(wall, new Vector3(x - 0.5f, y, z - 5.5f), Quaternion.identity, transform);
                                break;
                            case 4:
                                Instantiate(wall, new Vector3(x - 0.5f, y, z - 7.5f), Quaternion.identity, transform);
                                Instantiate(wall, new Vector3(x - 0.5f, y, z - 5.5f), Quaternion.identity, transform);
                                break;
                            case 5:
                                Instantiate(wall, new Vector3(x - 1f, y, z - 6.5f), Quaternion.Euler(0, 90, 0), transform);
                                break;
                            default:
                                break;
                        }
                    }
                }
        }
        finish.transform.position = new Vector3(_maxLengthLevel + 1, finish.transform.position.y, finish.transform.position.z);
    }

    int Choose(float[] probsC)
    {
        float total = 0;
        float tekProbability = 0;

        foreach (float el in probsC) total += el;

        tekProbability = Random.Range(0, total +1f);

        for (int i = 0; i<probsC.Length; i++)
        {
            tekProbability -= probsC[i];
            if (tekProbability < 0) return i;
        }
        return 0;
    }
}
