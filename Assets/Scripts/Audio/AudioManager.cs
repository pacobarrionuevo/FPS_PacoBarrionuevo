using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource rocketLauncherSound;
    [SerializeField] private AudioSource shotgunSound;
    [SerializeField] private AudioSource rifleSound;
    [SerializeField] private AudioSource pistolEmptyingSound;
    [SerializeField] private AudioSource emptyGun;
    [SerializeField] private AudioSource reloadAmmo;

    private void Awake()
    {
        Instance = this;
        /*
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        */
    }

    public void PlayPistolShootingSound()
    {
        pistolSound.Play();
    }

    public void PlayRocketLauncherShootingSound()
    {
        rocketLauncherSound.Play();
    }

    public void PlayRifleShootingSound()
    {
        rifleSound.Play();
    }

    public void PlayShotgunShootingSound()
    {
        shotgunSound.Play();
    }

    public void PlayPistolEmptyingSound()
    {
        pistolEmptyingSound.Play();
    }

    public void PlayEmptyGunSound()
    {
        emptyGun.Play();
    }

    /// <summary>
    /// Sound produced when you pick up an ammo clip
    /// </summary>
    public void PlayReloadAmmoSound()
    {
        reloadAmmo.Play();
    }
}
