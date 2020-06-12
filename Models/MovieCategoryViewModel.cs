using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieZone.Models{

    public class MovieCategoryViewModel{
        public List<Movie> Movies {get; set;}
        public SelectList Categories {get; set;}
        public string MovieCategory {get; set;}
        public string SearchString {get; set;}
    }
}