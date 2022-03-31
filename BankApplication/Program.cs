using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankLibrary;
using static BankLibrary.Bank<BankLibrary.Account>;

namespace BankApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank<Account> bank = new Bank<Account>("БельчичБанк");
            bool alive = true;
            while (alive) 
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Title = "БельчичБанк";
                Console.WriteLine("1. Открыть счет \t 2. Вывести средства \t "
                    + "3. Добавить на счет");
                Console.WriteLine("4. Закрыть счет \t 5. Пропустить день \t "
                    + "6. Выйти из БельчичБанк");
                Console.WriteLine("Введите номео пункта: ");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            OpenAccount(bank);
                            break;
                        case 2:
                            Withdraw(bank);
                            break;
                        case 3:
                            Put(bank);
                            break;
                        case 4:
                            CloseAccount(bank);
                            break;
                        case 5:
                            break;
                        case 6:
                            alive = false;
                            continue;
                    }
                    bank.CalculatePercentage();
                }

                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }

        private static void OpenAccount(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму, которую вы вносите на новый счет: ");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Выберите тип счета: 1. До востребования \t 2. Депозит");
            AccountType accountType;
            int Type = Convert.ToInt32(Console.ReadLine());
            if (Type == 2)
                accountType = AccountType.Deposit;
            else
                accountType = AccountType.Ordinary;
            bank.Open(accountType, sum,
                AddSumHandler,
                WithdrawSumHandler,
                (o, e) => Console.WriteLine(e.Message), // Обработчик начисления процентов (лямбда)
                CloseAccountHandler,
                OpenAccountHandler);
        }


        

        private static void Withdraw(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму для вывода средств со счета: ");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введите ID счета: ");
            Int32 id = Convert.ToInt32(Console.ReadLine());
            bank.Put(sum, id);
        }

        private static void Put(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму для пополнения счета: ");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введите ID счета: ");
            int id = Convert.ToInt32(Console.ReadLine());
            bank.Put(sum, id);
        }

        private static void CloseAccount(Bank<Account> bank)
        {
            Console.WriteLine("Введите ID счета для закрытия: ");
            Int32 id = Convert.ToInt32(Console.ReadLine());
            bank.Close(id);
        }

        // Обработчик событий класса Account

        // Обработчик открытия счета
        private static void OpenAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        // Обработчик добавления средств на счет
        private static void AddSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        // Обработчик вывода средств
        private static void WithdrawSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.Sum > 0)
                Console.WriteLine("Покупаем мефедрон");
        }

        // Обработчик закрытия счета
        private static void CloseAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
