using Logic;
using Protocol;
namespace Logic
{
    public interface IConnection
    {
        void SendMessageToClient(Package message);

        Package WaitForClientMessage();

        void Close();
    }
}