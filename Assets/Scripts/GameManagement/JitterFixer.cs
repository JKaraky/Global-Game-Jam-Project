using UnityEditor;
using UnityEngine;

// This script fixes the jitter of the camera, it just does.
public class JitterFixer : MonoBehaviour
{
    void Start()
    {
        Selection.objects = new Object[] { this.gameObject };
    }
}
