using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_QA.Model
{
    public class WareHouse : Actions
    {
        public int Id { get; set; }       
        public int PharmacyID { get; private set; }
        
        [MaxLength(50)]
        public string NameWareHouse { get; private set; }
        string NamePharmacy { get; set; }

        public override void Create()
        {
            if (!GetMessage()) return;

            //SQL Запрос

            var result = ConnectSQL.Connect.SelectString($@"Use QA Insert into WareHouse (PharmacyID,NameWareHouse) values ('{PharmacyID}','{NameWareHouse}')");
            if (result is null) return;

            Console.WriteLine($"Склад с именем {NameWareHouse} для аптеки {NamePharmacy} успешно создан");
        }

        public override void Remove()
        {
            var list = GetListWareHouse();
            if (list.Count == 0) { Console.WriteLine($"Список складов пуст в БД! \n"); return; }

            Console.WriteLine("Список Складов:\n");
            list.ForEach(x => Console.WriteLine(x));
            Console.Write("\nВыберите склад партии: ");

            var _warehouse = Console.ReadLine();
            var _warehouseid = GetIDWareHouse(_warehouse);

            if (_warehouseid == 0) { Console.WriteLine($"Склад с наименованием {_warehouse} не найден. Попробуйте снова!\n"); Remove(); return; }

            ConnectSQL.Connect.SelectString($@"use qa delete [QA].[dbo].[WareHouse] where id = {_warehouseid}");

            Console.WriteLine($"Товар {_warehouse} успешно Удалён!");

        }

        private bool GetMessage()
        {
            string _namewarehouse;
            string _namepharmacy = "";
            int _id = 0;

            if (!_setname()) return false;
            if (!_findidpharmacy()) return false;

            NameWareHouse = _namewarehouse;
            NamePharmacy = _namepharmacy;
            PharmacyID = _id;
            return true;

            //============

            bool _setname()
            {
                Console.Write("Введите имя склада: ");
                _namewarehouse = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_namewarehouse)) { Console.WriteLine($"Такое имя склада не подходит! Попробуйте снова\n"); return _setname(); }
                return true;
                
            }

            bool _findidpharmacy()
            {
                var list = GetPharmacyList();
                if (list.Count == 0) { Console.WriteLine($"Список аптек пуст в БД! \n"); return false; }
                Console.WriteLine("Список аптек:\n");
                list.ForEach(c => Console.WriteLine(c));

                Console.Write("\nВведите имя аптеки у которой будет создан склад: ");
                _namepharmacy = Console.ReadLine();

                _id = GetIDPharmacy(_namepharmacy);

                if (_id == 0) { Console.WriteLine($"Аптека с наименованием {_namepharmacy} не найдена. Попробуйте снова!\n"); return _findidpharmacy(); }

                return true;
            }

        }      

      
    }
}
