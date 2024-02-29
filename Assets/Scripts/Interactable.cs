using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Renderer interactableRenderer;
    protected Shader outlineShader;
    protected List<Material> materialHandler = new();
    private Material outlineMaterial;

    private void Awake()
    {
        this.gameObject.layer = 7; // interaction layer;
        
        outlineShader = Shader.Find("Shader Graphs/OutlineShader");
        outlineMaterial = new Material(outlineShader);
    }


    public abstract void OnInteract();
    public virtual void OnFocus() 
    {         
        materialHandler.Add(outlineMaterial);
        interactableRenderer.materials = materialHandler.ToArray();
    }
    
    public virtual void OnLoseFocus()
    {
        materialHandler.Remove(materialHandler.Last());
        interactableRenderer.materials = materialHandler.ToArray();
    }

    public virtual void OnEnable()
    {
        interactableRenderer = GetComponent<Renderer>();

        materialHandler = interactableRenderer.materials.ToList();
    }
}
