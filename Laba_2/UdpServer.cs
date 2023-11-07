using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Laba_2
{
    public class UdpServer
    {
        //Объявление и инициализация логгера
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const int Port = 2006;
        private const string FileName = "data.csv";
        private static List<Cinema> cinemas = new();
        static async Task Main(string[] args)
        {
            //Cоздание экземпляра класса UdpClient с параметром Port
            using UdpClient udpClient = new(Port);
            cinemas = ReadData();

            //Запись в лог информации о запуске сервера и прослушивании порта
            logger.Info($"Server started. Listening on port {Port}");

            while (true)
            {
                //Получение данных от клиента
                var result = await udpClient.ReceiveAsync();
                //Преобразование байтового массива в строку
                string request = Encoding.UTF8.GetString(result.Buffer);
                //Запись в лог информации о полученном запросе от клиента
                logger.Info($"Received request from {result.RemoteEndPoint}: {request}");
                //Обработка запроса
                string response = ProcessRequest(request);
                //Преобразование строки в байтовый массив
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                //Отправка ответа клиенту
                _ = udpClient.SendAsync(responseBytes, responseBytes.Length, result.RemoteEndPoint);
                //Запись в лог информации о отправленном ответе клиенту
                logger.Info($"Sent response to {result.RemoteEndPoint}: {response}");
            }   
        }
        private static List<Cinema> ReadData()
        {
            //Проверка наличия файла
            if (File.Exists(FileName))
            {
                using StreamReader reader = new(FileName);
                string line;
                //Чтение строки из файла
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    //Проверка, что количество частей равно 4
                    if (parts.Length == 4)
                    {
                        // Создание экземпляра класса
                        Cinema cinema = new()
                        {
                            Film = parts[0].Trim(),
                            DateTime = parts[1].Trim(),
                            Available_seats = bool.Parse(parts[2]),
                            Total_seats = int.Parse(parts[3]),
                        };
                        cinemas.Add(cinema);
                    }
                }
            }

            return cinemas;
        }

        private static string ProcessRequest(string request)
        {
            string[] parts = request.Split(',');
            string command = parts[0];
            switch (command)
            {
                case "1":
                    //Вывод всех записей
                    return GetAllFilms();
                case "2":
                    //Вывод записи по номеру
                    try
                    {
                        return GetCinema(int.Parse(parts[1]) - 1);
                    }
                    catch (Exception)
                    {

                        return $"{parts[1]} не является номером!";
                    }
                case "3":
                    //Удаление записи
                    try
                    {
                        DeleteCinema(int.Parse(parts[1]) - 1);
                        return "Фильм удален.";
                    }
                    catch (Exception)
                    {

                        return $"{parts[1]} не является номером!";
                    }

                case "4":
                    //Добавление записи
                    try
                    {
                        AddCinema(parts[1], parts[2], bool.Parse(parts[3]), int.Parse(parts[4]));
                        return "Фильм добавлен.";
                    }
                    catch (Exception)
                    {
                        return "Данные введены неверно!";
                    }
                default:
                    return "Недопустимая команда.";
            }
        }

        private static string GetAllFilms()
        {
            // Создание экземпляра класса
            StringBuilder builder = new();
            //Итерация по списку cinemas и добавление строк в экземпляр класса
            for (int i = 0; i < cinemas.Count; i++)
            {
                string cinemaString = $"Запись {i + 1}: \nНазвание фильма: {cinemas[i].Film} \nДата и время показа: {cinemas[i].DateTime} \nНаличие свободных мест: {cinemas[i].Available_seats} \nКоличество свободных мест: {cinemas[i].Total_seats} \n";
                builder.AppendLine(cinemaString);
            }
            return builder.ToString();
        }

        private static string GetCinema(int index)
        {
            if (index >= 0 && index < cinemas.Count)
            {
                return $"Запись {index + 1}: \nНазвание фильма: {cinemas[index].Film} \nДата и время показа: {cinemas[index].DateTime} \nНаличие свободных мест: {cinemas[index].Available_seats} \nКоличество свободных мест: {cinemas[index].Total_seats} \n";
            }
            return "Недопустимый индекс.";
        }

        private static void DeleteCinema(int index)
        {
            if (index >= 0 && index < cinemas.Count)
            {
                cinemas.RemoveAt(index);
                SaveData();
            }
        }

        private static void AddCinema(string film, string datetime, bool available_seats, int total_seats)
        {
            cinemas.Add(new Cinema { Film = film, DateTime = datetime,  Available_seats = available_seats, Total_seats = total_seats });
            SaveData();
        }

        private static void SaveData()
        {
            //Итерация по списку cinemas и запись каждого фильма в файл в формате строки
            using var writer = new StreamWriter(FileName);
            foreach (Cinema cinema in cinemas)
            {
                string line = $"{cinema.Film} , {cinema.DateTime} , {cinema.Available_seats} , {cinema.Total_seats}";
                writer.WriteLine(line);
            }
        }

    }
}
