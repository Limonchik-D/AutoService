using AutoServiceAdmin_.Services;
using AutoServiceAdmin.Utils;
using System;

namespace AutoServiceAdmin.Menus
{
    public class ReportsMenu
    {
        private readonly AutoServiceSystem _service;

        public ReportsMenu(AutoServiceSystem service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("ОТЧЕТЫ");
                Console.WriteLine("1. Клиенты за последний месяц");
                Console.WriteLine("2. Механики по автомобилю");
                Console.WriteLine("3. Активные заказы по специализациям");
                Console.WriteLine("4. Клиенты с 3+ заказами");
                Console.WriteLine("5. Выручка за период");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите отчет: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        var clients = _service.GetClientsFromLastMonth();
                        Console.WriteLine();
                        ConsoleUiHelper.PrintHeader("Клиенты за последний месяц");
                        ConsoleUiHelper.PrintTableHeader("ФИО", "Телефон");
                        foreach (var client in clients)
                        {
                            ConsoleUiHelper.PrintTableRow(client.FullName, client.Phone);
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "2":
                        Console.Write("Введите ID автомобиля: ");
                        if (int.TryParse(Console.ReadLine(), out int carId))
                        {
                            var mechanics = _service.GetMechanicsByCar(carId);
                            Console.WriteLine();
                            ConsoleUiHelper.PrintHeader("Механики, работавшие с автомобилем");
                            ConsoleUiHelper.PrintTableHeader("ФИО", "Специализация");
                            foreach (var mechanic in mechanics)
                            {
                                ConsoleUiHelper.PrintTableRow(mechanic.FullName, mechanic.Specialization.ToString());
                            }
                            ConsoleUiHelper.PrintTableFooter();
                        }
                        else ConsoleUiHelper.ShowError("Неверный ID!");
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "3":
                        var ordersBySpec = _service.GetActiveOrdersCountBySpecialization();
                        Console.WriteLine();
                        ConsoleUiHelper.PrintHeader("Активные заказы по специализациям");
                        ConsoleUiHelper.PrintTableHeader("Специализация", "Кол-во заказов");
                        foreach (var item in ordersBySpec)
                        {
                            ConsoleUiHelper.PrintTableRow(item.Key.ToString(), item.Value.ToString());
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "4":
                        var loyalClients = _service.GetClientsWithMoreThan3Orders();
                        Console.WriteLine();
                        ConsoleUiHelper.PrintHeader("Клиенты с 3+ заказами");
                        ConsoleUiHelper.PrintTableHeader("ФИО", "Кол-во заказов");
                        foreach (var client in loyalClients)
                        {
                            ConsoleUiHelper.PrintTableRow(client.FullName, client.OrderHistory.Count.ToString());
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "5":
                        Console.Write("Начальная дата (дд.мм.гггг): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime start))
                        {
                            Console.Write("Конечная дата (дд.мм.гггг): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime end))
                            {
                                var revenue = _service.GetRevenueForPeriod(start, end);
                                Console.WriteLine($"\nВыручка: {revenue} руб.");
                            }
                            else ConsoleUiHelper.ShowError("Неверная дата!");
                        }
                        else ConsoleUiHelper.ShowError("Неверная дата!");
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "0": return;
                    default: ConsoleUiHelper.ShowError("Неверный пункт меню!"); break;
                }
            }
        }
    }
}