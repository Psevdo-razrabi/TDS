//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Input/InputSystem.inputactions
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

public partial class @InputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""6e450952-ba26-482f-9847-bc0f84500717"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""7f768b78-22d3-484c-996c-1be8326ed7fd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""04fbddb3-c551-4123-9457-1c57a4b18f64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""4c86df98-aca3-4c62-97d2-ed09367b6eb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32b4ff42-1a46-42d5-9154-d3396e3981cc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db670fb-9673-4110-804f-ad5d621cd5bc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94a12ead-9968-4574-9afc-888559e49893"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Movement"",
            ""id"": ""06a0c42a-48d7-4f58-84d0-946279a6d6d6"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""63fc6ae5-9830-4759-aff6-cd5e90037c7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""db2ac6d7-8bb1-4d36-a08d-c9b6c19654e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouching"",
                    ""type"": ""Button"",
                    ""id"": ""23ad8b5a-1faf-4dfb-bcd2-9ab6bff6e36b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Clamb"",
                    ""type"": ""Button"",
                    ""id"": ""7659a99a-7a21-4929-a3c5-2cee5449e027"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5db3dc92-ba30-42b0-a48b-7dc42de76b92"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5cf5d1c-3645-439f-ae62-36b10a1fe88b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfff9625-e3ba-47ef-bb04-6f5f6076e8ad"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouching"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""587a1505-aa73-42f4-8105-4ba7bdcc841c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clamb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Weapon"",
            ""id"": ""c9768721-b1d5-4862-b4d9-b6e1a077aeeb"",
            ""actions"": [
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""94ba69bd-f429-436a-b27b-47ca3e815f05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""8efa953b-84bc-4e85-b1f3-5b9d97f9818a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeFireMode"",
                    ""type"": ""Button"",
                    ""id"": ""48aa6124-89fa-4f42-afcb-d168323a915b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""9122eff0-153b-48a1-8ff4-2de888826569"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""161f97f1-9bda-4882-88bb-512088b811a6"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6321b31-0c4e-4ab3-9d16-a452586e91c0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93d7b34f-3a39-40b8-b109-879e6cd3a1fb"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeFireMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef03e304-d9b3-406f-87c7-d545ca79e188"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""7e52b540-a282-4c02-b08a-d71cdc80d9af"",
            ""actions"": [
                {
                    ""name"": ""HideStorage"",
                    ""type"": ""Button"",
                    ""id"": ""86b74f74-f155-441b-87b3-e4ce7e43d048"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ca8c89d0-ae51-4697-90d7-d331b7ba7452"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HideStorage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_MousePosition = m_Mouse.FindAction("MousePosition", throwIfNotFound: true);
        m_Mouse_Aim = m_Mouse.FindAction("Aim", throwIfNotFound: true);
        m_Mouse_Shoot = m_Mouse.FindAction("Shoot", throwIfNotFound: true);
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        m_Movement_Dash = m_Movement.FindAction("Dash", throwIfNotFound: true);
        m_Movement_Crouching = m_Movement.FindAction("Crouching", throwIfNotFound: true);
        m_Movement_Clamb = m_Movement.FindAction("Clamb", throwIfNotFound: true);
        // Weapon
        m_Weapon = asset.FindActionMap("Weapon", throwIfNotFound: true);
        m_Weapon_Reload = m_Weapon.FindAction("Reload", throwIfNotFound: true);
        m_Weapon_Fire = m_Weapon.FindAction("Fire", throwIfNotFound: true);
        m_Weapon_ChangeFireMode = m_Weapon.FindAction("ChangeFireMode", throwIfNotFound: true);
        m_Weapon_ChangeWeapon = m_Weapon.FindAction("ChangeWeapon", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_HideStorage = m_UI.FindAction("HideStorage", throwIfNotFound: true);
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

    // Mouse
    private readonly InputActionMap m_Mouse;
    private List<IMouseActions> m_MouseActionsCallbackInterfaces = new List<IMouseActions>();
    private readonly InputAction m_Mouse_MousePosition;
    private readonly InputAction m_Mouse_Aim;
    private readonly InputAction m_Mouse_Shoot;
    public struct MouseActions
    {
        private @InputSystem m_Wrapper;
        public MouseActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Mouse_MousePosition;
        public InputAction @Aim => m_Wrapper.m_Mouse_Aim;
        public InputAction @Shoot => m_Wrapper.m_Mouse_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void AddCallbacks(IMouseActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseActionsCallbackInterfaces.Add(instance);
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
        }

        private void UnregisterCallbacks(IMouseActions instance)
        {
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
        }

        public void RemoveCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseActions @Mouse => new MouseActions(this);

    // Movement
    private readonly InputActionMap m_Movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_Movement_Jump;
    private readonly InputAction m_Movement_Dash;
    private readonly InputAction m_Movement_Crouching;
    private readonly InputAction m_Movement_Clamb;
    public struct MovementActions
    {
        private @InputSystem m_Wrapper;
        public MovementActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputAction @Dash => m_Wrapper.m_Movement_Dash;
        public InputAction @Crouching => m_Wrapper.m_Movement_Crouching;
        public InputAction @Clamb => m_Wrapper.m_Movement_Clamb;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Crouching.started += instance.OnCrouching;
            @Crouching.performed += instance.OnCrouching;
            @Crouching.canceled += instance.OnCrouching;
            @Clamb.started += instance.OnClamb;
            @Clamb.performed += instance.OnClamb;
            @Clamb.canceled += instance.OnClamb;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Crouching.started -= instance.OnCrouching;
            @Crouching.performed -= instance.OnCrouching;
            @Crouching.canceled -= instance.OnCrouching;
            @Clamb.started -= instance.OnClamb;
            @Clamb.performed -= instance.OnClamb;
            @Clamb.canceled -= instance.OnClamb;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Weapon
    private readonly InputActionMap m_Weapon;
    private List<IWeaponActions> m_WeaponActionsCallbackInterfaces = new List<IWeaponActions>();
    private readonly InputAction m_Weapon_Reload;
    private readonly InputAction m_Weapon_Fire;
    private readonly InputAction m_Weapon_ChangeFireMode;
    private readonly InputAction m_Weapon_ChangeWeapon;
    public struct WeaponActions
    {
        private @InputSystem m_Wrapper;
        public WeaponActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Reload => m_Wrapper.m_Weapon_Reload;
        public InputAction @Fire => m_Wrapper.m_Weapon_Fire;
        public InputAction @ChangeFireMode => m_Wrapper.m_Weapon_ChangeFireMode;
        public InputAction @ChangeWeapon => m_Wrapper.m_Weapon_ChangeWeapon;
        public InputActionMap Get() { return m_Wrapper.m_Weapon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponActions set) { return set.Get(); }
        public void AddCallbacks(IWeaponActions instance)
        {
            if (instance == null || m_Wrapper.m_WeaponActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Add(instance);
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @ChangeFireMode.started += instance.OnChangeFireMode;
            @ChangeFireMode.performed += instance.OnChangeFireMode;
            @ChangeFireMode.canceled += instance.OnChangeFireMode;
            @ChangeWeapon.started += instance.OnChangeWeapon;
            @ChangeWeapon.performed += instance.OnChangeWeapon;
            @ChangeWeapon.canceled += instance.OnChangeWeapon;
        }

        private void UnregisterCallbacks(IWeaponActions instance)
        {
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @ChangeFireMode.started -= instance.OnChangeFireMode;
            @ChangeFireMode.performed -= instance.OnChangeFireMode;
            @ChangeFireMode.canceled -= instance.OnChangeFireMode;
            @ChangeWeapon.started -= instance.OnChangeWeapon;
            @ChangeWeapon.performed -= instance.OnChangeWeapon;
            @ChangeWeapon.canceled -= instance.OnChangeWeapon;
        }

        public void RemoveCallbacks(IWeaponActions instance)
        {
            if (m_Wrapper.m_WeaponActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWeaponActions instance)
        {
            foreach (var item in m_Wrapper.m_WeaponActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WeaponActions @Weapon => new WeaponActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_HideStorage;
    public struct UIActions
    {
        private @InputSystem m_Wrapper;
        public UIActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @HideStorage => m_Wrapper.m_UI_HideStorage;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @HideStorage.started += instance.OnHideStorage;
            @HideStorage.performed += instance.OnHideStorage;
            @HideStorage.canceled += instance.OnHideStorage;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @HideStorage.started -= instance.OnHideStorage;
            @HideStorage.performed -= instance.OnHideStorage;
            @HideStorage.canceled -= instance.OnHideStorage;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IMouseActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
    public interface IMovementActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnCrouching(InputAction.CallbackContext context);
        void OnClamb(InputAction.CallbackContext context);
    }
    public interface IWeaponActions
    {
        void OnReload(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnChangeFireMode(InputAction.CallbackContext context);
        void OnChangeWeapon(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnHideStorage(InputAction.CallbackContext context);
    }
}
