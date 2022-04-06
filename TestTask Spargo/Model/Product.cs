using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestTask_QA.Model
{
    public class Product : Actions
    {

        public int Id { get; set; } 
        public string NameProduct { get; private set; }

        public override void Create()
        {
            if (!GetMessage()) return;

            var result = ConnectSQL.Connect.SelectString(@$"Use QA Insert into Product (NameProduct) values ('{NameProduct}')");

            if (result is null) return;

            Console.WriteLine($"Товар {NameProduct} успешно создан!");
        }

        public override void Remove()
        {
            var list = GetListProduct();
            if (list.Count == 0) { Console.WriteLine($"Товары не обнаружены! \n"); return; }

            Console.WriteLine("Список товаров:\n");
            list.ForEach(x => Console.WriteLine(x));
            Console.Write("Выберите товар, который хотите удалить:\n");

            var _product = Console.ReadLine();
            var _productid = GetIDProduct(_product);

            if (_productid == 0) { Console.WriteLine($"Товар с наименованием {_product} не найден. Попробуйте снова!\n"); Remove(); return; }

            ConnectSQL.Connect.SelectString($@"use qa delete [QA].[dbo].[Product] where id = {_productid}");            

            Console.WriteLine($"Товар {NameProduct} успешно Удалён!");
        }

        bool GetMessage()
        {
            Console.Write("Введите имя товара: ");
            var _product = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(_product)) { Console.WriteLine($"Имя товара не может быть пустым\n"); return GetMessage();  }

            if (!CheckProduct(_product)) { Console.WriteLine($"{_product} уже существует в БД\n");  return GetMessage(); }

            NameProduct = _product;
            return true;
        }

        bool CheckProduct(string _product)
        {
            var result = ConnectSQL.Connect.SelectString($@"Use QA Select NameProduct from Product where NameProduct = '{_product}'");
            if (result is null) return false;
            if (!string.IsNullOrEmpty(result.ToString())) return false;
            return true;
        }

       
    }
}
