using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonitorElectricConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorElectricConsoleApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MathController : Controller
    {
        /// <summary>
        /// (1)-метод (сумма двух чисел)
        /// </summary>
        /// <param name="first">первый аргумент</param>
        /// <param name="second">второй аргумент</param>
        /// <returns>резульат сложения</returns>
        [HttpGet]
        public IActionResult Sum([FromQuery] string first, [FromQuery] string second)
        {
            DateTime currentTime = DateTime.Now;
            if (int.TryParse(first, out int num1) && int.TryParse(second, out int num2))
            {
                int result = num1 + num2;
                using (AppContext app = new AppContext())
                {
                    Result resObj = new Result()
                    {
                        FirstArg = num1,
                        SecondArg = num2,
                        CurrentTime = currentTime,
                        Total = result
                    };
                    app.Results.Add(resObj);
                    app.SaveChanges();
                }
                Console.WriteLine("Сумма чисел {0} и {1} равна {2}", first, second, result);
                return Ok(result);
            }
            else
            {
                return BadRequest("Ошибка при попытке сложения. Используйте числа!");
            }
        }

        /// <summary>
        /// (2)-метод (деление двух чисел)
        /// </summary>
        /// <param name="first">первый аргумент</param>
        /// <param name="second">второй аргумент</param>
        /// <returns>результат деления</returns>
        [HttpGet]
        public IActionResult Div([FromQuery] string first, [FromQuery] string second)
        {
            DateTime currentTime = DateTime.Now;
            if (int.TryParse(first, out int num1) && int.TryParse(second, out int num2))
            {
                double result = (double)num1 / num2;
                using (AppContext app = new AppContext())
                {
                    Result resObj = new Result()
                    {
                        FirstArg = num1,
                        SecondArg = num2,
                        CurrentTime = currentTime,
                        Total = result
                    };
                    app.Results.Add(resObj);
                    app.SaveChanges();
                }
                Console.WriteLine("Результат деления двух чисел: {0} и {1} равен {2}", first, second, result);
                return Ok(result);
            }
            else
            {
                return BadRequest("Ошибка при попытке сложения. Используйте числа!");
            }
        }

        /// <summary>
        /// (3)-метод (два потока)
        /// </summary>
        /// <returns>результат</returns>
        [HttpGet]
        public IActionResult ParallelNoSafely()
        {
            int result = 0;//итоговое число
            int delay = 15000;//выделенное время на выполнения

            Stopwatch sw = new Stopwatch();

            Thread thread1 = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < delay) { result++; };
            });

            Thread thread2 = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < delay) { result--; };
            });

            //запускаем таймер и потоки
            sw.Start();
            thread1.Start();
            thread2.Start();

            //ждем завершения
            thread1.Join();
            thread2.Join();

            sw.Stop();

            Console.WriteLine("Итоговое число равно: {0}", result);
            return Ok(result);
        }

        /// <summary>
        /// (4)-метод (два потока (безопасно))
        /// </summary>
        /// <returns>результат</returns>
        [HttpGet]
        public IActionResult ParallelSafely()
        {
            int result = 0;//итоговое число
            int delay = 15000;//выделенное время на выполнения

            Stopwatch sw = new Stopwatch();
            object locker = new object();//заглушка

            Thread thread1 = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < delay)
                {
                    lock (locker)
                    {
                        result++;
                    }
                }
            });

            Thread thread2 = new Thread(() =>
            {
                while (sw.ElapsedMilliseconds < delay)
                {
                    lock (locker)
                    {
                        result--;
                    }
                }
            });

            //запускаем таймер и потоки
            sw.Start();
            thread1.Start();
            thread2.Start();

            //ждем завершения
            thread1.Join();
            thread2.Join();

            sw.Stop();

            Console.WriteLine("Итоговое число равно: {0}", result);
            return Ok(result);
        }
    }
}