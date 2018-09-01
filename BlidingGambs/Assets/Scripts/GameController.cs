using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    private BeatManager _beatManager;

    // Use this for initialization
    private void Awake () 
    {
        _beatManager = GetComponent<BeatManager>();
	}

    // Update is called once per frame
    private void Update () {
		
	}

    public PlayerController GetPlayer()
    {
        return player;
    }

    public EnemyController GetEnemy()
    {
        return enemy;
    }

    public BeatManager GetBeatManager()
    {
        return _beatManager;
    }
}
