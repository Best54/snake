using UnityEngine;

public class soundGM : MonoBehaviour
{
    public AudioClip backgroundMuzik;
    [Min (0)]
    public float backgroundVolume;    

    [Min(0.4f)]
    public float effectsVolume;

    private AudioSource _audioMuzik;

    private void Awake()
    {
        _audioMuzik = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioMuzik.volume = backgroundVolume;
        _audioMuzik.clip = backgroundMuzik;
        _audioMuzik.loop = true;
        _audioMuzik.Play();        
    }

    private void OnDisable()
    {
        _audioMuzik.Stop();
    }

    private void Update()
    {
        _audioMuzik.volume = backgroundVolume;
    }
}
