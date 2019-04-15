using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gate))]
public class GateEditor : Editor
{
    private void DrawBounds(Bounds b)
    {
        Vector3 tlf = new Vector3(b.max.x, b.max.y, b.max.z);
        Vector3 trf = new Vector3(b.min.x, b.max.y, b.max.z);
        Vector3 blf = new Vector3(b.max.x, b.min.y, b.max.z);
        Vector3 brf = new Vector3(b.min.x, b.min.y, b.max.z);
        Vector3 tlr = new Vector3(b.max.x, b.max.y, b.min.z);
        Vector3 trr = new Vector3(b.min.x, b.max.y, b.min.z);
        Vector3 blr = new Vector3(b.max.x, b.min.y, b.min.z);
        Vector3 brr = new Vector3(b.min.x, b.min.y, b.min.z);

        // top face
        Handles.DrawPolyLine(tlf, trf, trr, tlr, tlf);
        // bottom face
        Handles.DrawPolyLine(blf, brf, brr, blr, blf);
        // vertical edges
        Handles.DrawLine(tlf, blf);
        Handles.DrawLine(tlr, blr);
        Handles.DrawLine(trf, brf);
        Handles.DrawLine(trr, brr);
    }

    private void DrawSide(Bounds b, Direction d)
    {
        Vector3 tlf = new Vector3(b.max.x, b.max.y, b.max.z);
        Vector3 trf = new Vector3(b.min.x, b.max.y, b.max.z);
        Vector3 blf = new Vector3(b.max.x, b.min.y, b.max.z);
        Vector3 brf = new Vector3(b.min.x, b.min.y, b.max.z);
        Vector3 tlr = new Vector3(b.max.x, b.max.y, b.min.z);
        Vector3 trr = new Vector3(b.min.x, b.max.y, b.min.z);
        Vector3 blr = new Vector3(b.max.x, b.min.y, b.min.z);
        Vector3 brr = new Vector3(b.min.x, b.min.y, b.min.z);

        switch (d)
        {
            case Direction.Front:
                Handles.DrawLine(tlf, brf);
                Handles.DrawLine(trf, blf);
                break;
            case Direction.Back:
                Handles.DrawLine(tlr, brr);
                Handles.DrawLine(trr, blr);
                break;
            case Direction.Left:
                Handles.DrawLine(blr, tlf);
                Handles.DrawLine(blf, tlr);
                break;
            case Direction.Right:
                Handles.DrawLine(brr, trf);
                Handles.DrawLine(brf, trr);
                break;
            case Direction.Top:
                Handles.DrawLine(tlf, tlf);
                Handles.DrawLine(tlf, tlf);
                break;
            case Direction.Bottom:
                Handles.DrawLine(tlf, tlf);
                Handles.DrawLine(tlf, tlf);
                break;
        }
    }

    private void OnSceneGUI()
    {
        Gate g = target as Gate;
        if (g == null) return;

        Handles.color = Color.green;
        DrawBounds(g.Entrance.bounds);
        DrawBounds(g.Exit.bounds);
        Handles.color = Color.white;
        DrawSide(g.Entrance.bounds, g.EnterDirection);
        DrawSide(g.Exit.bounds, g.ExitDirection);
    }
}