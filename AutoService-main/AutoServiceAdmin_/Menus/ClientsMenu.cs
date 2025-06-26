using AutoServiceAdmin_.Services;
using AutoServiceAdmin.Utils;
using System;
using System.Linq;

namespace AutoServiceAdmin.Menus
{
    public class ClientsMenu
    {
        private readonly AutoServiceSystem _service;

        public ClientsMenu(AutoServiceSystem service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("КЛИЕНТЫ");
                Console.WriteLine("1. Добавить клиента");
                Console.WriteLine("2. Список всех клиентов");
                Console.WriteLine("3. Найти клиента по ID");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("ФИО: ");
                        var name = Console.ReadLine();
                        Console.Write("Телефон: ");
                        var phone = Console.ReadLine();
                        _service.AddClient(name, phone);
                        ConsoleUiHelper.ShowSuccess("Клиент добавлен!");
                        break;
                    case "2":
                        Console.WriteLine();
                        ConsoleUiHelper.PrintTableHeader("ID", "ФИО", "Телефон");
                        foreach (var client in _service.Clients)
                        {
                            ConsoleUiHelper.PrintTableRow(client.Id.ToString(), client.FullName, client.Phone);
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "3":
                        Console.Write("Введите ID клиента: ");
                        if (int.TryParse(Console.ReadLine(), out int clientId))
                        {
                            var client = _service.Clients.FirstOrDefault(c => c.Id == clientId);
                            if (client != null)
                            {
                                Console.WriteLine();
                                ConsoleUiHelper.PrintHeader("ИНФОРМАЦИЯ О КЛИЕНТЕ");
                                Console.WriteLine($"ФИО: {client.FullName}");
                                Console.WriteLine($"Телефон: {client.Phone}");
                                Console.WriteLine("История заказов:");
                                if (client.OrderHistory.Count == 0)
                                {
                                    Console.WriteLine("  Нет заказов.");
                                }
                                else
                                {
                                    ConsoleUiHelper.PrintTableHeader("ID заказа", "Авто", "Дата");
                                    foreach (var orderId in client.OrderHistory)
                                    {
                                        var order = _service.Orders.FirstOrDefault(o => o.Id == orderId);
                                        if (order != null)
                                        {
                                            var car = _service.Cars.FirstOrDefault(c => c.Id == order.CarId);
                                            if (car != null)
                                                ConsoleUiHelper.PrintTableRow(orderId.ToString(), $"{car.Brand} {car.Model}", order.OrderDate.ToString("dd.MM.yyyy"));
                                        }
                                    }
                                    ConsoleUiHelper.PrintTableFooter();
                                }
                            }
                            else ConsoleUiHelper.ShowError("Клиент не найден!");
                        }
                        else ConsoleUiHelper.ShowError("Неверный ID!");
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "0": return;
                    default: ConsoleUiHelper.ShowError("Неверный пункт меню!"); break;
                }
            }
        }
    }
}