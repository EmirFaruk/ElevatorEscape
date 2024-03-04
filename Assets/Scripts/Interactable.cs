using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    #region VARIABLES
    protected Renderer interactableRenderer;
    [SerializeField] protected List<Renderer> interactableRenderers = new();
    protected List<Material> materialHandler = new();
    [SerializeField] protected List<Material>[] materialHandlers;

    private Material outlineMaterial;
    private bool hasRenderer;
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
        //interactableRenderer = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>();
        if (TryGetComponent(out Renderer renderer))
        {
            hasRenderer = true;
            interactableRenderer = renderer;
            materialHandler = interactableRenderer.materials.ToList();
        }
        else
        {
            hasRenderer = false;

            foreach (Transform item in transform)
            {
                if (item.TryGetComponent(out Renderer renderer1))
                    interactableRenderers.Add(renderer1);
                print(interactableRenderers.Count + " : " + interactableRenderers);
            }

            materialHandlers = new List<Material>[interactableRenderers.Count];
            for (int i = 0; i < interactableRenderers.Count; i++)
                materialHandlers[i] = interactableRenderers[i].materials.ToList();
        }
    }

    #endregion

    #region METHODS

    void ShowOutline()
    {
        if (hasRenderer)
        {
            materialHandler.Add(outlineMaterial);
            interactableRenderer.materials = materialHandler.ToArray();
        }
        else
        {
            for (int i = 0; i < materialHandlers.Length; i++)
            {
                materialHandlers[i].Add(outlineMaterial);
                interactableRenderers[i].materials = materialHandlers[i].ToArray();
            }
        }
    }

    void RemoveOutline()
    {
        if (hasRenderer)
        {
            materialHandler.Remove(materialHandler.Last());
            interactableRenderer.materials = materialHandler.ToArray();
        }
        else
        {
            for (int i = 0; i < materialHandlers.Length; i++)
            {
                materialHandlers[i].Remove(materialHandlers[i].Last());
                interactableRenderers[i].materials = materialHandlers[i].ToArray();
            }
        }
    }

    #endregion
}