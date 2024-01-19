using System;
using System.Collections;
using TMPro;
using UnityEngine;


namespace DefaultNamespace
{
    public class DamageTag : MonoBehaviour
    {
        private const short timeToUpdate = 1;
        private short _time = 0;

        private RectTransform rectTransform;

        private bool state = false;

        private TMP_Text textHp;
        private Transform uiObject;
        private Transform player;

        private Vector3 startPos;

        private float _timeShow = 0;

        private void Awake()
        {
            player = this.transform;
            GameObject prefab = Resources.Load<GameObject>("tag");
            Transform canvas = GameObject.Find("Canvas").transform;
            GameObject tag = Instantiate(prefab, canvas);

            rectTransform = tag.GetComponent<RectTransform>();
            uiObject = tag.transform;

            textHp = tag.GetComponent<TMP_Text>();
            startPos = rectTransform.position;
            uiObject.gameObject.SetActive(false);
        }

        public void Kill()
        {
            textHp.text = "";
            Destroy(uiObject.gameObject);
            //StartCoroutine(uiObject.GetComponent<Destroy>().Kill());
        }

        public void ShowDamage(int damage)
        {
            rectTransform.position = startPos;
            textHp.text = "-" + damage;
            uiObject.gameObject.SetActive(true);
            _timeShow = 1;
        }


        private void Update()
        {
            _time--;
            if (_time > 0) return;
            _time = timeToUpdate;

            if (uiObject && uiObject.gameObject.activeSelf)
            {
                _timeShow -= Time.deltaTime;

                if (_timeShow <= 0)
                {
                    _timeShow = 0;
                    uiObject.gameObject.SetActive(false);
                }

                rectTransform.position = new Vector3(rectTransform.position.x,
                    rectTransform.position.y + 5f * Time.deltaTime,
                    rectTransform.position.z);
            }
            else
            {
            }

            Vector3 pos = player.position + Vector3.up * 3;

            if (Helper.PointInCameraView(pos))
            {
                if (_timeShow > 0)
                    uiObject.gameObject.SetActive(true);

                rectTransform.position = Camera.main.WorldToScreenPoint(pos);
                if (!state)
                    state = true;
            }
            else
            {
                uiObject.gameObject.SetActive(false);
                if (state)
                    state = false;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            //textTag.text = "11";
        }
    }
}