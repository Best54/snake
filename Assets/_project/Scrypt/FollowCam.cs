using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GoPlayer player;
    public Vector3 PlayerToCamOffset;
    private float _speedMovingCam;
    private Vector3 _startPosCam;

    private void Awake()
    {
        _speedMovingCam = player.speedPlayer;
        _startPosCam = transform.position;
    }

    public void StartPos() {
        transform.position = _startPosCam;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = player.transform.position + PlayerToCamOffset;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speedMovingCam);
    }
}
