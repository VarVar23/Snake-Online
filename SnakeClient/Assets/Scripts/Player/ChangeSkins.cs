using System.Collections.Generic;
using UnityEngine;

public class ChangeSkins : MonoBehaviour
{

    [SerializeField] private Material[] _allSkins;
    private List<MeshRenderer> _snakeMeshRenderers;

    public void Init(List<MeshRenderer> snakeMeshRenderers)
    {
        _snakeMeshRenderers = snakeMeshRenderers;
        SetSkin(Random.Range(0, _allSkins.Length));
    }

    public void SetSkin(int index)
    {
        if(index < 0 || index >= _allSkins.Length)
        {
            foreach (var mesh in _snakeMeshRenderers) mesh.material.color = _allSkins[0].color;
            return;
        }

        foreach (var mesh in _snakeMeshRenderers) mesh.material.color = _allSkins[index].color;
    }

    public int GetCountMaterials() => _allSkins.Length;

    public void Send(int index)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", index }
        };

        MultiplayerManager.Instance.SendMessage("changeColor", data);
    }
}
