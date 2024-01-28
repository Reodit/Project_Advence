using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public bool IsVisible { get; private set; }

    public delegate void DataBindingHandler();
    public event DataBindingHandler OnDataBind;
    
    protected virtual void Start()
    {
        Initialize();
    }
    
    protected virtual void Initialize()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void BindData()
    {
        OnDataBind?.Invoke();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        IsVisible = true;
        OnShow();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        IsVisible = false;
        OnHide();
    }

    protected virtual void OnShow()
    {
    }

    protected virtual void OnHide()
    {
    }
    protected virtual void OnEnable()
    {
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void SubscribeEvents()
    {
    }

    protected virtual void UnsubscribeEvents()
    {
    }
}