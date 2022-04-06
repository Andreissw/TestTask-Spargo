using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_QA.Model
{
    public abstract class Actions
    {
        public abstract void Create();
        public abstract void Remove();

        public List<string> GetListProduct()
        {
            var result = ConnectSQL.Connect.GetDatas($@"Use QA Select NameProduct from Product");
            if (result.Tables.Count == 0) return null;

            var list = new List<string>();

            foreach (DataRow item in result.Tables[0].Rows)
                list.Add(item[0].ToString());

            return list;
        }

         public int GetIDProduct(string Product)
        {
            return ConnectSQL.Connect.SelectStringInt($@"use QA select ID from Product where NameProduct = '{Product}'");
        }

        public List<string> GetPharmacyList()
        {
            var result = ConnectSQL.Connect.GetDatas($@"Use QA Select NamePharmacy from Pharmacy");
            if (result.Tables.Count == 0) return null;

            var list = new List<string>();

            foreach (DataRow item in result.Tables[0].Rows)
                list.Add(item[0].ToString());

            return list;

        }

        public int GetIDPharmacy(string Pharmacy)
        {
            return ConnectSQL.Connect.SelectStringInt($@"use QA select ID from Pharmacy where NamePharmacy = '{Pharmacy}'");
        }

        public int GetIDWareHouse(string WareHouse)
        {
            return ConnectSQL.Connect.SelectStringInt($@"use QA select ID from WareHouse where NameWareHouse = '{WareHouse}'");
        }

        public List<string> GetListWareHouse()
        {
            var result = ConnectSQL.Connect.GetDatas($@"Use QA Select NameWareHouse from WareHouse");
            if (result.Tables.Count == 0) return null;

            var list = new List<string>();

            foreach (DataRow item in result.Tables[0].Rows)
                list.Add(item[0].ToString());

            return list;
        }

    }
}
