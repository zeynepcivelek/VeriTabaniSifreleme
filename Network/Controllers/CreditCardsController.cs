using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Network.Models;

namespace Network.Controllers
{
    public class CreditCardsController : Controller
    {
      
        AdventureWorks2014Entities db = new AdventureWorks2014Entities();
        List<CreditCard> OPECard = new List<CreditCard>();


        // GET: Home
        public ActionResult Index()
        {

            Stopwatch sw = Stopwatch.StartNew();
            List<CreditCard> cr = new List<CreditCard>();
            cr = db.CreditCards.ToList();
            sw.Stop();
            
            string start = sw.Elapsed.TotalSeconds.ToString();
            Session["Start"] = start;
            
          
            return View(cr);
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }
            return View(creditCard);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CreditCardID,CardType,CardNumber,ExpMonth,ExpYear,ModifiedDate")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                db.CreditCards.Add(creditCard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(creditCard);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }
            return View(creditCard);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CreditCardID,CardType,CardNumber,ExpMonth,ExpYear,ModifiedDate")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(creditCard);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }
            return View(creditCard);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditCard creditCard = db.CreditCards.Find(id);
            db.CreditCards.Remove(creditCard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult OPE()
        {
            Stopwatch sw = Stopwatch.StartNew();
            foreach (var item in db.CreditCards)
            {
                CreditCard newCard = new CreditCard();

                newCard.CardNumber = item.CardNumber;
                newCard.CardType = item.CardType;
                newCard.CreditCardID = item.CreditCardID;
                newCard.ExpMonth = item.ExpMonth;
                newCard.ExpYear = item.ExpYear;
                newCard.ModifiedDate = item.ModifiedDate;

                OPEFunction(newCard);

            }

            sw.Stop();

            string start = sw.Elapsed.TotalSeconds.ToString();
            Session["End"] = start;

            return View(OPECard);
        }

        //update edip bilgileri veritabanına eklicez
        public ActionResult Update()
        {
            foreach (var item in db.CreditCards)
            {

                db.CreditCards.Add(OPEFunction(item));

            }
            return View(db.CreditCards.ToList());
        }
        public CreditCard OPEFunction(CreditCard creditCard)
        {
            //m1<m2 iff c1<c2
            CreditCard newCard = new CreditCard();

            newCard.CardNumber = creditCard.CardNumber * 7 + 1;
            newCard.CardType = creditCard.CardType;
            newCard.CreditCardID = creditCard.CreditCardID;
            double ExpMonth = creditCard.ExpMonth;

            ExpMonth = ExpMonth % 12 + 1;
            //ExpMonth -= 1; 
            if (ExpMonth == 0)
            {
                ExpMonth = 12;
            }
            newCard.ExpMonth = ExpMonth;


            newCard.ExpYear = creditCard.ExpYear+5;
            newCard.ModifiedDate = creditCard.ModifiedDate;


            OPECard.Add(newCard);
            return newCard;
        }
        public ActionResult MIN()
        {
            Stopwatch sw = Stopwatch.StartNew();
          
            var rept_min = (from c in db.CreditCards
                            select c).Min(c => c.ExpYear);
            Session["MIN"] = rept_min.ToString();
            sw.Stop();

            string timeMin = sw.Elapsed.TotalSeconds.ToString();
            Session["timeMIN"] = timeMin;
            return View();
        }

        public ActionResult MAX()
        {
            Stopwatch sw = Stopwatch.StartNew();

            var rept_max = (from c in db.CreditCards
                            select c).Max(c => c.ExpMonth);
            Session["MAX"] = rept_max.ToString();
            sw.Stop();

            string timeMax = sw.Elapsed.TotalSeconds.ToString();
            Session["timeMAX"] = timeMax;
            return View();
        }
        public ActionResult COUNT()
        {
            Stopwatch sw = Stopwatch.StartNew();

            var count = (from o in db.CreditCards
                         select o).Count();
            
            sw.Stop();
            string timeCount = sw.Elapsed.TotalSeconds.ToString();
            Session["timeCOUNT"] = timeCount;
            return View();
        }
        public ActionResult REANGE(int start, int end)
        {

            Stopwatch sw = Stopwatch.StartNew();
            var count = (from o in db.CreditCards
                         where o.CreditCardID>start
                         select o).Count();

            sw.Stop();
            string timeRange = sw.Elapsed.TotalSeconds.ToString();
            Session["timeRENGE"] = timeRange;
            return View();
       
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
