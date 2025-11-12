using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Referències UI")]
    [SerializeField] private Toggle toggleMusica;          // "Música"
    [SerializeField] private Slider sliderVolum;           // "Volum"
    [SerializeField] private Button btnCancelar;           // "Cancel·lar"
    [SerializeField] private Button btnDesar;              // "Desar"

    [Header("Objectes del nivell")]
    [SerializeField] private AudioSource musicSource;      // el AudioSource de la música del nivell
   

    // Còpia temporal per a Cancel·lar
    private bool musicaActiva_backup;
    private float volum_backup;
  

    private void Awake()
    {
        // Si no s’han assignat des de l’Inspector, intenta trobar-los
        if (musicSource == null)
            musicSource = GameObject.FindGameObjectWithTag("Music")?.GetComponent<AudioSource>();

        // Inicialitza listeners
        if (toggleMusica) toggleMusica.onValueChanged.AddListener(OnToggleMusica);
        if (sliderVolum)  sliderVolum.onValueChanged.AddListener(OnVolum);
      

        if (btnCancelar) btnCancelar.onClick.AddListener(OnCancelar);
        if (btnDesar)    btnDesar.onClick.AddListener(OnDesar);
    }

    private void OnEnable()
    {
        // Guardem estat previ per poder Cancel·lar
        musicaActiva_backup = SettingsService.MusicaActiva;
        volum_backup = SettingsService.VolumMusica;
        

        // Pintem UI des de l’estat actual
        if (toggleMusica) toggleMusica.isOn = SettingsService.MusicaActiva;
        if (sliderVolum)  sliderVolum.value = SettingsService.VolumMusica;
       

        // Apliquem a objectes (perquè l’usuari vegi l’efecte en temps real)
        ApplyLive();
    }

    // ——— Callbacks UI (canvi “live”, es pot revertir amb Cancel·lar) ———
    private void OnToggleMusica(bool on)
    {
        SettingsService.MusicaActiva = on;
        ApplyLive();
    }

    private void OnVolum(float v)
    {
        SettingsService.VolumMusica = v;
        ApplyLive();
    }


    private void ApplyLive()
    {
        // So
        SettingsService.AplicaAlAudioSource(musicSource);

    }

    // ——— Botons ———
    private void OnCancelar()
    {
        // Reverteix
        SettingsService.MusicaActiva = musicaActiva_backup;
        SettingsService.VolumMusica = volum_backup;
    
        ApplyLive();

        // Tanquem el menú d’opcions (el PauseMenu tornarà al panell principal)
        gameObject.SetActive(false);
    }

    private void OnDesar()
    {
        // Ja està “desat” a la sessió (SettingsService),
        // només apliquem i tanquem.
        ApplyLive();
        gameObject.SetActive(false);
    }
}
