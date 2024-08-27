using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicController : MonoBehaviour
{

    [SerializeField] private AudioSource music;
    private bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void on()
    {
        isMuted = !isMuted;
        music.mute = isMuted;
    }
}

