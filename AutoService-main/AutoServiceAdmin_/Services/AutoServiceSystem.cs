using AutoServiceAdmin_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

            // Заменяем switch с рекурсивными шаблонами на обычные условия
            decimal cost = 1000; // Базовая стоимость
            if (problem.ToLower().Contains("двигатель"))
                cost = 5000;
            else if (problem.ToLower().Contains("кузов"))
                cost = 3000;
            else if (problem.ToLower().Contains("электрик"))
                cost = 2000;

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

        public void SaveClientsToTxt()
        {
            using (var writer = new StreamWriter("clients.txt"))
            {
                foreach (var client in Clients)
                {
                    writer.WriteLine($"{client.Id}|{client.FullName}|{client.Phone}");
                }
            }
        }

        public void LoadClientsFromTxt()
        {
            Clients.Clear();
            if (!File.Exists("clients.txt")) return;
            foreach (var line in File.ReadAllLines("clients.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    Clients.Add(new Client
                    {
                        Id = int.Parse(parts[0]),
                        FullName = parts[1],
                        Phone = parts[2],
                        OrderHistory = new List<int>()
                    });
                }
            }
        }
        public void SaveCarsToTxt()
        {
            using (var writer = new StreamWriter("cars.txt"))
            {
                foreach (var car in Cars)
                    writer.WriteLine($"{car.Id}|{car.Brand}|{car.Model}|{car.Year}|{car.LicensePlate}");
            }
        }

        public void LoadCarsFromTxt()
        {
            Cars.Clear();
            if (!File.Exists("cars.txt")) return;
            foreach (var line in File.ReadAllLines("cars.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    Cars.Add(new Car
                    {
                        Id = int.Parse(parts[0]),
                        Brand = parts[1],
                        Model = parts[2],
                        Year = int.Parse(parts[3]),
                        LicensePlate = parts[4]
                    });
                }
            }
        }

        public void SaveMechanicsToTxt()
        {
            using (var writer = new StreamWriter("mechanics.txt"))
            {
                foreach (var mechanic in Mechanics)
                    writer.WriteLine($"{mechanic.Id}|{mechanic.FullName}|{(int)mechanic.Specialization}");
            }
        }

        public void LoadMechanicsFromTxt()
        {
            Mechanics.Clear();
            if (!File.Exists("mechanics.txt")) return;
            foreach (var line in File.ReadAllLines("mechanics.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    Mechanics.Add(new Mechanic
                    {
                        Id = int.Parse(parts[0]),
                        FullName = parts[1],
                        Specialization = (Specialization)int.Parse(parts[2])
                    });
                }
            }
        }

        public void SaveOrdersToTxt()
        {
            using (var writer = new StreamWriter("orders.txt"))
            {
                foreach (var order in Orders)
                    writer.WriteLine($"{order.Id}|{order.CarId}|{order.ClientId}|{order.ProblemDescription}|{order.AssignedMechanicId}|{order.Cost}|{(int)order.Status}|{order.OrderDate:O}");
            }
        }

        public void LoadOrdersFromTxt()
        {
            Orders.Clear();
            if (!File.Exists("orders.txt")) return;
            foreach (var line in File.ReadAllLines("orders.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length >= 8)
                {
                    Orders.Add(new Order
                    {
                        Id = int.Parse(parts[0]),
                        CarId = int.Parse(parts[1]),
                        ClientId = int.Parse(parts[2]),
                        ProblemDescription = parts[3],
                        AssignedMechanicId = int.Parse(parts[4]),
                        Cost = decimal.Parse(parts[5]),
                        Status = (OrderStatus)int.Parse(parts[6]),
                        OrderDate = DateTime.Parse(parts[7])
                    });
                }
            }
        }
    }
}
