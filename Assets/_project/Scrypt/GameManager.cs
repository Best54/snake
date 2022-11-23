using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public soundGM sGM;
    public GameObject gameUI;
    public int MaxLevel = 5;
    public GameObject panelWin;
    public GameObject panelLose;
    public FollowCam cam;
    public GoPlayer player;
    public LevelGenerator lvlGen;
    public TextMeshProUGUI tekLvlUI;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    private int _tekLvl;

    void Start()
    {
        _tekLvl = 1;

        if (gameUI == null || panelWin == null || panelLose == null)
        {
            Debug.Log("No put panel to script - StartSettingsScr");
        }
        else
        {
            StartLevel();
        }
    }

    public void GameWin()
    {
        panelWin.SetActive(true);
        winText.text = "���� ������������! �� ������� ������ " + _tekLvl + " �������! ����� � ���� 4 ������.";
        _tekLvl++;
        if (_tekLvl >= MaxLevel) {
            winText.text = "����������� � ������������ 4-�� ������! ���� ����� ���������� � 1-�� ������.";
            _tekLvl = 1;
        }
        lvlGen.ReCreateLevel(_tekLvl);
    }
    public void GameLose()
    {
        panelLose.SetActive(true);
        loseText.text = "�� �� ������ " + _tekLvl + " �������. �� ���������������, ���������� ������ ��� ��� ���. �������, ��� ��� ��������� � �������� �������� ����� ����������, � ��� �������������� ��� �������.";
    }

    public void ButtonNext() => StartLevel();

    private void StartLevel()
    {        
        cam.StartPos();
        player.GoToStart();
        gameUI.SetActive(true);
        panelWin.SetActive(false);
        panelLose.SetActive(false);
        tekLvlUI.text = "Level " + _tekLvl.ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
