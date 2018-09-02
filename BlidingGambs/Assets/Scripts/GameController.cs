using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    private BeatManager _beatManager;

    private void Awake() 
    {
        _beatManager = GetComponent<BeatManager>();
	}

    private void Start()
    {
        enemy.Init(this);
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
