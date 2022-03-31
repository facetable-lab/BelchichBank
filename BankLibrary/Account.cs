using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class Account : IAccount
    {
        // Событие, возникающее при снятии денег
        protected internal event AccountStateHandler Withdrawed;
        // Событие, возникающее при добавлении денег на счет
        protected internal event AccountStateHandler Added;
        // Событие, возникающее при открытии счета
        protected internal event AccountStateHandler Opened;
        // Событие, возникающее при закрытии счета
        protected internal event AccountStateHandler Closed;
        // Событие, возникающее при наличии процентов
        protected internal event AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;
        protected decimal _sum; // Для хранения суммы
        protected int _percentage; // Для хранения процента
        protected int _days = 0; // Для хранения кол-ва дней с открытыя счета

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = counter++;
        }
        
        // Текущая сумма на счете
        public decimal CurrentSum
        {
            get { return _sum; }
        }

        public int Precentage
        {
            get { return _percentage;}
        }

        public int Id
        {
            get { return _id; }
        }

        // Вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }
        // Вызов отдельных событий
        // Для каждого события определяется свой внутренний метод
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило " + sum, sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (sum <= _sum)
            {
                _sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs("Сумма " + sum
                    + " снята со счета " + _id, sum));
            } else
            {
                OnWithdrawed(new AccountEventArgs("Недостаточно средств на счете " + _id, sum));
            }
            return result;
        }

        // Открытие счета
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs("Открыт новый депозит. Ваш ID счета: " + this._id, this._sum));
        }

        // Закрытие счета
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs("Счет " + this._id + " закрыт. Итоговая сумма: " + CurrentSum, CurrentSum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }
        
        // Начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = _sum * _percentage / 100;
            _sum += increment;
            OnCalculated(new AccountEventArgs("Начислены проценты в размере: " + increment, increment));
        }
    }
}
