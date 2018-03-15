using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManager.Lib.ViewModels
{
    public partial class FolderItem
    {
        internal IEnumerable<RequestItem> GetRequests()
        {
            var folders = this.Items.OfType<FolderItem>().ToList();
            var firstLevelRequests = this.Items.OfType<RequestItem>().ToList();

            folders.ForEach(folder => firstLevelRequests.AddRange(folder.GetRequests()));

            return firstLevelRequests;
        }
    }
}
