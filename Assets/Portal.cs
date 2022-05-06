using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System;


namespace STP.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier id;
        //[SerializeField] Portal
        //[SerializeField] float fadeOutTime = 2f;
        //[SerializeField] float fadeInTime = 1f;
        //[SerializeField] float fadeWaitTime = 2f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //if (sceneToLoad == SceneManager.GetActiveScene().buildIndex) return;

            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            bool loadingSameLevel = sceneToLoad == SceneManager.GetActiveScene().buildIndex;
            //Fader fader = FindObjectOfType<Fader>();
            if (!loadingSameLevel)
                DontDestroyOnLoad(gameObject);
            DisablePlayerControl();
            //yield return fader.FadeOut(fadeOutTime);

            // Save current level
            //SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            //wrapper.Save();

            // load next level
            if (!loadingSameLevel)
            {
                yield return SceneManager.LoadSceneAsync(sceneToLoad);
                DisablePlayerControl();
                // load next level state
                //wrapper.Load();
                print("Scene Loaded");
            }

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            //wrapper.Save();
            yield return new WaitForSeconds(0.5f);
            //fader.FadeIn(fadeInTime);
            RestorePlayerControl();
            if (!loadingSameLevel)
                Destroy(gameObject);
        }


        private void RestorePlayerControl()
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.enabled = true;
        }

        private void DisablePlayerControl()
        {
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.enabled = false;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            if (otherPortal == null) { return; }
            PlayerController player = FindObjectOfType<PlayerController>();
            //player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            // player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position - player.SpriteOffset;
            //player.transform.rotation = otherPortal.spawnPoint.rotation;
            // player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            Portal[] portalsInScene = FindObjectsOfType<Portal>();
            foreach (Portal portal in portalsInScene)
            {
                if (portal == this) { continue; }
                if (portal.id != id) { continue; }
                return portal;
            }
            return null;
        }
    }
}
