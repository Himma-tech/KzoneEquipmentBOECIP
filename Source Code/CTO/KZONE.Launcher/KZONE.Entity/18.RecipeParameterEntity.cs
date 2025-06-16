using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZONE.Entity
{
    public class RecipeParameter : Entity
    {
        public RecipeParameterEntityData Data { get; set; }

        public RecipeParameter(RecipeParameterEntityData data)
        {
            Data = data;
        }
    }
}
