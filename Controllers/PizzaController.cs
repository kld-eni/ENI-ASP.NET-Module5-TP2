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
                Pizza newPizza = new Pizza {
                    Nom = pizzaVM.Pizza.Nom,
                    Ingredients = FakeDB.IngredientsDisponibles.Where(i => pizzaVM.SelectedIngredients.Contains(i.Id)).ToList(),
                    Pate = FakeDB.PatesDisponibles.SingleOrDefault(p => p.Id == pizzaVM.SelectedPate)
                };
                Pizza addedPizza = FakeDB.AddPizza(newPizza);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
            catch
            {
                return View();
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
    }
}
