using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMesh : MonoBehaviour
{
    public Mesh[] meshes;
    private MeshFilter _mf;

    public float changeSpeed = 0.1f;

    private int currentMesh;

    public Coroutine switchMeshCoroutine;
    void Start()
    {
        _mf = GetComponent<MeshFilter>();

        switchMeshCoroutine = StartCoroutine(SwitchMesh());
    }

    IEnumerator SwitchMesh()
    {
        while(true){
        currentMesh++;

        _mf.mesh = meshes[currentMesh % meshes.Length];
        yield return new WaitForSeconds(changeSpeed);
        }
    }

}
