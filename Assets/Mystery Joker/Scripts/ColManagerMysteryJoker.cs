using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JokerMystery
{
    public class ColManagerMysteryJoker : MonoBehaviour
    {
        public SOItemMysteryJoker items;
        public float speed, duration, startingTime, newSpeed, newDuration;
        public float normalizedTime;
        public GameObject[] pos = new GameObject[9];
        public List<ItemsMysteryJoker> slotItem;
        public List<int> itemNumbers;
        public LeanTweenType tweenType;
        public int i = 9;
        //private int j = 2;
        public bool spin, isColSpinningStopped, is1stCol;

        public static ColManagerMysteryJoker instance;

        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            newSpeed = speed;
            newDuration = duration;
            AssignRandomItems();
        }

        void AssignRandomItems()
        {
            for (int i = 0; i < 9; i++)
            {
                var item = pos[i].transform.GetChild(0).GetComponent<ItemsMysteryJoker>();

                if (item.img != null)
                {
                    int num = Random.Range(0, items.image.Length - 1);
                    item.SetSprite(items.image[num]);
                }
            }
        }

        void Spin()
        {
            foreach (var item in slotItem)
            {
                if (item != null)
                {
                    int CI = item.currentIndex;
                    // set sprite in the visible pos holder;
                    if (spin == false && (item.currentIndex == 3 || item.currentIndex == 4 || item.currentIndex == 5))
                    {
                        item.isSpriteChanged = true;
                        int elementNum = itemNumbers[ item.currentIndex - 3];  //result = 0, 1 & 2
                        item.elementNum = item.currentIndex - 2;
                        //make animator null if any before setting animator 
                        item.SetSprite(items.image[elementNum]);
                        item.elementNum = elementNum;
                    }
                    item.MoveY(pos[CI].transform, speed, tweenType);
                }
            }
        }


        public void SpinWheels() => StartCoroutine(nameof(_Spin), startingTime);
        private IEnumerator _Spin()
        {
            i = 11;
            //j = 2;
            normalizedTime = 0;
            ResetItemData();
            isColSpinningStopped = false;
            while (spin){
                Spin();

                yield return new WaitForSeconds(speed);
                if (isColSpinningStopped)
                {
                    
                    normalizedTime += Time.fixedDeltaTime / duration;
                    if (normalizedTime >= 1)
                    {
                        print("isStopped");
                        spin = false;
                        Spin();

                    }
                }
            }

            yield return new WaitForSeconds(0.8f);
            if (!spin)
            {
                spin = false;
                //call All slot stopped from manager
                ManagerJokerMystery.instance.AllSlotStopped();
            }


        }

        void ResetItemData()
        {
            itemNumbers.Clear();
            foreach (var item in slotItem)
            {
                item.isSpriteChanged = false;
                item.elementNum = 0;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
