using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPlayer : MonoBehaviour
{
    [SerializeField] Card chosenCard;
    public Transform AtkPosRef;
    // ==============================

    // =========== Health ===========
    public TMP_Text healthText;
    public HealthBar healthBar;
    public float Health;
    public float MaxHealth;
    // ==============================

    private Tweener animationTweener;

    public Attack? AttackValue
    {
        // get => chosenCard == null ? null : chosenCard.AttackValue;
        get
        {
            if (chosenCard == null)
            {
                return null;
            }
            else
                return chosenCard.AttackValue;
        }
    }

    // =================================================
    // script for logic Card
    public void Reset()
    {
        if (chosenCard = null)
        {
            chosenCard.Reset();
        }
        chosenCard = null;
    }

    public void SetchosenCard(Card newCard)
    {
        if (chosenCard != null)
        {
            chosenCard.Reset();
        }
        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.2f);
    }
    // =================================================

    // =================================================
    // script for logic Health
    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, 100);
        // HealthBar
        healthBar.UpdateBar(Health / MaxHealth);
        // Text health
        healthText.text = Health + "/" + MaxHealth;
    }
    // =================================================


    // ====================================================
    // Script For Animation
    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform.DOMove(AtkPosRef.position, 1);
    }

    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }

    internal void DamageAnimation()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetDelay(0.5f);
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.OriginalPos, 1)
            .SetEase(Ease.InBack)
            .SetDelay(0.2f);
    }

    internal void AnimateDraw()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.blue, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetDelay(0.5f);
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.OriginalPos, 1)
            .SetEase(Ease.InBack)
            .SetDelay(0.2f);
    }
    // ====================================================

    public void IsClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClikable(value);
        }
    }
}
