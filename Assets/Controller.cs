using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Controller : MonoBehaviour
{
    string API_URL = "https://localhost:5001/api/TodoItems";
    InputField postInputField;
    Text getMessage;
    Button getButton;
    Button postButton;

    string postText;

    // Start is called before the first frame update
    void Start()
    {
        getMessage = GameObject.Find("GETMessage").GetComponent<Text>();
        postInputField = GameObject.Find("POSTInputField").GetComponent<InputField>();
        getButton = GameObject.Find("GETButton").GetComponent<Button>();
        postButton = GameObject.Find("POSTButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InputText()
    {

        postText = postInputField.text;

    }

    public void OnClickGet()
    {
        StartCoroutine(GetRequest(API_URL));
    }

    public void OnClickPost()
    {
        StartCoroutine(PostRequest());
    }

    IEnumerator GetRequest(string uri)
    {
        var request = UnityWebRequest.Get(uri);
        request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);
            getMessage.text = request.downloadHandler.text;
        }
    }

    IEnumerator PostRequest()
    {
        TodoItem todoItem = new TodoItem();
        todoItem.name = postText;
        string postJson = JsonUtility.ToJson(todoItem);
        Debug.Log(postJson);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(postJson);
        var request = new UnityWebRequest(API_URL, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
