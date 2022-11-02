using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState State = GameState.ChooseAttack;
    public CardPlayer P1;
    public CardPlayer P2;
    private CardPlayer damagedPlayer;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;

    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver
    }

    public void Start()
    {
        Debug.Log(State);
        gameOverPanel.SetActive(false);
    }

    public void Update()
    {
        switch (State)
        {
            case GameState.ChooseAttack:
                if (P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.IsClickable(false);
                    P2.IsClickable(false);
                    State = GameState.Attacks;
                }
                break;
            case GameState.Attacks:
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    Debug.Log(damagedPlayer);
                    if (damagedPlayer != null)
                    {
                        damagedPlayer.DamageAnimation();
                        P1.DamageAnimation();
                        P2.DamageAnimation();
                        State = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }

                break;
            case GameState.Damages:
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    // calculate damage
                    if (damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-10);
                        P2.ChangeHealth(5);
                    }
                    else
                    {
                        P1.ChangeHealth(5);
                        P2.ChangeHealth(-10);
                    }

                    // check game over
                    var winner = GetWinner();

                    if (winner == null)
                    {
                        ResetPlayers();
                        P1.IsClickable(true);
                        P2.IsClickable(true);
                        State = GameState.ChooseAttack;
                    }
                    else
                    {
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "Player 1 wins" : "Player 2 wins";
                        ResetPlayers();
                        State = GameState.GameOver;
                    }
                }

                break;
            case GameState.Draw:

                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayers();
                    P1.IsClickable(true);
                    P2.IsClickable(true);
                    State = GameState.ChooseAttack;
                }
                break;
        }
    }

    // untuk mereset player jika tidak ada yang menang
    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    //logic untuk menentukan siapa yang kena damage 
    private CardPlayer GetDamagedPlayer()
    {
        Attack? Player1Atk = P1.AttackValue;
        Attack? Player2Atk = P2.AttackValue;

        if (Player1Atk == Attack.Rock && Player2Atk == Attack.Paper)
        {
            return P1;
        }
        else if (Player1Atk == Attack.Rock && Player2Atk == Attack.Scissor)
        {
            return P2;
        }
        else if (Player1Atk == Attack.Paper && Player2Atk == Attack.Scissor)
        {
            return P1;
        }
        else if (Player1Atk == Attack.Paper && Player2Atk == Attack.Rock)
        {
            return P2;
        }
        else if (Player1Atk == Attack.Scissor && Player2Atk == Attack.Rock)
        {
            return P1;
        }
        else if (Player1Atk == Attack.Scissor && Player2Atk == Attack.Paper)
        {
            return P2;
        }

        return null;
    }

    //logic untuk menentukan siapa yang menang
    private CardPlayer GetWinner()
    {
        if (P1.Health == 0)
        {
            return P2;
        }
        else if (P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }

    public void LoadScene(int scaneIndex)
    {
        SceneManager.LoadScene(scaneIndex);
    }
}
