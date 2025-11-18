using UnityEngine;

public class SensitivityAdapter : MonoBehaviour
{
    public enum Mode { FloatFieldNamedMouseSensitivity, FloatFieldNamedSensitivity, CharacterControllerLike }
    [SerializeField] private Mode mode = Mode.FloatFieldNamedSensitivity;
    [SerializeField] private MonoBehaviour target; // posa aquí el teu script de mirada

    // Truca-ho quan canviï la sensibilitat
    public void SetSensitivity(float value)
    {
        if (target == null) return;

        var t = target.GetType();
        switch (mode)
        {
            case Mode.FloatFieldNamedMouseSensitivity:
                var f1 = t.GetField("mouseSensitivity");
                if (f1 != null && f1.FieldType == typeof(float)) f1.SetValue(target, value);
                break;
            case Mode.FloatFieldNamedSensitivity:
                var f2 = t.GetField("sensitivity");
                if (f2 != null && f2.FieldType == typeof(float)) f2.SetValue(target, value);
                break;
            case Mode.CharacterControllerLike:
                var p = t.GetProperty("Sensitivity");
                if (p != null && p.PropertyType == typeof(float) && p.CanWrite) p.SetValue(target, value);
                break;
        }
    }
}
