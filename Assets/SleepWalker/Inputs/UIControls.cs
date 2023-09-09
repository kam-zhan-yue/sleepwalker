//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/SleepWalker/Inputs/UIControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UIControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UIControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UIControls"",
    ""maps"": [
        {
            ""name"": ""UIInput"",
            ""id"": ""5c8d3aad-a5f1-4293-b6ed-e9a63ba49c0a"",
            ""actions"": [
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""ded115b6-6a2d-4a12-8c81-d4293d87c786"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""89ad9bf0-d861-483a-ba0b-aaa96f0087d4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UIInput
        m_UIInput = asset.FindActionMap("UIInput", throwIfNotFound: true);
        m_UIInput_Next = m_UIInput.FindAction("Next", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // UIInput
    private readonly InputActionMap m_UIInput;
    private List<IUIInputActions> m_UIInputActionsCallbackInterfaces = new List<IUIInputActions>();
    private readonly InputAction m_UIInput_Next;
    public struct UIInputActions
    {
        private @UIControls m_Wrapper;
        public UIInputActions(@UIControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Next => m_Wrapper.m_UIInput_Next;
        public InputActionMap Get() { return m_Wrapper.m_UIInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIInputActions set) { return set.Get(); }
        public void AddCallbacks(IUIInputActions instance)
        {
            if (instance == null || m_Wrapper.m_UIInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIInputActionsCallbackInterfaces.Add(instance);
            @Next.started += instance.OnNext;
            @Next.performed += instance.OnNext;
            @Next.canceled += instance.OnNext;
        }

        private void UnregisterCallbacks(IUIInputActions instance)
        {
            @Next.started -= instance.OnNext;
            @Next.performed -= instance.OnNext;
            @Next.canceled -= instance.OnNext;
        }

        public void RemoveCallbacks(IUIInputActions instance)
        {
            if (m_Wrapper.m_UIInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIInputActions instance)
        {
            foreach (var item in m_Wrapper.m_UIInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIInputActions @UIInput => new UIInputActions(this);
    public interface IUIInputActions
    {
        void OnNext(InputAction.CallbackContext context);
    }
}