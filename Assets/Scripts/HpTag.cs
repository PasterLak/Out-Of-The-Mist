
using StarterAssets;
using TMPro;
using UnityEngine;


namespace DefaultNamespace
{
    public class HpTag : MonoBehaviour
    {
        public static int DistanceToStartShow = 30;
        
        private const short timeToUpdate = 1;
        private short _time = 0;

        private RectTransform rectTransform;

        private bool state = false;

        private TMP_Text textHp;
        private Transform uiObject;
        private Transform player;

        private Vector3 startPos;

        private float _timeShow = 1;

        public bool Founded = false;

        private void Awake()
        {
            player = this.transform;
            GameObject prefab = Resources.Load<GameObject>("tagHp");
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
            textHp.text = "-"+damage;
            uiObject.gameObject.SetActive(true);
            _timeShow = 1;
          
        }

        private void Update()
        {
            _time--;
            if (_time > 0) return;
            _time = timeToUpdate;
            
            if (!Founded)
            {
                float dis = Distance.Space3DSquared(Player.Instance.GetCenter(), transform.position);

                if (dis < DistanceToStartShow * DistanceToStartShow)
                {
                    Founded = true;
                }
                
                return;
            }
            

            if (uiObject && uiObject.gameObject.activeSelf)
            {
                _timeShow -= Time.deltaTime;

                if (_timeShow <= 0)
                {
                    _timeShow = 0;
                    uiObject.gameObject.SetActive(false);
                    
                }
                    
                rectTransform.position = new Vector3(rectTransform.position.x, 
                    rectTransform.position.y + 1f * Time.deltaTime,
                    rectTransform.position.z);
            }
            else
            {
                
            }

            Vector3 pos = player.position + Vector3.up * 2;

            if (Helper.PointInCameraView(pos))
            {
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
            
        }

        public void UpdateUI(int hp, int maxHp)
        {
            textHp.text = hp + "/" + maxHp;
        }
    }
}