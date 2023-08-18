using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GM : MonoBehaviour
{
    public static GM instance;
    bool newG = false;
    public static GameObject playerObj;
    public static PlayerMovement playerMove;
    public static Player player;
    public static MovementAnimation playerAnim;
    public static PlayerStateManager playerState;
    public static PlayerMoney playerMoney;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null&&instance!=this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    
    public IEnumerator New(string charType, string playerName)
    {
        if (newG == true)
            yield break;
        newG = true;
        SceneManager.LoadScene("3. Game");
        yield return new WaitForEndOfFrame(); //wait for awake
        yield return new WaitForEndOfFrame(); //wait for start
        new NewGame(charType,playerName,NewGame.Type.New);
        newG = false;
        yield break;
    }
    public IEnumerator CCreation()
    {
        SceneManager.LoadScene("2. Character Creation");
        yield break;
    }

    private IEnumerator Load(int slot)
    {
        SceneManager.LoadScene("3. Game");
        yield return new WaitForEndOfFrame(); //wait for awake
        yield return new WaitForEndOfFrame(); //wait for start
        new LoadGame("player."+slot.ToString());
        yield break;
    }

    public void LoadGame(int slot)
    {
        StartCoroutine(Load(slot));
    }
    public void CharacterCreation()
    {
        StartCoroutine(CCreation());
    }
    public void NewGameMethod(string charType,string playerName)
    {
        StartCoroutine(New(charType,playerName));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
                new SaveGame("player.0");

            Application.Quit();
        }
    }

}
