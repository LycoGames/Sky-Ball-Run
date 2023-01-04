using _Game.Scripts.Base.UserInterface;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Game.UserInterfaces.InGame
{
    public class WealthCanvas : BaseCanvas, IStartable, IQuitable
    {
        private const string bonusText = "Bonus";
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private RectTransform diamondSection;
        [SerializeField] private TMP_Text diamondText;
        [SerializeField] private GameObject levelPanel;
        
        private readonly Vector3 sectionChangeScale = new(.5f, .5f, .5f);

        // eğer coin ile diamond aynı anda toplanma ihtimali var ise tweener her para birimine özel olmalıdır
        // şuanlık aynı anda iki birim toplanamadığı için ortak tweener kullanılıyor.
        private Tweener punchTweener;


        public void OnStart()
        {
        }

        public void OnQuit()
        {
        }

        #region Changes

        public void SetupLevel(string value)
        {
            levelPanel.SetActive(true);
            levelText.text = value;
            if(bonusText==value)levelPanel.SetActive(false);
        }

        public void ChangeDiamond(string value)
        {
            diamondText.text = value;
            if (punchTweener is { active: true })
            {
                ResetEffect(diamondSection);
            }

            PunchEffect(diamondSection);
        }

        #endregion

        private void PunchEffect(Component section)
        {
            punchTweener = section.transform.DOPunchScale(sectionChangeScale, .1f, 1, 0F);
        }

        private void ResetEffect(Component section)
        {
            punchTweener.Kill();
            ResetSectionScale(section);
        }

        private void ResetSectionScale(Component section)
        {
            section.transform.localScale = Vector3.one;
        }
    }
}