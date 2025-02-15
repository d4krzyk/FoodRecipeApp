﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MealsLibrary1
{
    [ServiceContract]
    public interface IRecipeLibrary
    {
        [OperationContract]
        List<Recipe> FetchMealsData(string meal);

    }
}
