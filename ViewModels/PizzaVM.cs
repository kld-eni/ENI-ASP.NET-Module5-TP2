using Module5_TP2.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Module5_TP2.ViewModels
{
    public class PizzaVM
    {
        public Pizza Pizza { get; set; }

        [Display(Name = "Pâte choisie")]
        public int SelectedPate { get; set; }

        //System.InvalidCastException: Impossible d'effectuer un cast d'un objet de type 'System.Collections.Generic.List`1[System.Int32]' en type 'System.Array'.
        //[MinLength(2, ErrorMessage = "Veuillez choisir au moins 2 ingrédients")]
        //[MaxLength(5, ErrorMessage = "Veuillez choisir moins de 6 ingrédients")]
        [Display(Name = "Ingredients choisis (entre 2 et 5)")]
        public List<int> SelectedIngredients { get; set; } = new List<int>();

        public List<SelectListItem> PatesListItems { get; set; } = FakeDB.PatesDisponibles.Select(p => new SelectListItem { Text = p.Nom, Value = p.Id.ToString() }).ToList();

        public List<SelectListItem> IngredientsListItems { get; set; } = FakeDB.IngredientsDisponibles.Select(i => new SelectListItem { Text = i.Nom, Value = i.Id.ToString() }).ToList();
    }
}