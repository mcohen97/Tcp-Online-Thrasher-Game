using Protocol;

namespace Network
{
    public interface IConnection
    {
        void SendMessage(Package message);

        Package WaitForMessage();

        void Close();

        void SendErrorMessage(string message);

        void SendOkMessage(string message);

        void SendLogOutMessage();

        void StartGame();
        void SendImage(string path);
    }
}