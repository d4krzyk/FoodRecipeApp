﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace AppLibrary
{
    [ServiceContract]
    public interface IRecipeLibrary
    {
        [OperationContract]
        List<Recipe> FetchMealsData(string meal);

    }
}
