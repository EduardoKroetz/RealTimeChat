using Microsoft.AspNetCore.SignalR.Client;


class Program
{
    private static HubConnection _connection;
    private static Guid _chatRoomId;
    private static Guid _userId;

    static async Task Main(string[] args)
    {
        // Configuração da URL do hub SignalR
        var hubUrl = "https://localhost:7136/chathub"; // Altere para a URL do seu hub

        // Cria a conexão com o hub
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        // Configura os métodos para processar as mensagens recebidas
        _connection.On<Guid, string>("ReceiveMessage", (userId, message) =>
        {
            Console.WriteLine($"Received message from user {userId}: {message}");
        });

        _connection.On<Guid>("DeleteMessage", (messageId) =>
        {
            Console.WriteLine($"Message with ID {messageId} was deleted.");
        });

        _connection.On<Guid, string>("UpdateMessage", (messageId, newMessage) =>
        {
            Console.WriteLine($"Message with ID {messageId} was updated to: {newMessage}");
        });

        // Inicia a conexão com o hub
        await _connection.StartAsync();
        Console.WriteLine("Connected to the hub.");

        // Cria IDs de chatRoom e usuário para testes
        _chatRoomId = new Guid("dec7f52d-b2ec-4a86-8fe5-9db645900505");
        _userId = new Guid("e0c60f68-bf1a-4236-8161-956a190306bb");

        Console.WriteLine("Chat room and user ID initialized.");

        // Executa o menu interativo
        await ShowMenuAsync();
    }

    private static async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Send Message");
            Console.WriteLine("2. Update Message");
            Console.WriteLine("3. Delete Message");
            Console.WriteLine("4. Exit");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                await SendMessageAsync();
                break;
                case "2":
                await UpdateMessageAsync();
                break;
                case "3":
                await DeleteMessageAsync();
                break;
                case "4":
                await _connection.StopAsync();
                return;
                default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
            }
        }
    }

    private static async Task SendMessageAsync()
    {
        Console.Write("Enter message content: ");
        var message = Console.ReadLine();

        await _connection.InvokeAsync("SendMessageAsync", _chatRoomId, _userId, message);
        Console.WriteLine("Message sent.");
    }

    private static async Task UpdateMessageAsync()
    {
        Console.Write("Enter message ID to update: ");
        var messageId = Guid.Parse(Console.ReadLine());

        Console.Write("Enter new message content: ");
        var newMessage = Console.ReadLine();

        await _connection.InvokeAsync("UpdateMessageAsync", messageId, newMessage);
        Console.WriteLine("Message updated.");
    }

    private static async Task DeleteMessageAsync()
    {
        Console.Write("Enter message ID to delete: ");
        var messageId = Guid.Parse(Console.ReadLine());

        await _connection.InvokeAsync("DeleteMessageAsync", messageId);
        Console.WriteLine("Message deleted.");
    }
}
