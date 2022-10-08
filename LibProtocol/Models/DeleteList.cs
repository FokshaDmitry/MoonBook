using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProtocol.Models
{
    [Serializable]
    public class DeleteList
    {
        public Guid Id { get; set; }
        public Guid idUser { get; set; }
        public DateTime Date { get; set; }
        public Guid idElement { get; set; }
    }
}
