using System;
using System.Collections.Generic;
using System.IO;

namespace Practice_2
{
    public static class Application
    {
        private static char[,] start = new char[2,4];                                           // Исходное положение пятнашек
        private static char[,] finish = new char[2,4];                                          // Искомое положение пятнашек

        public static void Main(string[] args)
        {
            Dictionary<string, int> statesList = new Dictionary<string, int>();                 // Словарь состояний ячеек
            Queue<char[,]> queue = new Queue<char[,]>();                                        // Очередь для поиска в глубину
            char[,] cur;                                                                        // Текущее состояние ячеек
            Initialize(ref statesList, ref queue);                                              // Инициализация словаря и очереди
            while (queue.Count != 0)                                                            // Пока в очереди есть состояния
            {
                cur = queue.Dequeue();                                                          // Получить состояние из очереди
                if (ToString(cur) == ToString(finish))                                          // Если состояние искомое
                {
                    File.AppendAllText("output.txt", statesList[ToString(cur)].ToString());     // Вывести в файл количество перемещений
                    return;
                }

                
                for (int di = -1; di <= 1; di++)                                                // Для каждого сдвига влево или вправо
                {
                    for (int dj = -1; dj <= 1; dj++)                                            // Для каждого сдвига вниз или вверх
                    {
                        if (di * di + dj * dj == 1)                                             // Если он существует
                        {
                            char[,] next = (cur.Shift(di, dj));                                 // Выполнить сдвиг
                            if (!statesList.ContainsKey(ToString(next)))                        // Если состояние не существует
                            {
                                statesList.Add(ToString(next), statesList[ToString(cur)] + 1);  // Добавить в словарь с количеством +1
                                queue.Enqueue(next);                                            // Записать в очередь
                            }
                        }
                    }
                }
            }
            File.WriteAllText("output.txt", (-1).ToString());
        }
        
        /// <summary>
        /// Инициализация коллекций.
        /// </summary>
        /// <param name="statesList">Словарь состояний.</param>
        /// <param name="queue">Очередь состояний.</param>
        private static void Initialize(ref Dictionary<string, int> statesList, ref Queue<char[,]> queue)
        {
            string input = File.ReadAllText("input.txt").Replace("\r", "").Replace("\n", ""); // Получение входной строки
            start.ReadStart(input);
            finish.ReadFinish(input);
            statesList.Add(ToString(start), 0);
            queue.Enqueue(start);
        }
        /// <summary>
        /// Сдвиг решётки в массиве.
        /// </summary>
        /// <param name="pred">Предыдущее состояние.</param>
        /// <param name="di">Количество ячеек для сдвига по i.</param>
        /// <param name="dj">Количество ячеек для сдвига по j.</param>
        /// <returns></returns>
        public static char[,] Shift(this char[,] pred, int di, int dj)
        {
            char[,] next = (char[,])pred.Clone();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (next[i, j] == '#')
                    {
                        int ni = i + di;
                        int nj = j + dj;
                        if (0 <= ni && ni < 2 && 0 <= nj && nj < 4)
                        {
                            Swap(ref next[i, j], ref next[ni, nj]);
                        }
                        return next;
                    }
                }
            }
            throw new ArgumentException();
        }
        /// <summary>
        /// Меняет местами два значения.
        /// </summary>
        /// <param name="left">Левое значение.</param>
        /// <param name="right">Правое значение.</param>
        public static void Swap(ref char left, ref char right)
        {
            char temp = left;
            left = right;
            right = temp;
        }
        /// <summary>
        /// Чтение начального состояния ячеек.
        /// </summary>
        /// <param name="a">Массив символов.</param>
        /// <param name="input">Входная строка.</param>
        public static void ReadStart(this char[,] a, string input)
        {
            Int16 count = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = input[count];
                    count++;
                }
            }
        }
        /// <summary>
        /// Чтение конечного состояния.
        /// </summary>
        /// <param name="a">Массив символов.</param>
        /// <param name="input">Входная строка.</param>
        public static void ReadFinish(this char[,] a, string input)
        {
            Int16 count = 8;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = input[count];
                    count++;
                }
            }
        }
        /// <summary>
        /// Приведение массива в строку.
        /// </summary>
        /// <param name="arr">Массив для примедения.</param>
        /// <returns></returns>
        private static string ToString(char[,] arr)
        {
            string output = "";
            foreach (char symbol in arr)
            {
                output += symbol;
            }

            return output;
        }
    }
}