using Logic;
using Protocol;
namespace Logic
{
    public interface IConnection
    {
        void SendMessageToClient(Package message);

        Package WaitForClientMessage();

        void Close();

        void SendErrorMessage(string message);
        void SendOkMessage(string message);
    }
}