using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class StoveCounter : BaseCounter
{
    public class StoveCounterEventArgs : EventArgs
    {
        public readonly FryingState state;

        public StoveCounterEventArgs(FryingState state)
        {
            this.state = state;
        }
    }

    public enum FryingState
    {
        None,
        Uncooked,
        Cooked,
        Burnt,
    }

    public static float SFXVolume { get; set; }

    public event EventHandler<StoveCounterEventArgs> OnFryingStateChanged;
    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    [SerializeField] private CounterProgressBar progressBar;
    [SerializeField] private AudioSource sizzleAudioSource;
    [SerializeField] private StoveCounterWarningUI warningUI;
    [SerializeField] private AudioSource warningAudioSource;
    private FryingRecipeSO currentRecipe;
    private float fryingTimer;
    private FryingState state;

    public override void Interact(Player player)
    {
        if (KitchenObject == null)
        {
            if (player.KitchenObject == null) return;
            currentRecipe = Array.Find(fryingRecipes, r => r.from == player.KitchenObject.UnderlyingKitchenObjectSO);
            if (currentRecipe == null) return;
            fryingTimer = 0;
            progressBar.SetProgress(0);
            player.KitchenObject.Parent = this;
        }
        else
        {
            if (player.KitchenObject != null)
            {
                if (player.KitchenObject is not Plate playerPlate) return;
                //Pick object to plate
                if (!playerPlate.TryAddIngredient(KitchenObject)) return;
                fryingTimer = 0;
                progressBar.SetProgress(0);
                return;
            }

            fryingTimer = 0;
            progressBar.SetProgress(0);
            KitchenObject.Parent = player;
        }
    }

    public void Update()
    {
        if (KitchenObject == null)
        {
            if (state != FryingState.None)
            {
                OnFryingStateChanged?.Invoke(this, new StoveCounterEventArgs(state = FryingState.None));
            }

            warningUI.IsShow = false;
            sizzleAudioSource.Pause();
            warningAudioSource.Pause();
            return;
        }

        //Frying
        sizzleAudioSource.clip = SFXManager.Instance.SFXTypeToAudioClip(SFXManager.SFXType.PanSizzle);
        sizzleAudioSource.volume = SFXVolume;
        warningAudioSource.volume = SFXVolume;
        if (!sizzleAudioSource.isPlaying) sizzleAudioSource.Play();
        if (currentRecipe.from != KitchenObject.UnderlyingKitchenObjectSO &&
            currentRecipe.to != KitchenObject.UnderlyingKitchenObjectSO &&
            currentRecipe.burnt != KitchenObject.UnderlyingKitchenObjectSO) Debug.Log("Wrong object");
        fryingTimer += Time.deltaTime;
        if (fryingTimer >= currentRecipe.fryingTimeBurnt)
        {
            //Burnt
            if (state != FryingState.Cooked) return;
            warningAudioSource.Stop();
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(currentRecipe.burnt, spawnPoint.position, this);
            OnFryingStateChanged?.Invoke(this, new StoveCounterEventArgs(state = FryingState.Burnt));
            return;
        }

        if (fryingTimer >= currentRecipe.fryingTimeGood)
        {
            //Good Cook
            if (state == FryingState.Uncooked)
            {
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(currentRecipe.to, spawnPoint.position, this);
                OnFryingStateChanged?.Invoke(this, new StoveCounterEventArgs(state = FryingState.Cooked));
            }


            //If over half time, display warning
            if (fryingTimer >= Mathf.Lerp(currentRecipe.fryingTimeGood, currentRecipe.fryingTimeBurnt, 0.5f))
            {
                warningUI.IsShow = true;
                if (!warningAudioSource.isPlaying)
                {
                    warningAudioSource.Play();
                }
            }

            progressBar.SetProgress((fryingTimer - currentRecipe.fryingTimeGood) /
                                    (currentRecipe.fryingTimeBurnt - currentRecipe.fryingTimeGood));
            progressBar.Color = Color.Lerp(Color.green, Color.red,
                (fryingTimer - currentRecipe.fryingTimeGood) /
                (currentRecipe.fryingTimeBurnt - currentRecipe.fryingTimeGood));
            return;
        }

        progressBar.SetProgress(fryingTimer / currentRecipe.fryingTimeGood);
        progressBar.Color = Color.Lerp(Color.white, Color.green, fryingTimer / currentRecipe.fryingTimeGood);
        if (state == FryingState.None)
        {
            OnFryingStateChanged?.Invoke(this, new StoveCounterEventArgs(state = FryingState.Uncooked));
        }
    }

    public override string GetInteractionName()
    {
        if (KitchenObject == null && Player.Instance.KitchenObject != null &&
            fryingRecipes.Any(r => r.from == Player.Instance.KitchenObject.UnderlyingKitchenObjectSO))
        {
            return "Fry";
        }
        if (KitchenObject != null && Player.Instance.KitchenObject == null)
        {
            return "Pick up";
        }
        return "None";
    }
}