using UnityEngine;
using UnityEngine.Networking;



public class Player : NetworkBehaviour {


    #region Refrerances
 
    [SyncVar]
    private bool _isDead = false;
    public PlayerStats stats;
    private int maxHealth = 100;
    [SyncVar][HideInInspector]
    public float currentHealth;
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject deathEffect;
    [HideInInspector]
    public GameOver gameOver;

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    #endregion

    private void Start()
    {
        if (isLocalPlayer)
        {
            stats = PlayFabController.PFC.stats;
        }
        gameOver = GetComponent<GameOver>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            stats = PlayFabController.PFC.stats;
        }
    }
    #region Setup

    public void Setup()
    {       
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
       SetDefeults();
       
    }

    public void SetDefeults()
    {
        _isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);

        }

        if (isLocalPlayer)
        {
            CmdSetUsername(stats.username, stats.coins, stats.stars, stats.level);
        }

    }

    [Command]
    void CmdSetUsername(string name, int coins, int stars, int lvl)
    {
        stats.username = name;
        stats.coins = coins;
        stats.stars = stars;
        stats.level = lvl;
    }

    #endregion

    [ClientRpc]
    public void RpcTakeDamage (float _amount)
    {
        if (_isDead)
            return;

        currentHealth -= _amount;

     //   Debug.Log(transform.name + " has now " + currentHealth + " health");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die ()
    {
        isDead = true;
       
         for (int i = 0; i < disableOnDeath.Length; i++)
       {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

       GameObject _gfx = (GameObject) Instantiate(deathEffect, weapon.transform.position, Quaternion.identity);
        Destroy(_gfx, 2f);

        Debug.Log(transform.name + "is dead");

        gameOver.Lose();
     
    }


}
