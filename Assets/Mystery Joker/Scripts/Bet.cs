using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JokerMystery{ 
public class Bet : MonoBehaviour
{
    public float creditValue, betAmount;
    [SerializeField] TextMeshProUGUI BetValue;

    private static Bet _instance;

    public List<GameObject> selectedBet;
    public static Bet Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //create a function to update credit value at first so the player balance in game match with backend balance


    private void Start()
    {
        UpdateBetAmount();
        ManagerJokerMystery.instance.UpdateUserBalance(creditValue);
    }

        /// <summary>
        /// Perform bet up function
        /// </summary>
    public void PlusBtnClicked()
    {
        if (ManagerJokerMystery.instance.spin || ManagerJokerMystery.instance.autoSpin) return;
        betAmount = betAmount switch
        {
            1.00f => 1.50f,
            1.50f => 2.00f,
            2.00f => 3.00f,
            3.00f => 5.00f,
           5.00f => 1.00f
            //  _ => 0.1f
        };
        UpdateBetAmount();
    }
        /// <summary>
        /// perform bet down function
        /// </summary>
    public void MinusBtnClicked()
    {
        if (ManagerJokerMystery.instance.spin || ManagerJokerMystery.instance.autoSpin) return;

        betAmount = betAmount switch
        {
            5.00f => 3.00f,
            3.00f => 2.00f,
            2.00f => 1.50f,
            1.50f => 1.00f,
            1.00f => 5.00f
        };
        UpdateBetAmount();
    }

  /// <summary>
  /// updates bet in accordance to plus and minus button
  /// </summary>
    void UpdateBetAmount()
    {
        //print("bet amount is" + betAmount);
        BetValue.text = betAmount.ToString("f2");
        DeactivateSelectedBets();
        if (betAmount == 1.00f)
        {
            selectedBet[0].SetActive(true);
        }
        else if (betAmount == 1.50f)
        {
            selectedBet[1].SetActive(true);

        }
        else if (betAmount == 2.00f)
        {
            selectedBet[2].SetActive(true);


        }
        else if (betAmount == 3.00f)
        {
            selectedBet[3].SetActive(true);


        }
        else if (betAmount == 5.00f)
        {
            selectedBet[4].SetActive(true);

        }
    }

    void DeactivateSelectedBets()
    {
        foreach (var item in selectedBet)
        {
            item.SetActive(false);
        }
    }

        #region update user balance
        public void Updatebalance()
    {
        creditValue -= betAmount;
        ManagerJokerMystery.instance.UpdateUserBalance(creditValue);
    }

    public void UpdateWonCredit(float amount)
    {
        creditValue += amount;
        ManagerJokerMystery.instance.UpdateUserBalance(creditValue);

    }
        #endregion

        /// <summary>
        /// checks if the player have enough balance to spin
        /// </summary>
        /// <returns></returns>
        public bool HaveSufficientCredit()
    {
        if (betAmount <= creditValue)
            return true;
        else
            return false;
    }
}
}