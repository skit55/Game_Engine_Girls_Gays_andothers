using System;
using UnityEngine;

public enum FightState { Start, EnemyTurn,TurnAdvance, PlayerTurn, Win, Lose }

public class FightFlowState : MonoBehaviour
{
    [SerializeField] EnemyTurnController enemyTurn;
    [SerializeField] PlayerTurnController playerTurn;
    [SerializeField] PlayerParryController playerParry;

    [SerializeField] FightUI fightUI;

    [SerializeField] EnemyRuntime enemyRuntime;

    [Header("Optional")]
    [SerializeField] EnemyData currentEnemy;

    FightState state;

    [SerializeField] public GameObject advance;
    [SerializeField] public GameObject leftMouse;

    [SerializeField] public GameObject wasd;



    public event Action<bool> FightFinished; // bool = playerWon


    void OnEnable()
    {
        // Additive safe: Instance kann kurz null sein, je nach Load-Reihenfolge.
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.Died += OnPlayerDeath;

        if (enemyTurn != null)
            enemyTurn.EnemyTurnFinished += OnEnemyTurnFinished;

        if (playerTurn != null)
            playerTurn.PlayerTurnFinished += OnPlayerTurnFinished; // <- brauchst du im PlayerTurnController
                                                                   // ggf. Action<TimingResult> siehe weiter unten

        if (InputHub.Instance != null)
            InputHub.Instance.AdvancePressed += OnAdvancePressed;  // <- brauchst du ggf. neu

        if (enemyRuntime != null)
            enemyRuntime.Died += OnEnemyDeath;
    }

    void OnDisable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.Died -= OnPlayerDeath;

        if (enemyTurn != null)
            enemyTurn.EnemyTurnFinished -= OnEnemyTurnFinished;

        if (playerTurn != null)
            playerTurn.PlayerTurnFinished -= OnPlayerTurnFinished;

        if (InputHub.Instance != null)
            InputHub.Instance.AdvancePressed -= OnAdvancePressed;

        if (enemyRuntime != null)
            enemyRuntime.Died -= OnEnemyDeath;
    }

    public void BeginFight(EnemyData data)
    {
        currentEnemy = data;
        enemyRuntime.InitFrom(data);
        SetState(FightState.Start);

        // UI: "Press Advance"
        // FightUI.Instance?.ShowPressAdvance(true);
    }

    void SetState(FightState next)
    {
        state = next;
        Debug.Log( next.ToString());
        switch (state)
        {
            case FightState.Start:
                playerParry.enabled = false;
                wasd.SetActive(true);
                leftMouse.SetActive(false);
                advance.SetActive(true);
                break;

            case FightState.EnemyTurn:
                // FightUI.Instance?.ShowPressAdvance(false);
                advance.SetActive(false);
                playerParry.enabled = true;
                SfxManager.Instance.PlaySFX2D(SfxBank.Instance.turnStart);
                wasd.SetActive(false);

                enemyTurn.BeginEnemyTurn(currentEnemy);
                break;

            case FightState.TurnAdvance:
                playerParry.enabled = false;
                advance.SetActive(true);
                break;

            case FightState.PlayerTurn:

                fightUI.Show();
                playerTurn.BeginPlayerTurn();
                SfxManager.Instance.PlaySFX2D(SfxBank.Instance.turnStart);
                leftMouse.SetActive(true);

                //Debug.Log("BEGIN PLAYER TURN");
                break;

            case FightState.Win:
                // Cleanup / UI / disable input
                // FightUI.Instance?.ShowWin();
                Debug.Log("WIN STATE!");
                fightUI.ShowResultPanel(true);
                SfxManager.Instance.PlaySFX2D(SfxBank.Instance.win);
                PlayerStats.Instance.Heal();
                leftMouse.SetActive(false);
                wasd.SetActive(false);
                advance.SetActive(false);
                break;

            case FightState.Lose:
                // FightUI.Instance?.ShowLose();
                fightUI.ShowResultPanel(false);
                SfxManager.Instance.PlaySFX2D(SfxBank.Instance.lose);
                PlayerStats.Instance.Heal();
                leftMouse.SetActive(false);
                wasd.SetActive(false);
                advance.SetActive(false);
                break;
        }
    }

    void OnAdvancePressed()
    {
        if (state == FightState.Start) { 
            advance.SetActive(false);

            SetState(FightState.EnemyTurn); 
            return; }
        if (state == FightState.TurnAdvance) {             advance.SetActive(false);
            SetState(FightState.PlayerTurn); return; }

        // NEU: Win/Lose verlassen
        if (state == FightState.Win)
        {
                        advance.SetActive(false);

            FightFinished?.Invoke(true);
            return;
        }
        if (state == FightState.Lose)
        {
                        advance.SetActive(false);

            FightFinished?.Invoke(false);
            return;
        }
    }

    void OnEnemyTurnFinished()
    {
        if (state != FightState.EnemyTurn) return;

        // optional: Wenn du warten willst bis alle Projectiles weg sind, dann hier nicht sofort.
        SetState(FightState.TurnAdvance);
    }

    void OnPlayerTurnFinished(TimingResult result)
    {
        if (state != FightState.PlayerTurn) return;

        // hier würdest du EnemyHP checken -> Win, sonst weiter
        // if (enemyStats.HP <= 0) { SetState(FightState.Win); return; }
        if(result == TimingResult.Hit)
        {
            enemyRuntime.DealDamage(1);
        }
        fightUI.Hide(result);
        
        if(enemyRuntime.dead == false)
        {
            SetState(FightState.Start);
        }
        
    }

    void OnPlayerDeath()
    {
        if (state == FightState.Win || state == FightState.Lose) return;
        enemyTurn.StopSpawns();
        SetState(FightState.Lose);
    }

    // Call this when enemy dies (z.B. EnemyStats.Died event oder nachdem du HP abziehst)
    public void OnEnemyDeath()
    {
        Debug.Log("WIN!!!!!");
        if (state == FightState.Win || state == FightState.Lose) return;
        SetState(FightState.Win);
    }
}
