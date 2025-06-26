using AutoServiceAdmin_.Services;
using AutoServiceAdmin.Utils;
using System;

namespace AutoServiceAdmin.Menus
{
    public class CarsMenu
    {
        private readonly AutoServiceSystem _service;

        public CarsMenu(AutoServiceSystem service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("АВТОМОБИЛИ");
                Console.WriteLine("1. Добавить автомобиль");
                Console.WriteLine("2. Список всех автомобилей");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("Марка: ");
                        var brand = Console.ReadLine();
                        Console.Write("Модель: ");
                        var model = Console.ReadLine();
                        Console.Write("Год: ");
                        if (int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.Write("Гос. номер: ");
                            var plate = Console.ReadLine();
                            _service.AddCar(brand, model, year, plate);
                            ConsoleUiHelper.ShowSuccess("Автомобиль добавлен!");
                        }
                        else ConsoleUiHelper.ShowError("Неверный год!");
                        break;
                    case "2":
                        Console.WriteLine();
                        ConsoleUiHelper.PrintTableHeader("ID", "Марка", "Модель", "Год", "Гос. номер");
                        foreach (var car in _service.Cars)
                        {
                            ConsoleUiHelper.PrintTableRow(car.Id.ToString(), car.Brand, car.Model, car.Year.ToString(), car.LicensePlate);
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