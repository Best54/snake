using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cubewall : MonoBehaviour
{
    public TextMeshPro numberText;
    private int _numberTail;
    public int blockWeightMin = 1;
    public int blockWeightMax = 11;
    float colorStep = 0;
    private void Awake()
    {
        int vilkaCube = blockWeightMax - blockWeightMin;
        colorStep = 1f / vilkaCube;
        _numberTail = Random.Range(blockWeightMin, blockWeightMax);
        numberText.text = _numberTail.ToString();
        Color tekColor = new Vector4(1 - _numberTail * colorStep, 0, 0, 1);
        GetComponent<Renderer>().material.color = tekColor;
    }

    private void InstantCubeParam(int col)
    {
        _numberTail -= col;
        numberText.text = _numberTail.ToString();
        Color tekColor = new Vector4(1 - _numberTail * colorStep, 0, 0, 1);
        GetComponent<Renderer>().material.color = tekColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out GoPlayer player))
        {
            player.PlayerVeloZero();
            int tempNum = player.DelTailSnake(_numberTail);
            if (tempNum == _numberTail)
            {
                gameObject.SetActive(false);
                player.PlayerVeloGo();
            }
            else InstantCubeParam(tempNum);
        }
    }
}
