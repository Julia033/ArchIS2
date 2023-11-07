using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ProgramClient
    {
        static void PrintMenu()
        {
            Console.WriteLine("\nВыберите операцию:");
            Console.WriteLine("\n1)Показать все фильмы");
            Console.WriteLine("2)Выбор фильма по номеру");
            Console.WriteLine("3)Удалить фильм");
            Console.WriteLine("4)Добавить фильм\n");
            Console.WriteLine("Esc - выход из программы");
        }
        static void Main(string[] args)
        {

            PrintMenu();
            ConsoleKey key = ConsoleKey.Enter;

            while (key != ConsoleKey.Escape)
            {

                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        _ = Client.SendRequest("1");
                        PrintMenu();
                        break;
                    case ConsoleKey.D2:
                        {
                            Console.Write("\nВведите номер фильма:");
                            string str = Console.ReadLine();
                            Console.Clear();
                            _ = Client.SendRequest("2" + "," + str);
                            PrintMenu();
                        }
                        break;
                    case ConsoleKey.D3:
                        {
                            Console.Write("\nВведите номер фильма:");
                            string str = Console.ReadLine();
                            Console.Clear();
                            _ = Client.SendRequest("3" + "," + str);
                            PrintMenu();
                        }
                        break;
                    case ConsoleKey.D4:
                        {
                            Console.Write("\nНазвание фильма:");
                            string film = Console.ReadLine();
                            Console.Write("Дата и время показа:");
                            string datetime = Console.ReadLine();
                            Console.Write("Есть ли свободные места:");
                            string available_seats = Console.ReadLine();
                            Console.Write("Количество свободных мест:");
                            string total_seats = Console.ReadLine();
                            Console.Clear();
                            _ = Client.SendRequest("4" + "," + film + "," + datetime + "," + available_seats + "," + total_seats);
                            PrintMenu();
                        }
                        break;
                    case ConsoleKey.D5:
                        {
                            Console.Clear();
                            _ = Client.SendRequest("5");
                            PrintMenu();
                            break;
                        }
                    default:
                        break;
                }
            }

        }
    }
}
