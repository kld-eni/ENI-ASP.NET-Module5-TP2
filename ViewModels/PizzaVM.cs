using Module5_TP2.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Module5_TP2.ViewModels
{
    public class PizzaVM
    {
        public Pizza Pizza { get; set; }

        public int SelectedPate { get; set; }

        public List<int> SelectedIngredients { get; set; } = new List<int>();

        public List<SelectListItem> PatesListItems { get; set; } = FakeDB.PatesDisponibles.Select(p => new SelectListItem { Text = p.Nom, Value = p.Id.ToString() }).ToList();

        public List<SelectListItem> IngredientsListItems { get; set; } = FakeDB.IngredientsDisponibles.Select(i => new SelectListItem { Text = i.Nom, Value = i.Id.ToString() }).ToList();
    }
}