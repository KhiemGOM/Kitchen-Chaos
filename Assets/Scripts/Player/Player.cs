using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<SelectEventArgs> OnSelect;
    public event EventHandler OnInteract;


    public KitchenObject KitchenObject { get; set; }

    public class SelectEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter { get; set; }
    }

    [SerializeField] private float normalMovementSpeed = 5f;
    [SerializeField] private float sprintMovementSpeed = 10f;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private LayerMask counterLayerMask = 0;
    [SerializeField] private GameInput gameInput;
    private BaseCounter selectedCounter;
    private bool isWalking;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should only be one PlayerControl in the scene!");
        }

        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteract += GameInputOnInteract;
        gameInput.OnInteractAlt += GameInputOnOnInteractAlt;
    }


    private void Update()
    {
        HandleMovement();
        HandleInteractionVisual();
    }

    private void GameInputOnOnInteractAlt(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsPlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void GameInputOnInteract(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsPlaying()) return;
        if (selectedCounter == null) return;
        selectedCounter.Interact(this);
        OnInteract?.Invoke(this, EventArgs.Empty);
    }


    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        var movementSpeed = gameInput.IsSprintKeyPressed()? sprintMovementSpeed : normalMovementSpeed;
        var moveVector = gameInput.GetMovementVectorNormalized();
        var tTransform = transform;
        var position = tTransform.position;
        const float playerRadius = .7f;
        const float playerHeight = 2f;
        var moveDisplacement = moveVector * (Time.deltaTime * movementSpeed);
        var isSlide = Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveVector,
            out var hitInfo, moveDisplacement.magnitude);
        var slideVector = moveVector -
                          Vector3.Dot(moveVector, hitInfo.normal) / hitInfo.normal.sqrMagnitude * hitInfo.normal;
        var slideDisplacement = slideVector * (Time.deltaTime * movementSpeed);
        isWalking = moveVector != Vector3.zero;
        var angle = Vector3.Slerp(tTransform.forward, moveVector, Time.deltaTime * rotationSpeed);
        //Handle actually moving the player
        tTransform.LookAt(position + angle);
        if (!Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius,
                isSlide ? slideDisplacement : moveDisplacement,
                isSlide ? slideDisplacement.magnitude : moveDisplacement.magnitude, counterLayerMask))
        {
            tTransform.Translate(isSlide ? slideDisplacement : moveDisplacement, Space.World);
        }
    }

    private void HandleInteractionVisual()
    {
        const float interactionRange = 3f;
        const float interactionSphereRadius = .5f;
        var _transform = transform;
        if (Physics.SphereCast(_transform.position, interactionSphereRadius, _transform.forward, out var hitInfo,
                interactionRange,
                counterLayerMask))
        {
            if (hitInfo.transform.TryGetComponent<BaseCounter>(out var counterFromRaycast))
            {
                if (counterFromRaycast != selectedCounter)
                {
                    SetSelectedCounter(counterFromRaycast);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter = counter;
        OnSelect?.Invoke(this, new SelectEventArgs { SelectedCounter = selectedCounter });
    }

    public Transform GetFollowSpawnPoint()
    {
        return spawnPoint;
    }
}