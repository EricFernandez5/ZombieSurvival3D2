using UnityEngine;

public static class SettingsService
{
    // Valors per a la sessió actual (no es persisteixen)
    public static bool MusicaActiva = true;
    public static float VolumMusica = 0.8f;      // 0..1
    public static float Sensibilitat = 1.0f;     // escala multiplicadora

    // Helper opcional per aplicar ràpidament al so
    public static void AplicaAlAudioSource(AudioSource musicSource)
    {
        if (musicSource == null) return;
        musicSource.mute = !MusicaActiva;
        musicSource.volume = VolumMusica;
    }
}
