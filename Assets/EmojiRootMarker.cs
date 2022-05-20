using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class EmojiRootMarker : MonoBehaviour
    {
        bool _hasEmoji;
        public void AddEmoji(GameObject prefab, float destorySeconds)
        {
            if (_hasEmoji) return;

            _hasEmoji = true;
            var emoji = Instantiate(prefab);
            emoji.transform.SetParent(transform);
            emoji.transform.localPosition = Vector3.zero;
            emoji.transform.localScale = Vector3.one;
            StartCoroutine(ResetMe(emoji, destorySeconds));
        }

        IEnumerator ResetMe(GameObject emojiObj, float destroySeconds)
        {
            yield return new WaitForSeconds(destroySeconds);

            Destroy(emojiObj);
            _hasEmoji = false;

            // 6.06, -6.02
        }
    }
}