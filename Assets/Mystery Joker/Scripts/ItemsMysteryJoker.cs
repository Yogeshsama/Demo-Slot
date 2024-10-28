using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JokerMystery
{
    public class ItemsMysteryJoker : MonoBehaviour
    {

        public int currentIndex;
        public Image img;
        public bool isSpriteChanged;
        public int elementNum;
        // Start is called before the first frame update
        void Start()
        {
            img = GetComponent<Image>();
            currentIndex = int.Parse(transform.name);


        }

        //tween to move items
        public void MoveY(Transform transform, float speed, LeanTweenType tweenType)
        {
            //print("here is move");
            if (currentIndex == 11) currentIndex = 0;
            else currentIndex++;
            if (currentIndex == 1) gameObject.transform.position = transform.position;
            else LeanTween.move(gameObject, transform.position, speed).setEase(tweenType);
            this.transform.SetParent(transform);
        }

        //set sprite function
        public void SetSprite(Sprite sprite) => img.sprite = sprite;


        //set animator and audios if necessary
    }
}
