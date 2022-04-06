using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_QA.Model
{
    public class Lot : Actions
    {
        public int Id { get; set; }
        public int ProductID { get; private set; }
        public int WareHouseID { get; private set; }
        public int Count { get; private set; }
        string WareHouse { get; set; }
        string Product { get; set; }


        public override void Create()
        {
            if (!GetMessage()) return;

            //SQL
            var result = ConnectSQL.Connect.SelectString($@"Use QA Insert into Lot (ProductID,WareHouseID,[Count]) values ('{ProductID}','{WareHouseID}',{Count})");
            if (result is null) return;

            Console.WriteLine($"Партия товара {Product}, склада {WareHouse} с количеством {Count} успешно создана");
        }

        public override void Remove()
        {
            var list = GetListLOT();
            if (list.Count == 0) { Console.WriteLine($"Список партий пуст в БД! \n"); return; }
            
            Console.WriteLine(String.Format("{0,5}    |{1,5}    |{2,5}   |{3,5} |{4,5}  ", "Товар", "Склад", "Аптека", "Кол-во", "Номер партии"));


            //list.ForEach(x => Console.WriteLine($"{x.NameProduct}       | {x.NameWareHouse}     | {x.NamePharmacy}      | {x.Count}       | {x.ID}"));
            //String.Format("{0}|{1}|{2}|{3}|{4}", x.NameProduct, x.NameWareHouse, x.NamePharmacy, x.Count, x.ID));

            list.ForEach(x => Console.WriteLine(String.Format("{0,5}  |{1,5}  |{2,5}  |{3,5}  |{4,5}  ", x.NameProduct, x.NameWareHouse, x.NamePharmacy, x.Count, x.ID)));
            Console.Write("\nВыберите номер партии: ");

            var _IDinput = Console.ReadLine();
            var _ID = GetLOTID(_IDinput);

            if (_ID == 0) { Console.WriteLine($"Партия с номером {_IDinput} не найден. Попробуйте снова!\n"); Remove(); return; }

            ConnectSQL.Connect.SelectString($@"use qa delete [QA].[dbo].[LOT] where id = {_ID}");

            var info = list.Where(c => c.ID == _IDinput).FirstOrDefault();

            Console.WriteLine($"Партия с номером {_ID}, товаром - {info.NameProduct}, Складом - {info.NameWareHouse}, Аптекой - {info.NamePharmacy} успешно Удалён!");
        }

        private bool GetMessage()
        {
            int _count = 0;
            int _productid = 0;
            int _warehouseid = 0;
            string _product = "";
            string _warehouse = "";

            if (!_setcount()) return false;
            if (!_findproductid()) return false;
            if (!_findwarehouse()) return false;
            
            Count = _count;
            ProductID = _productid;
            WareHouseID = _warehouseid;
            WareHouse = _warehouse;
            Product = _product;

            return true;

            //=====================

            bool _findwarehouse()
            {
                var list = GetListWareHouse();
                if (list.Count == 0) { Console.WriteLine($"Список складов пуст в БД! \n"); return false; }

                Console.WriteLine("Список Складов:\n");
                list.ForEach(x => Console.WriteLine(x));
                Console.Write("\nВыберите склад партии: ");

                _warehouse = Console.ReadLine();
                _warehouseid = GetIDWareHouse(_warehouse);

                if (_warehouseid == 0) { Console.WriteLine($"Склад с наименованием {_warehouse} не найден. Попробуйте снова!\n"); return _findwarehouse(); }
                return true;

            }

            bool _findproductid()
            {
                var list = GetListProduct();
                if (list.Count == 0) { Console.WriteLine($"Список товаров пуст в БД! \n"); return false; }

                Console.WriteLine("Список товаров:\n");
                list.ForEach(x => Console.WriteLine(x));
                Console.Write("Выберите товар, который входит в партию:\n");

                _product = Console.ReadLine();
                _productid = GetIDProduct(_product);

                if (_productid == 0) { Console.WriteLine($"Товар с наименованием {_product} не найден. Попробуйте снова!\n"); return _findproductid(); }
                return true;
            }

            bool _setcount()
            {
                Console.Write("Введите кол-во партии: ");
                if (!int.TryParse(Console.ReadLine(), out _count)) { Console.WriteLine($"Необходимо ввести число. Попробуйте снова\n"); return _setcount();  };

                if (_count <= 0) { Console.WriteLine($"{_count} - Не подходящее число. Кол-во партии должно превышать 0\n"); return _setcount(); };
                return true;
            }


        }


        int GetLOTID(string ID)
        {
            return ConnectSQL.Connect.SelectStringInt($@"use QA select ID from Lot where ID = '{ID}'");
        }

        List<ListLot> GetListLOT()
        {
            var result = ConnectSQL.Connect.GetDatas($@"use qa
            SELECT p.NameProduct Товар, wh.NameWareHouse Склад, ph.NamePharmacy Аптека, Count, l.ID 'НомерПартии'
            FROM [QA].[dbo].[Lot] l
            left join Product p on l.ProductID = p.ID
            left join WareHouse wh on l.WareHouseID = wh.ID
            left join Pharmacy ph on wh.PharmacyID = ph.ID ");
            if (result.Tables.Count == 0) return null;

            var list = new List<ListLot>();

            foreach (DataRow item in result.Tables[0].Rows)
                list.Add(new ListLot() { 
                    NameProduct = item[0].ToString()
                   ,NameWareHouse = item[1].ToString()
                   ,NamePharmacy = item[2].ToString()
                   ,Count = item[3].ToString()
                   ,ID = item[4].ToString() });

            return list;
        }

    }
}

