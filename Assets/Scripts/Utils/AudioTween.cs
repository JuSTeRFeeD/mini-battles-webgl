using DG.Tweening;
using UnityEngine.Audio;

namespace Utils
{
    public static class AudioTween
    {
        public static Tweener DoSetFloat(this AudioMixer target, string floatName, float endValue, float time)
        {
            return DOTween.To(() =>
            {
                target.GetFloat(floatName, out var curVal);
                return curVal;
            }, x => target.SetFloat(floatName, x), endValue, time);
        }
    }
}