using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;
using System;

namespace JokerMystery {
    public class ManagerJokerMystery : MonoBehaviour
    {
        public static ManagerJokerMystery instance;

        [Header("Col manager")]
        public ColManagerMysteryJoker[] colManager = new ColManagerMysteryJoker[3];

        [Space]
        [Header("bools")]
        public bool spin, stop, autoSpin, infinite, isBlink, isCheckCalled;

        [Space]
        [Header("Win")]
        [SerializeField] TextMeshProUGUI winValue;
        [SerializeField] TextMeshProUGUI AdsText;
        [SerializeField] float winAmount;
        public float prize;

        [Space]
        [Header("user balance")]
        public TextMeshProUGUI userBalance;

        [Space]
        [Header("Buttons")]
        public GameObject spinBtn, stopBtn;
        private Button spinInteractable;
        private Button stopInteractable;
        public List<Button> BtnHolders;

        JSONNode node;
        private JSONArray jsonArray;

        public int counter = 0;

        public List<MyList> myList;

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            spinInteractable = spinBtn.GetComponent<Button>();
            stopInteractable = stopBtn.GetComponent<Button>();
            StartCoroutine(EnableSpinInteractable());
        }
        void EnableBtnHolders()
        {
            foreach (var btns in BtnHolders)
            {
                btns.interactable = true;
            }
        }
        void DisableBtnHolders()
        {
            foreach (var btns in BtnHolders)
            {
                btns.interactable = false;
            }
        }
        IEnumerator EnableSpinInteractable()
        {
            // enable spin btn interactable only if game is active 
            yield return new WaitForSeconds(1f);
            spinInteractable.interactable = true;
        }

        public void UpdateUserBalance(float bal)
        {
            userBalance.text = bal.ToString("f2");
        }

        public void AutoSpinBtn()
        {
            autoSpin = true;
            StartSpin();
        }
        public void SpinBtn()
        {
            if (!spinInteractable.interactable) return;
            StartSpin();

            // if auto 
        }

        void StartSpin()
        {
            //print("spin is started");
            CancelInvoke();
            ClearCache();
            DisableBtnHolders();
            //check if user has sufficient credit
            if (!Bet.Instance.HaveSufficientCredit())
            {
                autoSpin = false;
                return;
            }

            spin = true;
            stop = false;

            spinInteractable.interactable = false;
            stopInteractable.interactable = false;
            foreach (var item in colManager)
            {
                item.tweenType = LeanTweenType.linear;
                item.spin = true;
                item.SpinWheels();
            }
            Bet.Instance.Updatebalance();
            GetOnlineData();
        }

        public void StopBtnCLicked()
        {
            autoSpin = false;
            if (spin)
            {
                foreach (ColManagerMysteryJoker item in colManager)
                {
                    item.duration = 0;
                    item.isColSpinningStopped = true;
                    item.spin = false;
                }
            }
            stopBtn.SetActive(false);
        }

        void GetOnlineData()
        {
            foreach (var item in colManager)
            {
                item.itemNumbers.Clear();
            }
            ApiManager.instance.GetGameData();
            StartCoroutine(IEGetOnlineData());
        }

        private IEnumerator IEGetOnlineData()
        {
            yield return new WaitUntil(() => ApiManager.instance.node != null);
            node = ApiManager.instance.node;
            jsonArray = (JSONArray)node["itemIndexes"];
            StartCoroutine(IEAddDataToColumn());
        }

        IEnumerator IEAddDataToColumn()
        {
            yield return new WaitForSeconds(1);
            AddDataToColumn();
        }

        void AddDataToColumn()
        {
            foreach (var item in jsonArray)
                for (int i = 0; i < item.Value.Count; i++)
                    colManager[i].itemNumbers.Add(item.Value[i]);
            foreach (var item in jsonArray)
                for (int i = 0; i < item.Value.Count; i++)
                    colManager[i].itemNumbers.Add(item.Value[i]);
            foreach (var item in jsonArray)
                for (int i = 0; i < item.Value.Count; i++)
                    colManager[i].itemNumbers.Add(item.Value[i]);
            foreach (var item in jsonArray)
                for (int i = 0; i < item.Value.Count; i++)
                    colManager[i].itemNumbers.Add(item.Value[i]);


            colManager[0].isColSpinningStopped = true;
            StopBtnActivate();
        }

        public void AllSlotStopped()
        {
            //print("hereis all slot stop");
            if (colManager[2].spin == false)
            {
                /*if (!autoSpin)
                {
                    //plus minus btn interactable off
                }*/


            }

            if (counter < 2)
            {
                counter++;
                colManager[counter].isColSpinningStopped = true;

            }

            if (counter == 2)
            {
                colManager[counter].isColSpinningStopped = true;

               /* if (!autoSpin)
                {
                    // check if spin and plus minus btns need to be activated
                }*/

                if (!stop)
                    Invoke(nameof(SendDataAtStop), 0.1f);
                spin = false;
            }
        }

        void SendDataAtStop()
        {
            if (!isCheckCalled)
            {
                isCheckCalled = true;
                spin = false;
                StartCoroutine(CheckData());
            }
        }

        //check data from the json files to see if there is any win 
        IEnumerator CheckData()
        {
            yield return new WaitForSeconds(.5f);
            float priceUsed = node["price_used"];
            prize = node["prize"];
            //print("prize");

            foreach (var item in node["combination"])
            {
                //print("here is inside combination node");
                string colName = item.Key.ToString();
                int[] colNum = Array.ConvertAll(item.Key.Split(','), Convert.ToInt32);
                Check5Combo(colNum, colName);
            }
            spin = false;
            StartCoroutine(CheckWin());

        }

        IEnumerator CheckWin()
        {
            //print("here is check win");
            yield return new WaitForSeconds(.5f);
            resetTiming();
            if (myList.Count != 0)
            {
                //winValue.text = prize.ToString("f2");
                winAmount = prize * Bet.Instance.betAmount;
                winValue.text = winAmount.ToString("f2");
                AdsText.text = "Congratulations you won $" + winAmount.ToString("f2");
                Bet.Instance.UpdateWonCredit(winAmount);
            }
            else if (myList.Count == 0)
            {
                //print("there is no list");
                AdsText.text = "Better Luck Next Time!";
                StartCoroutine(EnableSpinbtn());
            }

            StartCoroutine(EnableSpinbtn());
            if (!autoSpin)
            {
                EnableBtnHolders();
            }

        }

        // lines of the game
        void Check5Combo(int[] colNum, string colName)
        {
            //print("here is Check5Combo");
            switch (colName)
            {
                case "1,1,1": CheckCombo(colNum, 0); break;
                case "0,0,0": CheckCombo(colNum, 1); break;
                case "2,2,2": CheckCombo(colNum, 2); break;
                case "0,1,2": CheckCombo(colNum, 3); break;
                case "2,1,0": CheckCombo(colNum, 4); break;
            }
        }

        //compare sprite names to check if all the sprites in combinations are same 
        void CheckCombo(int[] colNum, int lineNum)
        {
            //print("here is checkCombo");
            List<GameObject> li = new List<GameObject>();

            GameObject sprite0 = colManager[0].pos[colNum[0] + 3];
            GameObject sprite1 = colManager[1].pos[colNum[1] + 3];
            GameObject sprite2 = colManager[2].pos[colNum[2] + 3];

            string name0 = sprite0.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name;
            string name1 = sprite1.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name;
            string name2 = sprite2.transform.GetChild(0).GetComponentInChildren<Image>().sprite.name;

            /*print("name 1 is"+ name0);
            print("name 2 is" + name1);
            print("name 3 is" + name2);*/
            if (name0 == name1 && name1 == name2 && name2 == name0)
            {
                //print("here is combination");

                li.Add(sprite0);
                li.Add(sprite1);
                li.Add(sprite2);
                myList.Add(new MyList(li));
            }
        }
        void StopBtnActivate()
        {
            //print("here is activate");
            stopInteractable.interactable = true;
            stopBtn.SetActive(true);
        }

        
        IEnumerator EnableSpinbtn()
        {
            yield return new WaitForSeconds(.8f);
            if (!autoSpin)
            {
                spinInteractable.interactable = true;
                stopBtn.SetActive(false);
                //print("spin enable");
            }
            else if (autoSpin)
            {
                yield return new WaitForSeconds(.4f);
                StopAllCoroutines();
                Invoke(nameof(StartSpin), 0.5f);
            }
            
           
        }
        public void AutoSpin()
        {

        }

        void ClearCache()
        {
            //ApiManager.instance.isDataReceived = false;
            counter = 0;
            isCheckCalled = false;
            winAmount = 0;
            winValue.text = null ;
            AdsText.text = null;
            myList.Clear();
            
        }


        public void resetTiming()
        {
            for (int i = 0; i < colManager.Length; i++)
            {
                colManager[i].speed = colManager[i].newSpeed;
                colManager[i].duration = colManager[i].newDuration;


            }
        }
        // Update is called once per frame
        void Update()
        {

        }

    }

        [Serializable]
        public class MyList
        {
            public List<GameObject> myItem;
            public MyList(List<GameObject> i )
            {
                myItem = i;
            }
        }
    
}
