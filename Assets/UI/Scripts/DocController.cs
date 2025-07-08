using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DocController : MonoBehaviour
{
    UIDocument doc;
    VisualElement root;

    public UIDocument Doc { get => doc; }
    public VisualElement Root { get => root; protected set => root = value; }

    protected virtual void Awake()
    {
        doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;

        if(root != null)
            SetComponents();
    }

    protected abstract void SetComponents();

    public virtual void ShowDoc(bool show)
    {
        root.style.display = show ? root.style.display = DisplayStyle.Flex : root.style.display = DisplayStyle.None;
    }

}
