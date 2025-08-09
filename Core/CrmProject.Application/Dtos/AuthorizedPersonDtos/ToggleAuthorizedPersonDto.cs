using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Dtos.AuthorizedPersonDtos
{
    public class ToggleAuthorizedPersonDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
