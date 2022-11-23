using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeAdd : MonoBehaviour
{    
    public TextMeshPro numberText;
    private int _numberTail;

    private void Awake()
    {
        _numberTail = Random.Range(1, 10);
        numberText.text = _numberTail.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        if (collision.collider.TryGetComponent(out GoPlayer player)){
            player.AddTailSnake(_numberTail);
        }
    }
}
