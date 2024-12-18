using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab5
{
    class Program
    {
        enum SubscriberType
        {
            Individual,
            Business,
            Government
        }

        struct Subscriber
        {
            public int AccountNumber;
            public string FullName;
            public string Address;
            public string PhoneNumber;
            public string ContractNumber;
            public DateTime ContractDate;
            public bool HasBenefits;
            public SubscriberType Type;
            public string TariffPlan;

            public override string ToString()
            {
                return $"Рахунок: {AccountNumber}, Ім'я: {FullName}, Адреса: {Address}, Телефон: {PhoneNumber}, " +
                       $"Договір: {ContractNumber}, Дата: {ContractDate.ToShortDateString()}, Пільги: {HasBenefits}, " +
                       $"Тип: {Type}, Тариф: {TariffPlan}";
            }
        }

        static string filePath = "subscribers.txt";

        static void Main()
        {
            List<Subscriber> subscribers = new List<Subscriber>();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Додати абонента");
                Console.WriteLine("2. Показати всіх абонентів");
                Console.WriteLine("3. Шукати абонента (за договором, ім'ям, тарифом)");
                Console.WriteLine("4. Завантажити дані з файлу");
                Console.WriteLine("5. Зберегти дані у файл");
                Console.WriteLine("6. Вихід");
                Console.Write("Виберіть опцію: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        subscribers.Add(CreateSubscriber());
                        break;
                    case 2:
                        DisplaySubscribers(subscribers);
                        break;
                    case 3:
                        SearchSubscribers(subscribers);
                        break;
                    case 4:
                        subscribers = LoadSubscribersFromFile();
                        break;
                    case 5:
                        SaveSubscribersToFile(subscribers);
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
        }

        static Subscriber CreateSubscriber()
        {
            Console.Write("Введіть номер рахунку: ");
            int accountNumber = int.Parse(Console.ReadLine());

            Console.Write("Введіть П.І.П.: ");
            string fullName = Console.ReadLine();

            Console.Write("Введіть адресу: ");
            string address = Console.ReadLine();

            Console.Write("Введіть номер телефону: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Введіть номер договору: ");
            string contractNumber = Console.ReadLine();

            Console.Write("Введіть дату укладення договору (рррр-мм-дд): ");
            DateTime contractDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Наявність пільг (true/false): ");
            bool hasBenefits = bool.Parse(Console.ReadLine());

            Console.Write("Виберіть тип абонента (0 - Individual, 1 - Business, 2 - Government): ");
            SubscriberType type = (SubscriberType)int.Parse(Console.ReadLine());

            Console.Write("Введіть тарифний план: ");
            string tariffPlan = Console.ReadLine();

            return new Subscriber
            {
                AccountNumber = accountNumber,
                FullName = fullName,
                Address = address,
                PhoneNumber = phoneNumber,
                ContractNumber = contractNumber,
                ContractDate = contractDate,
                HasBenefits = hasBenefits,
                Type = type,
                TariffPlan = tariffPlan
            };
        }

        static void DisplaySubscribers(List<Subscriber> subscribers)
        {
            if (subscribers.Count == 0)
            {
                Console.WriteLine("Дані відсутні.");
                return;
            }

            foreach (var subscriber in subscribers)
            {
                Console.WriteLine(subscriber);
            }
        }

        static void SearchSubscribers(List<Subscriber> subscribers)
        {
            Console.WriteLine("Виберіть критерій пошуку:");
            Console.WriteLine("1. За номером договору");
            Console.WriteLine("2. За ім'ям");
            Console.WriteLine("3. За тарифом");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Введіть номер договору: ");
                    string contractNumber = Console.ReadLine();
                    var byContract = subscribers.Where(s => s.ContractNumber == contractNumber);
                    DisplaySubscribers(byContract.ToList());
                    break;

                case 2:
                    Console.Write("Введіть ім'я: ");
                    string fullName = Console.ReadLine();
                    var byName = subscribers.Where(s => s.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));
                    DisplaySubscribers(byName.ToList());
                    break;

                case 3:
                    Console.Write("Введіть тарифний план: ");
                    string tariffPlan = Console.ReadLine();
                    var byTariff = subscribers.Where(s => s.TariffPlan.Contains(tariffPlan, StringComparison.OrdinalIgnoreCase));
                    DisplaySubscribers(byTariff.ToList());
                    break;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        static void SaveSubscribersToFile(List<Subscriber> subscribers)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var subscriber in subscribers)
                {
                    writer.WriteLine($"{subscriber.AccountNumber};{subscriber.FullName};{subscriber.Address};" +
                                     $"{subscriber.PhoneNumber};{subscriber.ContractNumber};{subscriber.ContractDate};" +
                                     $"{subscriber.HasBenefits};{subscriber.Type};{subscriber.TariffPlan}");
                }
            }
            Console.WriteLine("Дані збережено у файл.");
        }

        static List<Subscriber> LoadSubscribersFromFile()
        {
            List<Subscriber> subscribers = new List<Subscriber>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не знайдено.");
                return subscribers;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    subscribers.Add(new Subscriber
                    {
                        AccountNumber = int.Parse(parts[0]),
                        FullName = parts[1],
                        Address = parts[2],
                        PhoneNumber = parts[3],
                        ContractNumber = parts[4],
                        ContractDate = DateTime.Parse(parts[5]),
                        HasBenefits = bool.Parse(parts[6]),
                        Type = Enum.Parse<SubscriberType>(parts[7]),
                        TariffPlan = parts[8]
                    });
                }
            }

            Console.WriteLine("Дані завантажено з файлу.");
            return subscribers;
        }
    }
}
