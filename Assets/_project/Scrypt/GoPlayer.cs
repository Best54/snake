using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoPlayer : MonoBehaviour
{
    public GameManager gM;
    public GameObject playerNoGo;
    public float offsetPlayer = 1f;
    public float movingDelay = 0.3f;
    public GameObject bodySnake;
    public TextMeshPro sizeSnake;
    public float speedPlayer = 1f;
    public Vector3 direction;

    private bool _moveR = false;
    private bool _moveL = false;
    private bool _moveU = false;
    private bool _moveD = false;
    private float _timing;
    public List<GameObject> snake = new List<GameObject>();
    private GameObject _curBodySnake;
    private GameObject _predBodySnake;
    private Vector3 _curBodySnakeTP;
    private Vector3 _predBodySnakeTP;
    private Vector3 _startPosPlayer;

    public ParticleSystem deathPS;
    public ParticleSystem tailDelPS;
    public ParticleSystem cubeDelPS;

    private AudioSource _audio;
    public AudioClip saluteAC;
    public AudioClip winAC;
    public AudioClip eatAC;
    public AudioClip explosAC;
    public AudioClip loseAC;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _timing = movingDelay;
        snake.Add(gameObject);
        _startPosPlayer = transform.position;
    }
    private void FixedUpdate()
    {
        foreach (GameObject sn in snake)
        {
            sn.transform.Translate(direction.normalized * speedPlayer);
        }
    }
    void Update()
    {
        //Чтобы не переполнился счётчик
        if (_timing < movingDelay * 10) _timing += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            _moveR = true;
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            _moveR = false;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            _moveL = true;
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            _moveL = false;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            _moveU = true;
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            _moveU = false;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            _moveD = true;
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            _moveD = false;        
        if (_timing >= movingDelay) MovingHead();
    }

    void MovingHead()
    {
        float _z = 0, _y = 0;
        if (_moveR)
        {
            _z = Mathf.Clamp(transform.position.z - offsetPlayer, -6f, 6f);
            transform.position = new Vector3(transform.position.x, transform.position.y, _z);
            _timing = 0f;
        }
        if (_moveL)
        {
            _z = Mathf.Clamp(transform.position.z + offsetPlayer, -6f, 6f);
            transform.position = new Vector3(transform.position.x, transform.position.y, _z);
            _timing = 0f;
        }
        if (_moveU)
        {
            _y = Mathf.Clamp(transform.position.y + offsetPlayer, 1f, 5f);
            transform.position = new Vector3(transform.position.x, _y, transform.position.z);
            _timing = 0f;
        }
        if (_moveD)
        {
            _y = Mathf.Clamp(transform.position.y - offsetPlayer, 1f, 5f);
            transform.position = new Vector3(transform.position.x, _y, transform.position.z);
            _timing = 0f;
        }        
        MovingTail();
    }

    void MovingTail()
    {
        for (int i=1; i<snake.Count; i++)
        {
            _curBodySnake = snake[i];
            _predBodySnake = snake[i - 1];

            _curBodySnakeTP = _curBodySnake.transform.position;
            _predBodySnakeTP = _predBodySnake.transform.position;

            _curBodySnake.transform.position = Vector3.Slerp(_curBodySnakeTP, new Vector3(_curBodySnakeTP.x, _predBodySnakeTP.y, _predBodySnakeTP.z), movingDelay *2);
        }
    }

    public void AddTailSnake(int col)
    {
        PlayerVeloZero();
        _audio.PlayOneShot(eatAC, gM.sGM.effectsVolume);
        for (int i=0; i < col; i++)
        {
            float tekLength = snake.Count * bodySnake.transform.localScale.x + bodySnake.transform.localScale.x / 2f;
            snake.Add(Instantiate(bodySnake, transform.position + new Vector3(-tekLength, 0, 0), Quaternion.identity, playerNoGo.transform));
        }
        sizeSnake.text = snake.Count.ToString();
        PlayerVeloGo();
    }
    public int DelTailSnake(int numberTail)
    {
        PlayerVeloZero();
        if (numberTail >= snake.Count)
        {
            int countDel = 0;
            while(snake.Count > 1){
                countDel++;
                DelNumSnake(snake.Count - 1);
            }
            sizeSnake.text = snake.Count.ToString();
            IsDie();
            return countDel;
        }
        else
        {
            //удалям куб, хвост и движемся дальше
            for (int i = 0; i < numberTail; i++)
            {
                DelNumSnake(snake.Count - 1);
            }
            sizeSnake.text = snake.Count.ToString();
            PlayerVeloGo();
            return numberTail;
        }
    }

    private void DelNumSnake(int whyDel)
    {
        _audio.PlayOneShot(explosAC, gM.sGM.effectsVolume);
        Destroy(snake[whyDel], 0.01f);
        snake.RemoveAt(whyDel);
    }

    public void GoToStart()
    {
        _moveD = false; _moveL = false; _moveR = false; _moveU = false;
        transform.position = _startPosPlayer;
        for (int i = 1; i < snake.Count; i++)
        {
            float tekLength = i * bodySnake.transform.localScale.x + bodySnake.transform.localScale.x / 10f;
            snake[i].transform.position = transform.position + new Vector3(-tekLength, 0, 0);
        }
        PlayerVeloGo();
    }

    public void PlayerVeloZero() => enabled = false;
    public void PlayerVeloGo() => enabled = true;

    public void PlayerBoomCube(Vector3 pos)
    {
        _audio.PlayOneShot(saluteAC, gM.sGM.effectsVolume);
        cubeDelPS.transform.position = pos;
        cubeDelPS.Play();
    }

    public void IsDie()
    {
        deathPS.Play();        
        PlayerVeloZero();
        _audio.PlayOneShot(loseAC, gM.sGM.effectsVolume);
        gM.GameLose();
    }

    public void IsWin()
    {
        tailDelPS.Play();
        PlayerVeloZero();
        _audio.PlayOneShot(winAC, gM.sGM.effectsVolume);
        gM.GameWin();
    }
}
