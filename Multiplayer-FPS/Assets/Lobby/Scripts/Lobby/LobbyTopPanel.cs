using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class LobbyTopPanel : MonoBehaviour
    {
        public bool isInGame = false;

        protected bool isDisplayed = true;
        protected Image panelImage;

        void Start()
        {
            panelImage = GetComponent<Image>();
        }


        void Update()
        {
            if (!isInGame)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleVisibility(!isDisplayed);

                if (isDisplayed)
                    foreach (Health h in FindObjectsOfType<Health>())
                    {
                        try
                        {
                            if (h.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                            {
                                StartCoroutine(DisableLook(h));
                                break;
                            }
#pragma warning disable 0168
                        }
                        catch (System.NullReferenceException e)
#pragma warning restore 0168
                        {
                            continue;
                        }
                    }
                else
                    foreach (Health h in FindObjectsOfType<Health>())
                    {
                        try
                        {
                            if (h.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                            {
                                StartCoroutine(EnableLook(h));
                                break;
                            }
#pragma warning disable 0168
                        }
                        catch (System.NullReferenceException e)
#pragma warning restore 0168
                        {
                            continue;
                        }

                    }
            }
        }

        IEnumerator DisableLook(Health h)
        {
            yield return null;
            h.mainRotator.paused = true;
            h.pivotRotator.paused = true;
        }

        IEnumerator EnableLook(Health h)
        {
            yield return new WaitUntil(() => !h.mainRotator.settingPosition && !h.pivotRotator.settingPosition);
            h.mainRotator.paused = false;
            h.pivotRotator.paused = false;
            
        }

        public void ToggleVisibility(bool visible)
        {
            isDisplayed = visible;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(isDisplayed);
            }

            if (panelImage != null)
            {
                panelImage.enabled = isDisplayed;
            }
        }
    }
}