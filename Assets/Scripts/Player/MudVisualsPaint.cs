using UnityEngine;
namespace Player
{
        public class MudVisualsPaint : MonoBehaviour
    {
        // None = 0%, Low = 25%, Mid = 50%, High = 75%, Full = 100% (out of 100)
        private enum CurrentMudLevel { None, Low, Mid, High, Full }
        [SerializeField]private CurrentMudLevel currentMudLevel;
        private PlayerMudPaintScript paintScript;
        private SpriteRenderer spriteRenderer;
        [SerializeField, Tooltip("Add 4 sprites for each level of muddiness here. 0 = low, 3 = full")] 
        private Sprite[] muddyCharacterSprites = new Sprite[4];
        private Sprite defaultSprite;
        [SerializeField, Range(0,1)] float currentMudTotal;

        private void Start()
        {
            paintScript = GetComponent<PlayerMudPaintScript>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            defaultSprite = spriteRenderer.sprite;
            currentMudLevel = CurrentMudLevel.None;
        }

        private void FixedUpdate()
        {
            //currentMudTotal = Mathf.InverseLerp(100, 0, paintScript.GetTotalMud());
            Debug.Log(currentMudTotal);
            MudLevelStateMachine();
        }
        // Change Sprite depending on how much mud the player has currently
        private void MudLevelStateMachine()
        { 
            switch (currentMudLevel)
            {
                case CurrentMudLevel.None:
                    spriteRenderer.sprite = defaultSprite;
                    if (currentMudTotal > .10f) currentMudLevel = CurrentMudLevel.Low;
                    break;
                case CurrentMudLevel.Low: // 10% mud collected
                    spriteRenderer.sprite = muddyCharacterSprites[0];
                    currentMudLevel = currentMudTotal switch
                    {
                        > .30f => CurrentMudLevel.Mid,
                        < .10f => CurrentMudLevel.None,
                        _ => currentMudLevel
                    };
                    break;
                case CurrentMudLevel.Mid: // 30% mud collected
                    spriteRenderer.sprite = muddyCharacterSprites[1];
                    currentMudLevel = currentMudTotal switch
                    {
                        > .35f => CurrentMudLevel.High,
                        < .10f => CurrentMudLevel.Low,
                        _ => currentMudLevel
                    };
                    break;
                case CurrentMudLevel.High: // 50% mud collected
                    spriteRenderer.sprite = muddyCharacterSprites[2];
                    currentMudLevel = currentMudTotal switch
                    {
                        >= .80f => CurrentMudLevel.Full,
                        < .50f => CurrentMudLevel.Mid,
                        _ => currentMudLevel
                    };
                    break;
                case CurrentMudLevel.Full: // 80% mud collected
                    spriteRenderer.sprite = muddyCharacterSprites[3];
                    if (currentMudTotal < .80) currentMudLevel = CurrentMudLevel.High;
                    break;
                default:
                    break;
            }
        }
    }
}

