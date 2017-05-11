﻿using System;
using System.Collections.Generic;

namespace RpsBarrier
{
    public class RpsBarrier
    {
        public int MaxOperationsPerSecond { get; set; } = 500;
        static RpsBarrier _instance;
        HashSet<DateTime> _rpsRequestsTimeStamps;

        RpsBarrier()
        {
            _rpsRequestsTimeStamps = new HashSet<DateTime>();
        }

        public static RpsBarrier Instance
        {
            get
            {
                return _instance ?? (_instance = new RpsBarrier());
            }
        }

        public bool CanExecute()
        {
            // Чистим список таймстампов операций
            ResetTimeStamps();

            // Если текущее количество операций меньше, чем максимально допустимое, 
            // то разрешаем выполнение и добавляем текущий отпечаток времени в хэшсет
            var res = _rpsRequestsTimeStamps.Count < MaxOperationsPerSecond;
            if (res)
                _rpsRequestsTimeStamps.Add(DateTime.UtcNow);
            return res;
        }

        void ResetTimeStamps()
        {
            // Удаляем из хешсета те отпечатки времени, которые меньше текущего на 1 секунду
            // чтобы избежать переполнения
            _rpsRequestsTimeStamps.RemoveWhere(dt => dt.AddSeconds(1) < DateTime.UtcNow);
        }
    }

}