using AutoServiceAdmin.Utils;
using AutoServiceAdmin_.Models;
using AutoServiceAdmin_.Services;
using System;

namespace AutoServiceAdmin.Menus
{
    public class MechanicsMenu
    {
        private readonly AutoServiceSystem _service;

        public MechanicsMenu(AutoServiceSystem service)
        {
            _service = service;
        }

        public void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUiHelper.PrintHeader("МЕХАНИКИ");
                Console.WriteLine("1. Добавить механика");
                Console.WriteLine("2. Список механиков");
                Console.WriteLine("3. Уволить механика");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("ФИО: ");
                        var name = Console.ReadLine();
                        Console.WriteLine("Специализация (0-Моторист, 1-Кузовщик, 2-Электрик):");
                        if (int.TryParse(Console.ReadLine(), out int specInt) &&
                            Enum.IsDefined(typeof(Specialization), specInt))
                        {
                            var spec = (Specialization)specInt;
                            _service.AddMechanic(name, spec);
                            ConsoleUiHelper.ShowSuccess("Механик добавлен!");
                        }
                        else ConsoleUiHelper.ShowError("Неверная специализация!");
                        break;
                    case "2":
                        Console.WriteLine();
                        ConsoleUiHelper.PrintTableHeader("ID", "ФИО", "Специализация");
                        foreach (var mechanic in _service.Mechanics)
                        {
                            ConsoleUiHelper.PrintTableRow(mechanic.Id.ToString(), mechanic.FullName, mechanic.Specialization.ToString());
                        }
                        ConsoleUiHelper.PrintTableFooter();
                        ConsoleUiHelper.WaitForInput();
                        break;
                    case "3":
                        Console.Write("Введите ID механика: ");
                        if (int.TryParse(Console.ReadLine(), out int mechanicId))
                        {
                            try
                            {
                                _service.RemoveMechanic(mechanicId);
                                ConsoleUiHelper.ShowSuccess("Механик уволен!");
                            }
                            catch (Exception ex)
                            {
                                ConsoleUiHelper.ShowError(ex.Message);
                            }
                        }
                        else ConsoleUiHelper.ShowError("Неверный ID!");
                        break;
                    case "0": return;
                    default: ConsoleUiHelper.ShowError("Неверный пункт меню!"); break;
                }
            }
        }
    }
}