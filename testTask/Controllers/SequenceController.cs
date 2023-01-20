using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using testTask.Models;

namespace testTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SequenceController : ControllerBase
    {
        private TestTaskDbContext _dbcontext;

        public SequenceController(TestTaskDbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        //Вместо обращения по префиксу сделал обращение по ID, на случай если разные заказчики решат использовать один и тот же префикс
        //да и базе комфортнее если primary key будет identity интом, чем если он будет произвольной строкой
        public string NextNumber(int sequenceId)
        {
            //Блокировка производится внутри конструктора SequenceElement
            var sequence = new SequenceElement(sequenceId, _dbcontext);
            var currentValue = sequence.Current;
            var nextValue = sequence.NextValue;
            return nextValue;
        }
    }
}