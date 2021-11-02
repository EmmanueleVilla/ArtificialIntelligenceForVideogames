using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    public AudioSource Ambient;
    public GameManager GameManager;

    void Start()
    {
        GameManager.GameStarted += GameManager_GameStarted;
    }

    private void GameManager_GameStarted(object sender, EventArgs e)
    {
        Ambient.Play();
    }
}
