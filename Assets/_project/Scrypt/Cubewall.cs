using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cubewall : MonoBehaviour
{
    public TextMeshPro numberText;
    [Min(0)]
    public int blockWeightMin = 1;
    [Min(1)]
    public int blockWeightMax = 11;

    private int _numberTail;
    private float _colorStep = 0;

    private void Awake()
    {
        int vilkaCube = blockWeightMax - blockWeightMin;
        _colorStep = 1f / vilkaCube;
        _numberTail = Random.Range(blockWeightMin, blockWeightMax);
        InstantCubeParam(0);
    }

    private void InstantCubeParam(int col)
    {
        _numberTail -= col;
        numberText.text = _numberTail.ToString();
        Color tekColor = new Vector4(1 - _numberTail * _colorStep, 0, 0, 1);
        GetComponent<Renderer>().material.color = tekColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out GoPlayer player))
        {
            player.PlayerVeloZero();
            //сколько игрок отнял ХП у куба
            int tempNum = player.DelTailSnake(_numberTail);
            if (tempNum == _numberTail)
            {
                player.PlayerBoomCube(transform.position + new Vector3(10, 0 , 0));
                gameObject.SetActive(false);
                player.PlayerVeloGo();
            }
            else InstantCubeParam(tempNum);
        }
    }
}
