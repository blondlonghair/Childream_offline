using Unity.VisualScripting;
using UnityEngine;

public class Helper
{
    public static bool Approximately(Transform a, PRS b)
    {
        var b1 = (double) Mathf.Abs(b.pos.x - a.position.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.x), Mathf.Abs(b.pos.x)), Mathf.Epsilon * 8f);
        var b2 = (double) Mathf.Abs(b.pos.y - a.position.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.y), Mathf.Abs(b.pos.y)), Mathf.Epsilon * 8f);
        var b3 = (double) Mathf.Abs(b.pos.z - a.position.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.z), Mathf.Abs(b.pos.z)), Mathf.Epsilon * 8f);

        var b4 = (double) Mathf.Abs(b.rot.x - a.rotation.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.x), Mathf.Abs(b.rot.x)), Mathf.Epsilon * 8f);
        var b5 = (double) Mathf.Abs(b.rot.y - a.rotation.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.y), Mathf.Abs(b.rot.y)), Mathf.Epsilon * 8f);
        var b6 = (double) Mathf.Abs(b.rot.z - a.rotation.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.z), Mathf.Abs(b.rot.z)), Mathf.Epsilon * 8f);

        var b7 = (double) Mathf.Abs(b.scale.x - a.localScale.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.x), Mathf.Abs(b.scale.x)), Mathf.Epsilon * 8f);
        var b8 = (double) Mathf.Abs(b.scale.y - a.localScale.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.y), Mathf.Abs(b.scale.y)), Mathf.Epsilon * 8f);
        var b9 = (double) Mathf.Abs(b.scale.z - a.localScale.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.z), Mathf.Abs(b.scale.z)), Mathf.Epsilon * 8f);

        return b1 && b2 && b3 && b4 && b5 && b6 && b7 && b8 && b9;
    }
    
    public static bool Approximately(Transform a, Transform b)
    {
        var b1 = (double) Mathf.Abs(b.position.x - a.position.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.x), Mathf.Abs(b.position.x)), Mathf.Epsilon * 8f);
        var b2 = (double) Mathf.Abs(b.position.y - a.position.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.y), Mathf.Abs(b.position.y)), Mathf.Epsilon * 8f);
        var b3 = (double) Mathf.Abs(b.position.z - a.position.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.position.z), Mathf.Abs(b.position.z)), Mathf.Epsilon * 8f);

        var b4 = (double) Mathf.Abs(b.rotation.x - a.rotation.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.x), Mathf.Abs(b.rotation.x)), Mathf.Epsilon * 8f);
        var b5 = (double) Mathf.Abs(b.rotation.y - a.rotation.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.y), Mathf.Abs(b.rotation.y)), Mathf.Epsilon * 8f);
        var b6 = (double) Mathf.Abs(b.rotation.z - a.rotation.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.rotation.z), Mathf.Abs(b.rotation.z)), Mathf.Epsilon * 8f);

        var b7 = (double) Mathf.Abs(b.localScale.x - a.localScale.x) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.x), Mathf.Abs(b.localScale.x)), Mathf.Epsilon * 8f);
        var b8 = (double) Mathf.Abs(b.localScale.y - a.localScale.y) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.y), Mathf.Abs(b.localScale.y)), Mathf.Epsilon * 8f);
        var b9 = (double) Mathf.Abs(b.localScale.z - a.localScale.z) < (double)
            Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a.localScale.z), Mathf.Abs(b.localScale.z)), Mathf.Epsilon * 8f);

        return b1 && b2 && b3 && b4 && b5 && b6 && b7 && b8 && b9;
    }
}

public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public int index;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale, int index)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
        this.index = index;
    }
}