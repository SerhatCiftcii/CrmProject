using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Dtos.AuthorizedPersonDtos
{
    public class CreateAuthorizedPersonDto
    {
        public int CustomerId { get; set; } // Zorunlu
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Notes { get; set; }
    }
}
