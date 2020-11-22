using Base.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Model
{
    public class Entity
    {
        public Entity()
        {
            Id = GuidComb.GenerateComb();
        }
        public Guid Id { get; set; }
        public bool Delete { get; set; }
    }
}
