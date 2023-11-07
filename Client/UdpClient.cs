using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        private const int Port = 2006;

        public static async Task SendRequest(string request)
        {
            //Создается экземпляр класса
            UdpClient udpClient = new();
            //Преобразуется строка запроса в массив байтов
            byte[] requestBytes = Encoding.UTF8.GetBytes(request);
            //Отправляется UDP-запрос на сервер
            await udpClient.SendAsync(requestBytes, requestBytes.Length, new IPEndPoint(IPAddress.Loopback, Port));
            //Получается ответ от сервера
            var result = await udpClient.ReceiveAsync();
            //Преобразует массив байтов ответа в строку
            string response = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine(response);
        }
    }
}
