using System;
using System.Collections.Generic;

/// <summary>
/// 사용법 :

//public void MovePlayer() {
// Broadcaster<Transform>.SendEvent("FindChild", transform);
//}

//public void OnTransformFind(Transform child) {
//   transform.parent = child;
//}

//public void OnEnable() {
// Broadcaster<Transform>.EnableListener("FindChild", OnTransformFind);
//}

//public void OnDisable() {
// Broadcaster<Transform>.DisableListener("FindChild", OnTransformFind);
//}

///
/// </summary>

public delegate void Call();
public delegate void Call<T>(T va);

public enum TypeOfMessage
{
    requireReceiver,
    dontRequireReceiver
}

static internal class BroadcasterInner
{
    public static Dictionary<string, Delegate> tableName = new Dictionary<string, Delegate>();
    public static readonly TypeOfMessage receiverMode = TypeOfMessage.requireReceiver;

    public static void OnListenerEnable(string eventName, Delegate listenerEnabled)
    {
        if (!tableName.ContainsKey(eventName))
        {
            tableName.Add(eventName, null);
        }
        Delegate type = tableName[eventName];
        if (type != null && type.GetType() != listenerEnabled.GetType())
        {
            throw new EnabledListenerException(string.Format("Attempting to enable listener with inconsistent signature for event name {0}. Current listeners have type {1} and listener being added has type {2}", eventName, type.GetType().Name, listenerEnabled.GetType().Name));
        }
    }
    public static void OnListenerDisable(string eventName, Delegate listenerDisabled)
    {
        if (tableName.ContainsKey(eventName))
        {
            Delegate type = tableName[eventName];

            if (type == null)
            {
                throw new EnabledListenerException(string.Format("Attempting to disable listener with for event name {0} but current listener is null.", eventName));
            }
            else if (type.GetType() != listenerDisabled.GetType())
                throw new EnabledListenerException(string.Format("Attempting to disable listener with for event name {0} but current listener is null.", eventName));
        }

        else
        {
            throw new EnabledListenerException(string.Format("Attempting to disable listener for type {0} but Broadcaster doesn't know about this event name.", eventName));

        }
    }
    public static void OnListenerDisabled(string eventName)
    {
        if (tableName[eventName] == null)
        {
            tableName.Remove(eventName);
        }

    }
    public static MessageException GenerateMessageException(string eventName)
    {
        return new MessageException(string.Format("Sending message {0} but listeners have a different kind of signature than the broadcaster.", eventName));
    }
    public static void OnBroadcastMessage(string eventName, TypeOfMessage type)
    {
        if (type == TypeOfMessage.requireReceiver && !tableName.ContainsKey(eventName))
        {
            throw new BroadcasterInner.MessageException(string.Format("Sending message {0} but no listener found.", eventName));
        }
    }
    public class EnabledListenerException : Exception
    {
        public EnabledListenerException(string error)
            : base(error)
        {
        }
    }
    public class MessageException : Exception
    {
        public MessageException(string error)
            : base(error)
        {
        }
    }
}

public static class Broadcaster
{
    private static Dictionary<string, Delegate> tableNames = BroadcasterInner.tableName;

    public static void EnableListener(string eventName, Call handlingMethod)
    {
        BroadcasterInner.OnListenerEnable(eventName, handlingMethod);
        tableNames[eventName] = (Call)tableNames[eventName] + handlingMethod;
    }
    public static void DisableListener(string eventName, Call handlingMethod)
    {
        BroadcasterInner.OnListenerDisable(eventName, handlingMethod);
        tableNames[eventName] = (Call)tableNames[eventName] - handlingMethod;
        BroadcasterInner.OnListenerDisabled(eventName);
    }
    public static void SendEvent(string eventName)
    {
        SendEvent(eventName, BroadcasterInner.receiverMode);

    }
    public static void SendEvent(string eventName, TypeOfMessage messageType)
    {
        BroadcasterInner.OnBroadcastMessage(eventName, messageType);
        Delegate type;
        if (tableNames.TryGetValue(eventName, out type))
        {
            Call c = type as Call;
            if (c != null)
            {
                c();
            }
            else
            {
                throw BroadcasterInner.GenerateMessageException(eventName);

            }
        }
    }
}
public static class Broadcaster<T>
{
    private static Dictionary<string, Delegate> tableNames = BroadcasterInner.tableName;

    public static void EnableListener(string eventName, Call<T> handlingMethod)
    {
        BroadcasterInner.OnListenerEnable(eventName, handlingMethod);
        tableNames[eventName] = (Call<T>)tableNames[eventName] + handlingMethod;
    }
    public static void DisableListener(string eventName, Call<T> handlingMethod)
    {
        BroadcasterInner.OnListenerDisable(eventName, handlingMethod);
        tableNames[eventName] = (Call<T>)tableNames[eventName] - handlingMethod;
        BroadcasterInner.OnListenerDisabled(eventName);
    }
    public static void SendEvent(string eventName, T va)
    {
        SendEvent(eventName, va, BroadcasterInner.receiverMode);

    }
    public static void SendEvent(string eventName, T va, TypeOfMessage messageType)
    {
        BroadcasterInner.OnBroadcastMessage(eventName, messageType);
        Delegate type;
        if (tableNames.TryGetValue(eventName, out type))
        {
            Call<T> c = type as Call<T>;
            if (c != null)
            {
                c(va);
            }
            else
            {
                throw BroadcasterInner.GenerateMessageException(eventName);
            }
        }
    }
}