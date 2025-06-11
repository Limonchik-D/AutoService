using AutoServiceAdmin_.Models;
using AutoServiceAdmin_.Services;
using AutoServiceAdmin.Menus;
using AutoServiceAdmin.Utils;
using System;

namespace AutoServiceAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new AutoServiceSystem();
            InitializeTestData(service);

            var clientsMenu = new ClientsMenu(service);
            var carsMenu = new CarsMenu(service);
            var mechanicsMenu = new MechanicsMenu(service);
            var ordersMenu = new OrdersMenu(service);
            var reportsMenu = new ReportsMenu(service);

            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("АДМИНИСТРАТОР АВТОСЕРВИСА");
                Console.WriteLine("1. Управление клиентами");
                Console.WriteLine("2. Управление автомобилями");
                Console.WriteLine("3. Управление механиками");
                Console.WriteLine("4. Управление заказами");
                Console.WriteLine("5. Отчеты");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите раздел: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": clientsMenu.Show(); break;
                    case "2": carsMenu.Show(); break;
                    case "3": mechanicsMenu.Show(); break;
                    case "4": ordersMenu.Show(); break;
                    case "5": reportsMenu.Show(); break;
                    case "0": return;
                    default: ConsoleUiHelper.ShowError("Неверный пункт меню!"); break;
                }
            }
        }

        // Инициализация тестовых данных
        static void InitializeTestData(AutoServiceSystem service)
        {
            service.AddClient("Иванов Иван Иванович", "+79111111111");
            service.AddClient("Петров Петр Петрович", "+79222222222");

            service.AddCar("Toyota", "Camry", 2015, "А123БВ77");
            service.AddCar("BMW", "X5", 2018, "У456КХ77");

            service.AddMechanic("Смирнов Алексей", Specialization.Motorist);
            service.AddMechanic("Кузнецов Дмитрий", Specialization.Bodyworker);
            service.AddMechanic("Васильев Андрей", Specialization.Electrician);
        }
    }
}