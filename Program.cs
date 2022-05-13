using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UNNV
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = DateTime.Now;
            TimeSpan time_span = DateTime.Now.Subtract(date);
            string source = "hw6.txt";
            string compressed = "hw6.zip";

            Console.WriteLine("{\r\n" + string.Join("\r\n", GroupsNumbersArr(30).Select(gr => "\t{" + string.Join(", ", gr) + "}")) + "\r\n}");
            Console.WriteLine($"\n На выполнение задачи потребовалось: {time_span.TotalSeconds} секунд и {time_span.TotalMilliseconds} миллисекунд");
            GZipStream(source, compressed);

            FileStream file_stream = new FileStream("hw6.txt", FileMode.Create);
            var streamwriter = new StreamWriter(file_stream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);

            Console.WriteLine("{\r\n" + string.Join("\r\n", GroupsNumbersArr(50).Select(gr => "\t{" + string.Join(", ", gr) + "}")) + "\r\n}");
            Console.WriteLine($"\n На выполнение задачи потребовалось : {time_span.TotalSeconds} секунд и ");


        }

        public static int[][] GroupsNumbersArr(uint number)
        {
            // Если переданное число ноль
            if (number == 0) return Array.Empty<int[]>();
            // Если передаваемое число единица, то возращает список групп с обной группой - единицей
            if(number == 1) return new int[][] {new int[] { 1 } };
            // Создание массива для групп
            int[][] groups = new int[(int) Math.Log(number, 2)+1][];
            groups[0] = new int[] { 1 };
            int index_Group = 1;// Индекс добовляемой группы

            //Создание массива чисел содержащего все числа от 1 до заданного
            //Еденица используется как маркер
            //Вместо удаления элементов их значение будет приравниваться нулю
            //После сортировки 1 будет разделять удаленные элмента и оставшиеся
            int[] numbers = new int[number];
            for (int i = 0; i < number; i++)
                numbers[i] = i + 1;

            //Массив с промежуточными данными
            int[] group = new int[number];

            //Цикл пока в массиве индекс единицы не последний
            int index1;
            while ((index1 = Array.BinarySearch(numbers, 1)) != number-1) // проверка индекса единицы
            {
                //Копия элементов в массив группы
                Array.Copy(numbers, group, number);
                int count_Group = 0;
                for (int i = index1 + 1; i < number; i++)
                {
                    if (group[i] != 0) //Пропуск удаленных элементов
                    {    //Удаление из группы всех элементов кратных проверяемому, кроме его самого
                        for (int j = i + 1; j < number; j++)
                        {
                            if (group[j] % group[i] == 0) group[j] = 0;
                        }

                        //Удаление элемента из массива чисел
                        numbers[i] = 0;
                        //Счетчик группы увеличивается
                        count_Group++;
                    }

                }
                //Сортировка массивов после удаления элементов
                Array.Sort(group);
                Array.Sort(numbers);

                //Создание массива для добавления в группы
                //и копирование в него значений старше 1
                int[]_gr = new int[count_Group];
                Array.Copy(group, Array.BinarySearch(group, 1)+1, _gr, 0, count_Group);

                //Добавление группы в массив групп
                groups[index_Group] = _gr;
                index_Group++;
            }
            //Возврат списка групп
            return groups;
        }

        public static void GZipStream(string source, string compressed)
        {
            //Поток для записи сжатого файла
            using (FileStream ss = new FileStream(source, FileMode.OpenOrCreate))
            {//Поток архивации
                using (FileStream ts = File.Create(compressed))
                {
                    using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress) )
                    {
                       //Копируем байты из одного потока в другой
                       ss.CopyTo(cs);
                       Console.WriteLine("Сжатие файла {0} завершено. Было {1} стало {2}", source, ss.Length, ts.Length);
                    }
                    
                }
            }


        }


    }





}

