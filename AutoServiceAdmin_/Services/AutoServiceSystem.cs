using AutoServiceAdmin_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceAdmin_.Services
{
    public class AutoServiceSystem
    {
        public List<Car> Cars { get; } = new List<Car>();
        public List<Client> Clients { get; } = new List<Client>();
        public List<Mechanic> Mechanics { get; } = new List<Mechanic>();
        public List<Order> Orders { get; } = new List<Order>();

        // === Методы для работы с клиентами ===
        public void AddClient(string fullName, string phone)
        {
            var newId = Clients.Count > 0 ? Clients.Max(c => c.Id) + 1 : 1;
            Clients.Add(new Client { Id = newId, FullName = fullName, Phone = phone });
        }

        // === Методы для работы с автомобилями ===
        public void AddCar(string brand, string model, int year, string licensePlate)
        {
            var newId = Cars.Count > 0 ? Cars.Max(c => c.Id) + 1 : 1;
            Cars.Add(new Car
            {
                Id = newId,
                Brand = brand,
                Model = model,
                Year = year,
                LicensePlate = licensePlate
            });
        }

        // === Методы для работы с механиками ===
        public void AddMechanic(string fullName, Specialization spec)
        {
            var newId = Mechanics.Count > 0 ? Mechanics.Max(m => m.Id) + 1 : 1;
            Mechanics.Add(new Mechanic
            {
                Id = newId,
                FullName = fullName,
                Specialization = spec
            });
        }

        public void RemoveMechanic(int id)
        {
            var mechanic = Mechanics.FirstOrDefault(m => m.Id == id);
            if (mechanic == null)
                throw new Exception("Механик не найден!");
            Mechanics.Remove(mechanic);
        }

        // === Методы для работы с заказами ===
        public void CreateOrder(int carId, int clientId, string problem, int mechanicId)
        {
            // Проверяем существование сущностей
            if (!Cars.Any(c => c.Id == carId))
                throw new Exception("Автомобиль не найден!");

            if (!Clients.Any(c => c.Id == clientId))
                throw new Exception("Клиент не найден!");

            if (!Mechanics.Any(m => m.Id == mechanicId))
                throw new Exception("Механик не найден!");

            var newId = Orders.Count > 0 ? Orders.Max(o => o.Id) + 1 : 1;

<<<<<<< HEAD
            // Заменяем switch с рекурсивными шаблонами на обычные условия
            decimal cost = 1000; // Базовая стоимость
            if (problem.ToLower().Contains("двигатель"))
                cost = 5000;
            else if (problem.ToLower().Contains("кузов"))
                cost = 3000;
            else if (problem.ToLower().Contains("электрик"))
                cost = 2000;
=======
            decimal cost = RepairCostCalculator.GetCost(problem);
>>>>>>> c46bb07 (Добавлен новый класс – Определение стоимости ремонта для заданного типа работДобавьте файлы проекта.)

            var order = new Order
            {
                Id = newId,
                CarId = carId,
                ClientId = clientId,
                ProblemDescription = problem,
                AssignedMechanicId = mechanicId,
                Cost = cost,
                Status = OrderStatus.InProgress,
                OrderDate = DateTime.Now
            };

            Orders.Add(order);

            // Обновляем историю клиента
            Clients.First(c => c.Id == clientId).OrderHistory.Add(newId);
        }

        // === LINQ-запросы ===

        // 1. Механики, работавшие с конкретным автомобилем
        public List<Mechanic> GetMechanicsByCar(int carId)
        {
            return (from order in Orders
                    where order.CarId == carId
                    join mechanic in Mechanics on order.AssignedMechanicId equals mechanic.Id
                    select mechanic).Distinct().ToList();
        }

        // 2. Количество активных заказов по специализациям
        public Dictionary<Specialization, int> GetActiveOrdersCountBySpecialization()
        {
            return (from order in Orders
                    where order.Status == OrderStatus.InProgress
                    join mechanic in Mechanics on order.AssignedMechanicId equals mechanic.Id
                    group mechanic by mechanic.Specialization into g
                    select new { Spec = g.Key, Count = g.Count() })
                   .ToDictionary(x => x.Spec, x => x.Count);
        }

        // 3. Клиенты с более чем 3 заказами
        public List<Client> GetClientsWithMoreThan3Orders()
        {
            return Clients.Where(c => c.OrderHistory.Count > 3).ToList();
        }

        // 4. Выручка за период
        public decimal GetRevenueForPeriod(DateTime start, DateTime end)
        {
            return Orders
                .Where(o => o.OrderDate >= start && o.OrderDate <= end && o.Status == OrderStatus.Completed)
                .Sum(o => o.Cost);
        }

        // 5. Получить все заказы клиента
        public List<Order> GetClientOrders(int clientId)
        {
            return Orders.Where(o => o.ClientId == clientId).ToList();
        }

        // 6. Клиенты, сделавшие заказы за последний месяц
        public List<Client> GetClientsFromLastMonth()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var clientIds = Orders
                .Where(o => o.OrderDate >= lastMonth)
                .Select(o => o.ClientId)
                .Distinct()
                .ToList();

            return Clients.Where(c => clientIds.Contains(c.Id)).ToList();
        }
    }
}
