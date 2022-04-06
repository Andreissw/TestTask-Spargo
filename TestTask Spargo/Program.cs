using System.Data;
using TestTask_QA.Model;
List<string> ListQuery = new List<string>() { "Create", "Delete" };
List<Actions> Listactions = new List<Actions>() { new Lot(), new Pharmacy(), new Product(), new WareHouse() };

while (true)
{
    var Result = Start();

    switch (Result)
    {
        case "1":
            GetInfo();
            break;

        case "2":
            StartProgramm();
            break;
    }
}


Console.ReadKey();

void GetInfo()
{ 
    Pharmacy pharmacy = new Pharmacy();
    Console.WriteLine("Выберите аптеку, чтобы выгрузить список товаров:\n");
    pharmacy.GetList().ForEach(p => Console.WriteLine(p));

    var _result = Console.ReadLine();
    var _id = pharmacy.GetID(_result);
    if (_id == 0 ) { Console.WriteLine($"{_result} Такой аптеки не существует. Попробуйте снова."); GetInfo(); return; }

    var _datas = TestTask_QA.ConnectSQL.Connect.GetDatas($@"SELECT (select pr.NameProduct from Product pr where pr.id = l.ProductID) Товар      
      ,[Count] Кол_во
      FROM [QA].[dbo].[Lot] l
      where WareHouseID in (select p.ID from Pharmacy p where p.ID = '{_id}')");

    Console.WriteLine(String.Format("{0,5}      |{1,5}", "Товар", "Кол-во"));
    SetBorder('_');

    int _count = 0;
    foreach (DataRow item in _datas.Tables[0].Rows)
    {
        Console.WriteLine(String.Format("{0,5}    |{1,5}", item[0], item[1]));
        _count += int.Parse(item[1].ToString());
    }

    SetBorder('_');

    Console.WriteLine($"Товаров всего = {_count}");
    return;

}

void StartProgramm()
{
    SetBorder('=');
    GetTables();
    SetBorder('-');
    var line = ChoiceTable();

    Actions? Table = GetAction(line);

    if (Table is null) { Console.WriteLine($"\nТаблицы {line} не существует. Попробуйте снова."); StartProgramm(); return;  }

    var Query = StartQuery();
    if (string.IsNullOrWhiteSpace(Query)) return;

    switch (Query)
    {
        case "Create":
            Table.Create();
            break;

        case "Delete":
            Table.Remove();
            break;           
    }

    //GetTable(Table);
}

string Start()
{
    Console.Write("Выберите ваши действия:\n");
    new List<string>() { "1. Выгрузить весь список товаров в Аптеке", "2. Создать Товар/Аптеку/Склад/Партию" }.ForEach(c=> Console.WriteLine(c));
    var result = Console.ReadLine();
    if (!(result == "1" || result == "2")) { Console.WriteLine("Можно выбрать только 1 или 2. Попробуйте Ещё раз") ; return Start(); }
    return result;
}

string StartQuery()
{
    var Query = ChoiceQuery();
    if (CheckQuery(Query) is null) { Console.WriteLine($"\nЗапроса {Query} не существует. Попробуйте снова."); return StartQuery(); }    
    return Query;
}

Actions? GetAction(string? Name)
{
    return Listactions.Where(c => c.GetType().Name == Name).FirstOrDefault();
}

string? CheckQuery(string Query)
{ 
    return ListQuery.Where(c => c == Query).FirstOrDefault();
}

void SetBorder(char ch)
{
    Console.WriteLine($"\n{new string(ch, 50)}");
}

void GetTables() 
{
    Console.WriteLine("\nСписок таблиц:\n");
    Listactions.ForEach(a => Console.WriteLine(a.GetType().Name));
}

string? ChoiceTable()
{
    Console.Write("\nВыберите таблицу: ");
    return Console.ReadLine();
}

string? ChoiceQuery()
{
    Console.Write("\nВыберите опреацию запроса:\n");
    ListQuery.ForEach(c => Console.WriteLine(c));

    return Console.ReadLine();
}


