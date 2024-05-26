using Koakuma.Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestMessageHandler : MessageHandler<MessageType.Test1>
{
    public override async Task HandleMessage(MessageType.Test1 arg)
    {
        Debug.Log("[TestMessageHandler] �յ�Test1");
        Debug.Log("[TestMessageHandler] Content: " + arg.name);
        Debug.Log("[TestMessageHandler] TestReq���� ����: 2109A");
        GameManager.Message.Post<MessageType.Test1Req>(new MessageType.Test1Req() { className = "2109A"}).Coroutine();
        await Task.Yield();
    }
}
