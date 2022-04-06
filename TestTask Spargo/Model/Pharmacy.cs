using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_QA.Model
{
    public class Pharmacy : Actions
    {

        public int ID { get; set; }

        [MaxLength(50)]
        public string NamePharmacy { get; private set; }

        [MaxLength(100)]
        public string NameAdress { get; private set; }

        [MaxLength(12)]
        public string? Phone { get; set; }

        public List<string> GetList()
        {
            return GetPharmacyList();
        }

        public int GetID(string Name)
        { 
            return GetIDPharmacy(Name);
        }

        

        public override void Create()
        {
            if (!GetMessage()) return;

            //SQL запрос

            var result = ConnectSQL.Connect.SelectString($@"Use QA Insert into Pharmacy (NamePharmacy,NameAdress,Phone) values ('{NamePharmacy}','{NameAdress}','{Phone}')");
            if (result is null) return;

            Console.WriteLine($"Аптека с наименованием {NamePharmacy} и адрессом {NameAdress} успешно добавлена");

        }

        public override void Remove()
        {
            var list = GetPharmacyList();
            if (list.Count == 0) { Console.WriteLine($"Аптеки не обнаружены! \n"); return; }

            Console.WriteLine("Список аптек:\n");
            list.ForEach(c => Console.WriteLine(c));

            Console.Write("\nВведите имя аптеки которую необходимо удалить: ");
            var _namepharmacy = Console.ReadLine();

            var _id = GetIDPharmacy(_namepharmacy);

            if (_id == 0) { Console.WriteLine($"Аптека с наименованием {_namepharmacy} не найдена. Попробуйте снова!\n"); Remove(); return; }

            ConnectSQL.Connect.SelectString($@"use qa delete [QA].[dbo].[Pharmacy] where id = {_id}");
           

            Console.WriteLine($"Товар {_namepharmacy} успешно Удалён!");

        }

        private bool GetMessage()
        {
            string? _namepharmacy;
            string? _nameadress;
            string? _phone;

            if (!_setnamepharmacy()) return false;
            if (!_setnameadress()) return false;
            if (!_setphone()) return false;

            NamePharmacy = _namepharmacy;
            NameAdress = _nameadress;
            Phone = _phone;
            return true;
            
            //=============================================

            bool _setphone()
            {
                Console.Write("Введите телефон: ");
                _phone = Console.ReadLine();

                if (_phone.Length != 12) { Console.WriteLine($"Длина номера должен быть 12 символов. Попробуйте снова\n"); return _setphone(); }

                if (!_phone.StartsWith('+')) { Console.WriteLine($"Номер должен начинаться на +. Попробуйте снова\n"); return _setphone();  }

                foreach (var item in _phone.Remove(0).ToList())    
                    if (!byte.TryParse(item.ToString(),out byte  result)) 
                    { Console.WriteLine($"{item} не подходящий символ для телефонного формата. Попробуйте снова\n"); return _setphone(); }

               return true;
                
            }

            bool _setnameadress()
            {
                Console.Write("Введите адресс: ");
                _nameadress = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_nameadress)) { Console.WriteLine($"Пустой адресс не подходит. Попробуйте снова!\n"); return _setnameadress();}

                var result = CheckAdress(_nameadress);

                if (!string.IsNullOrWhiteSpace(result)) { Console.WriteLine($"{_nameadress} По такому адресу уже сщуествует аптека {result} \n"); return _setnameadress();  }
                return true;
            }

            bool _setnamepharmacy()
            {
                Console.Write("Введите имя новой аптеки: ");
                _namepharmacy = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_namepharmacy)) { Console.WriteLine($"Такое имя не подходит. Попробуйте снова!\n"); return _setnamepharmacy();  }

                if (!CheckNamePharmacy(_namepharmacy)) { Console.WriteLine($"{_namepharmacy} имя уже существует в БД\n"); return _setnamepharmacy();  }
                return true;
            }


        }

        private bool CheckNamePharmacy(string _namepharmacy)
        {
            var result = ConnectSQL.Connect.SelectString($@"use QA select NamePharmacy from Pharmacy where NamePharmacy = '{_namepharmacy}'");
            if (result is null) return false;
            if (!string.IsNullOrEmpty(result.ToString())) return false;
            return true;
        }

        private string CheckAdress(string _nameadress)
        {
            var result = ConnectSQL.Connect.SelectString($@"use QA select NamePharmacy from Pharmacy where NameAdress = '{_nameadress}'");
            if (result is null) return "";
            if (string.IsNullOrEmpty(result.ToString())) return "";
            return result.ToString();
        }






    }
}
