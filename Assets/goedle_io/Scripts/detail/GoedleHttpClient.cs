using System.Collections;
using UnityEngine;
using System;
using SimpleJSON;
using UnityEngine.Networking;
namespace goedle_sdk.detail
{
    public class GoedleHttpClient: MonoBehaviour 
	{
        public static GoedleHttpClient instance = null;

        public void sendGet( string url, IGoedleWebRequest gwr, bool staging)
        {
            StartCoroutine(getRequest( url, gwr, staging));
        }
        
        
        public void sendPost(string url, string authentification, IGoedleWebRequest gwr, IGoedleUploadHandler guh, bool staging)
        {
            StartCoroutine(postJSONRequest(url, authentification, gwr, guh, staging));
        }

        public IEnumerator getRequest(string url, IGoedleWebRequest gwr, bool staging)
        {
            if (staging)
            {
                Debug.Log("Staging is on your get request would look like this:\n" + url);
            }
            else
            {
                gwr.unityWebRequest = new UnityWebRequest();
                using (gwr.unityWebRequest)
                {
                    gwr.method = "GET";
                    gwr.url = url;
                    yield return gwr.SendWebRequest();
                    if (gwr.isNetworkError || gwr.isHttpError)
                    {
                        Debug.Log("{\"error\": {  \"isHttpError\": \"" + gwr.isHttpError + "\",  \"isNetworkError\": \"" + gwr.isNetworkError + "\" } }");
                    }
                    yield break;
                }
            }
        }

      
        public IEnumerator postJSONRequest( string url, string authentification, IGoedleWebRequest gwr, IGoedleUploadHandler guh, bool staging)
	    {
            if (staging)
            {
                Debug.Log("Staging is on you would request from this url:\n" + url + "\n The data would look like this:\n"+ guh.getDataString());
            }
            else
            {
                gwr.unityWebRequest = new UnityWebRequest();
                using (gwr.unityWebRequest)
                {
                    gwr.method = "POST";
                    gwr.url = url;
                    gwr.uploadHandler = guh.uploadHandler;
                    gwr.SetRequestHeader("Content-Type", "application/json");
                    if (!string.IsNullOrEmpty(authentification))
                        gwr.SetRequestHeader("Authorization", authentification);
                    gwr.chunkedTransfer = false;
                    yield return gwr.SendWebRequest();
                    if (gwr.isNetworkError || gwr.isHttpError)
                    {
                        Debug.Log("{\"error\": {  \"isHttpError\": \"" + gwr.isHttpError + "\",  \"isNetworkError\": \"" + gwr.isNetworkError + "\" } }");
                    }
                    yield break;
                }
            }
	    }

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
            {
                //if not, set instance to this
                instance = this;
            }
            //If instance already exists and it's not this:
            else if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }
	}
}


