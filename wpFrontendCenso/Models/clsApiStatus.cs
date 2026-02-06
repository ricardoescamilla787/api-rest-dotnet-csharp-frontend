using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

using Newtonsoft.Json.Linq;

namespace wpFrontendCenso.Models
{
    public class clsApiStatus
    {
        public bool statusExec { get; set; }
        public string msg { get; set; }
        public int ban { get; set; }
        public JObject datos { get; set; }
    }
}
