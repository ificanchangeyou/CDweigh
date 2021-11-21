using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CDweigh.Class
{
    static class  Global
    {
        /// <summary>
        /// 链接SQL字符串
        /// </summary>
        public static string Sqldatabase;
        /// <summary>
        /// 当前用户名
        /// </summary>
        public static string Username;
        /// <summary>
        /// 当前用户权限
        /// </summary>
        public static int Rank;

        public static User users;



        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (row[p.Name] is Int64)
                    {
                        p.SetValue(entity, Convert.ToInt32(row[p.Name]), null);
                        continue;
                    }
                    p.SetValue(entity, row[p.Name], null);
                }
                list.Add(entity);
            }
            return list;

        }

    }

    class User
    {
        public string Username { get; set; } = "guest";
        public bool Change_password { get; set; } = false;
        public bool Delete_user { get; set; } = false;
        public bool Change_rank { get; set; } = false;
        public bool Scales_page { get; set; } = false;
        public bool Report_page { get; set; } = false;
        public bool DriverInfo_page { get; set; } = false;
        public bool VehicleInfo_page { get; set; } = false;
        public bool CustomerInfo_page { get; set; } = false;
        public bool MaterialsInfo_page { get; set; } = false;
        public bool OperationLog_page { get; set; } = false;
        public bool AlarmLog_page { get; set; } = false;
        public bool SystemSet_page { get; set; } = false;
        public bool DataAegis_page { get; set; } = false;
        public bool Camera_page { get; set; } = false;

    }


}
