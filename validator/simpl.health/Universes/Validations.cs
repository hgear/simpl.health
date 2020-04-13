using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    public class Validations : List<Validation>
    {
        public bool IsValid { get; set; }

        public Validations()
        {

        }
    }
}
