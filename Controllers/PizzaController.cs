using Module5_TP2.ViewModels;
using Module5_TP2.DAL;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Module5_TP2.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            List<Pizza> pizzas = FakeDB.Pizzas;
            return View(pizzas);
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            Pizza pizza = FakeDB.Pizzas.SingleOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                return RedirectToAction("Index");
            }
            return View(pizza);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            PizzaVM vm = new PizzaVM();
            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaVM pizzaVM)
        {
            try
            {
                ModelState.Remove("Pizza.Id");
                if (ModelState.IsValid)
                {
                    bool validatedPizza = ValidatePizza(ModelState, pizzaVM);
                    if (!validatedPizza)
                    {
                        return View(pizzaVM);
                    }

                    Pizza newPizza = new Pizza
                    {
                        Nom = pizzaVM.Pizza.Nom,
                        Ingredients = FakeDB.IngredientsDisponibles.Where(i => pizzaVM.SelectedIngredients.Contains(i.Id)).ToList(),
                        Pate = FakeDB.PatesDisponibles.SingleOrDefault(p => p.Id == pizzaVM.SelectedPate)
                    };
                    Pizza addedPizza = FakeDB.AddPizza(newPizza);
                    return RedirectToAction("Index");
                }
                return View(pizzaVM);
            }
            catch
            {
                return View(pizzaVM);
            }
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            Pizza selectedPizza = FakeDB.Pizzas.SingleOrDefault(p => p.Id == id);
            if (selectedPizza == null)
            {
                return RedirectToAction("Index");
            }
            PizzaVM vm = new PizzaVM {
                Pizza = selectedPizza,
                SelectedIngredients = selectedPizza.Ingredients.Select(i => i.Id).ToList(),
                SelectedPate = selectedPizza.Pate.Id
            };
            return View(vm);
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PizzaVM pizzaVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool validatedPizza = ValidatePizza(ModelState, pizzaVM, id);
                    if (!validatedPizza)
                    {
                        return View(pizzaVM);
                    }

                    Pizza pizza = FakeDB.Pizzas.SingleOrDefault(p => p.Id == id);
                    if (pizza == null)
                    {
                        return RedirectToAction("Index");
                    }

                    pizza.Nom = pizzaVM.Pizza.Nom;
                    pizza.Ingredients = FakeDB.IngredientsDisponibles.Where(i => pizzaVM.SelectedIngredients.Contains(i.Id)).ToList();
                    pizza.Pate = FakeDB.PatesDisponibles.SingleOrDefault(p => p.Id == pizzaVM.SelectedPate);

                    return RedirectToAction("Index");
                }
                return View(pizzaVM);
            }
            catch
            {
                return View(pizzaVM);
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            Pizza pizza = FakeDB.Pizzas.SingleOrDefault(p => p.Id == id);
            if (pizza == null)
            {
                return RedirectToAction("Index");
            }
            return View(pizza);
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDB.Pizzas.SingleOrDefault(p => p.Id == id);
                if (pizza == null)
                {
                    return RedirectToAction("Index");
                }
                FakeDB.Pizzas.Remove(pizza);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private bool ValidatePizza(ModelStateDictionary ms, PizzaVM pizzaVM, int id = 0)
        {
            #region Fields validation
            bool validatedName = pizzaVM.Pizza.Nom != null && pizzaVM.Pizza.Nom.Length >= 5 && pizzaVM.Pizza.Nom.Length <= 20;
            if (!validatedName)
            {
                ms.AddModelError("", "Nom obligatoire et devant contenir entre 5 et 20 charactères");
            }

            bool validatedPate = pizzaVM.SelectedPate > 0;
            if (!validatedPate)
            {
                ms.AddModelError("", "Pâte obligatoire");
            }

            bool validatedIngredients = pizzaVM.SelectedIngredients.Count >= 2 && pizzaVM.SelectedIngredients.Count <= 5;
            if (!validatedIngredients)
            {
                ms.AddModelError("", "Une pizza doit avoir entre 2 et 5 ingrédients");
            }

            if (!validatedName || !validatedPate || !validatedIngredients)
            {
                return false;
            }
            #endregion

            #region Server validation

            bool alreadyExistingName = FakeDB.Pizzas.Any(p => p.Nom.ToUpper() == pizzaVM.Pizza.Nom.ToUpper() && (id != 0 ? p.Id != id : true));
            if (alreadyExistingName)
            {
                ms.AddModelError("", "In existe déjà une pizza portant ce nom");
            }

            bool alreadyExistingComposition = FakeDB.Pizzas.Any(p => p.Ingredients.Count() == pizzaVM.SelectedIngredients.Count() && p.Ingredients.All(i => pizzaVM.SelectedIngredients.Contains(i.Id) && (id != 0 ? p.Id != id : true)));

            if (alreadyExistingComposition)
            {
                ms.AddModelError("", "Une pizza comporte déjà cette liste d'ingrédients");
            }

            return !(alreadyExistingName || alreadyExistingComposition);
            #endregion
        }
    }
}
