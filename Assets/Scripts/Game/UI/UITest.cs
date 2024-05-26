using Koakuma.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Button btn;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Message.Subscribe<MessageType.Test1Req>(OnTest1Req);
        btn.onClick.AddListener(() =>
        {
            Debug.Log("[UITest] Test1≤‚ ‘ ∑¢ÀÕ: TESTNAME");
            GameManager.Message.Post<MessageType.Test1>(new MessageType.Test1() { name = "TESTNAME" }).Coroutine();
        });
    }
    private async Task OnTest1Req(MessageType.Test1Req arg)
    {
        Debug.Log("[UITest]  ’µΩTest1Req");
        Debug.Log("[UITest] Content: " + arg.className);
        await Task.Yield();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
