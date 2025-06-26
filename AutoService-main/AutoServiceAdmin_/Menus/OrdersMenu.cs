using AutoServiceAdmin_.Services;
using AutoServiceAdmin.Utils;
using System;
using System.Linq;

namespace AutoServiceAdmin.Menus
{
    public class OrdersMenu
    {
        private readonly AutoServiceSystem _service;

        public OrdersMenu(AutoServiceSystem service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("ЗАКАЗЫ");
                Console.WriteLine("1. Создать заказ");
                Console.WriteLine("2. Список всех заказов");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("ID клиента: ");
                        if (int.TryParse(Console.ReadLine(), out int clientId))
                        {
                            Console.Write("ID автомобиля: ");
                            if (int.TryParse(Console.ReadLine(), out int carId))
                            {
                                Console.Write("ID механика: ");
                                if (int.TryParse(Console.ReadLine(), out int mechanicId))
                                {
                                    Console.Write("Описание проблемы: ");
                                    var problem = Console.ReadLine();
                                    try
                                    {
                                        _service.CreateOrder(carId, clientId, problem, mechanicId);
                                        ConsoleUiHelper.ShowSuccess("Заказ создан!");
                                    }
                                    catch (Exception ex)
                                    {
                                        ConsoleUiHelper.ShowError(ex.Message);
                                    }
                                }
                                else ConsoleUiHelper.ShowError("Неверный ID механика!");
                            }
                            else ConsoleUiHelper.ShowError("Неверный ID автомобиля!");
                        }
                        else ConsoleUiHelper.ShowError("Неверный ID клиента!");
                        break;
                    case "2":
                        Console.WriteLine();
                        ConsoleUiHelper.PrintTableHeader("ID", "Клиент", "Авто", "Механик", "Статус");
                        foreach (var order in _service.Orders)
                        {
                            var client = _service.Clients.FirstOrDefault(c => c.Id == order.ClientId);
                            var car = _service.Cars.FirstOrDefault(c => c.Id == order.CarId);
                            var mechanic = _service.Mechanics.FirstOrDefault(m => m.Id == order.AssignedMechanicId);
                            if (client != null && car != null && mechanic != null)
                                ConsoleUiHelper.PrintTableRow(
                                    $"#{order.Id}",
                                    client.FullName,
                                    $"{car.Brand} {car.Model}",
                                    mechanic.FullName,
                                    order.Status.ToString()
                                );
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "0": return;
                    default: ConsoleUiHelper.ShowError("Неверный пункт меню!"); break;
                }
            }
        }
    }
}