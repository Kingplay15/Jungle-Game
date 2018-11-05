using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance = null;

    AudioSource GameSource;
    AudioSource BGSoundTrack;

    [SerializeField] AudioClip MouseClick;

    //Items
    [SerializeField] AudioClip Chest;
    [SerializeField] AudioClip Gold;
    [SerializeField] AudioClip Weapon;
    [SerializeField] AudioClip Shield;
    [SerializeField] AudioClip PotionPickedUp;
    [SerializeField] AudioClip PotionUsed;

    //Player
    [SerializeField] AudioClip PlayerGetHurt;
    [SerializeField] AudioClip PlayerDie;
    [SerializeField] AudioClip LevelUp;

    [SerializeField] AudioClip Victory;
    [SerializeField] AudioClip GameOver;

    void Awake()
    {
        //There has to be only ONE instance of SoundController in the game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        GameSource = GetComponent<AudioSource>();
    }

    public void FindBGSoundTrack()
    {
        BGSoundTrack = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }

    public void PlayMainMenu()
    {
        BGSoundTrack.PlayDelayed(5.5f);
    }

    public void PlayMouseClick()
    {
        GameSource.PlayOneShot(MouseClick);
    }

    public void PlayChest()
    {
        GameSource.PlayOneShot(Chest);
    }

    public void PlayGold()
    {
        GameSource.PlayOneShot(Gold);
    }

    public void PlayWeapon()
    {
        GameSource.PlayOneShot(Weapon);
    }

    public void PlayShield()
    {
        GameSource.PlayOneShot(Shield);
    }

    public void PlayPotionPickedUp()
    {
        GameSource.PlayOneShot(PotionPickedUp);
    }

    public void PlayPotionUsed()
    {
        GameSource.PlayOneShot(PotionUsed);
    }

    public void PlayPlayerGetHurt()
    {
        GameSource.PlayOneShot(PlayerGetHurt);
    }

    public void PlayPlayerDie()
    {
        GameSource.PlayOneShot(PlayerDie);
    }

    public void PlayLevelUp()
    {
        GameSource.PlayOneShot(LevelUp);
    }

    public void PlayVictory()
    {
        GameSource.PlayOneShot(Victory);
    }

    public void PlayGameOver()
    {
        GameSource.PlayOneShot(GameOver);
    }
}
