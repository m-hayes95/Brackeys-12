using UnityEngine;

namespace Player
{
        public class MudVisuals : MonoBehaviour
    {
        private enum CurrentMudLevel { None, Low, Mid, High, Full }
        [SerializeField] private CurrentMudLevel currentMudLevel;
        [SerializeField, Tooltip("Add 4 sprites for each level of muddiness here. 0 = low, 3 = full")] 
        private Sprite[] muddyCharacterSprites = new Sprite[4];

        [SerializeField, Range(0f,100f), Tooltip("Set a threshold for when sprites change according to how dirty the player currently is")]
        private float lowDirtThreshold, midDirtThreshold, highDirtThreshold, fullDirtThreshold; 
        
        private DirtyMeter dirtyMeter;
        private SpriteRenderer spriteRenderer;
        private Sprite defaultSprite;
        private float currentMudTotal;

        private void Start()
        {
            dirtyMeter = GetComponent<DirtyMeter>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            defaultSprite = spriteRenderer.sprite;
            currentMudLevel = CurrentMudLevel.None;
        }

        private void Update()
        {
            currentMudTotal = dirtyMeter.GetPlayerMudTotal();
            MudLevelStateMachine();
        }
        // Change Sprite depending on how much mud the player has currently
        private void MudLevelStateMachine()
        { 
            switch (currentMudLevel)
            {
                case CurrentMudLevel.None:
                    spriteRenderer.sprite = defaultSprite;
                    if (currentMudTotal > lowDirtThreshold) currentMudLevel = CurrentMudLevel.Low;
                    break;
                case CurrentMudLevel.Low: 
                    spriteRenderer.sprite = muddyCharacterSprites[0];
                    if (currentMudTotal < lowDirtThreshold) currentMudLevel = CurrentMudLevel.None;
                    else if (currentMudTotal > midDirtThreshold) currentMudLevel = CurrentMudLevel.Mid;
                    break;
                case CurrentMudLevel.Mid:
                    spriteRenderer.sprite = muddyCharacterSprites[1];
                    if (currentMudTotal < midDirtThreshold) currentMudLevel = CurrentMudLevel.Low;
                    else if (currentMudTotal > highDirtThreshold) currentMudLevel = CurrentMudLevel.High;
                    break;
                case CurrentMudLevel.High:
                    spriteRenderer.sprite = muddyCharacterSprites[2];
                    if (currentMudTotal < highDirtThreshold) currentMudLevel = CurrentMudLevel.Mid;
                    else if (currentMudTotal > fullDirtThreshold) currentMudLevel = CurrentMudLevel.Full;
                    break;
                case CurrentMudLevel.Full:
                    spriteRenderer.sprite = muddyCharacterSprites[3];
                    if (currentMudTotal < fullDirtThreshold) currentMudLevel = CurrentMudLevel.High;
                    break;
                default:
                    break;
            }
        }
    }
}

