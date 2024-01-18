using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MusicDLWin
{
    public class Admin
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Admin() { }

        /// <summary>
        /// プロパティ
        /// </summary>
        public bool GetAdmin {get;set;}

        /// <summary>
        /// 管理者権限かどうかの取得
        /// </summary>
        /// <returns></returns>
        public void IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            this.GetAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }


}
