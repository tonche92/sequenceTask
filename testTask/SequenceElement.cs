using System.Transactions;
using testTask.Models;

namespace testTask
{
    public class SequenceElement
    {
        public string Current { get; private set; }
        public string NextValue { get; private set; }

        public SequenceElement(int id, TestTaskDbContext _context)
        {
            //Устанавливаем транзакцию, чтобы никто не обращался к нашей строке, пока мы не закончили операцию
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var sequence = _context.Sequences.FirstOrDefault(x => x.Id == id);
                //Слип для запуска 2 одновременных и проверки что они корректно выдадут разные результаты
                //Thread.Sleep(10000);
                if (sequence == null)
                    throw new ArgumentException("Current ID does not exist in database");
                Current = sequence.CurrentValue;
                var prefix = GetPrefixValue(sequence.Prefix);
                int counter;
                //Если изменился префикс или нет предыдущего значения (в первый раз дергаем шаблон) - сбрасываем счетчик. Например начался следующий год/месяц
                //Можно добавить в базу поле resetOnPrefixChange, чтобы управлять этим поведением
                if (!Current.StartsWith(prefix) || sequence.CurrentValue == "")
                    counter = 1;
                else
                {
                    counter = Convert.ToInt32(Current.Substring(prefix.Length)) + 1;
                    //Если превысили установленное значение MaxCounter - сбрасываем счетчик
                    if (counter > sequence.MaxCounter)
                        counter = 1;
                }
                var stringCounter = counter.ToString();
                //MinDigits - параметр отвечающий за минимальную длину циферного ID, чтобы вместо AA20231 было, например AA202300001
                while (stringCounter.Length < sequence.MinDigits)
                    stringCounter = "0" + stringCounter;
                NextValue = prefix + stringCounter;
                sequence.CurrentValue = NextValue;
                //Обновляем CurrentValue в базе и закрываем транзакцию
                _context.SaveChanges();
                transactionScope.Complete();
            }
        }

        private static string GetPrefixValue(string prefix)
        {
            //В этом разделе можем добавлять другие параметры 
            prefix = prefix.Replace("[year]", DateTime.Now.Year.ToString());
            prefix = prefix.Replace("[month]", (DateTime.Now.Month < 10 ? "0" : "") + DateTime.Now.Month.ToString());
            return prefix;
        }
    }
}
