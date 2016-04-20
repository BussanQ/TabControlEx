using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;

namespace BQ.Access
{
    class Program
    {
        static void Main(string[] args)
        {
            string ss = "testsssf";
            DataSet s = DBAccess.Instance.Query("select * from TableTest");
            int i = DBAccess.Instance.ExecuteSql("insert into TableTest(TimeS,NameT) values ('2015-7-10','YY')");
        }
    }
}
