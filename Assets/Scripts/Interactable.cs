using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    #region VARIABLES
    protected Renderer interactableRenderer;
    protected List<Material> materialHandler = new();

    private Material outlineMaterial;
    #endregion

    #region ABSTRACT METHODS
    public abstract void OnInteract();

    public virtual void OnFocus()
    {
        ShowOutline();
    }

    public virtual void OnLoseFocus()
    {
        RemoveOutline();
    }
    #endregion

    #region UNITY EVENT FUNCTIONS

    private void Awake()
    {
        this.gameObject.layer = 7; // interaction layer;

        Shader outlineShader = Shader.Find("Shader Graphs/OutlineShader");
        outlineMaterial = new Material(outlineShader);
    }

    public virtual void OnEnable()
    {
        interactableRenderer = GetComponent<Renderer>();

        materialHandler = interactableRenderer.materials.ToList();
    }

    #endregion

    #region METHODS

    void ShowOutline()
    {
        materialHandler.Add(outlineMaterial);
        interactableRenderer.materials = materialHandler.ToArray();
    }

    void RemoveOutline()
    {
        materialHandler.Remove(materialHandler.Last());
        interactableRenderer.materials = materialHandler.ToArray();
    }

    #endregion
}