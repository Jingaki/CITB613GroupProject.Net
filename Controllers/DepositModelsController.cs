using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProjectDepositCatalogWebApp.Data;
using GroupProjectDepositCatalogWebApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GroupProjectDepositCatalogWebApp.Controllers
{
    public class DepositModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepositModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DepositModels
        public async Task<IActionResult> Index()
        {
            //var users_table = _context.Users.ToList();

            IEnumerable<SearchResultEntity> data = await (
                        from users_table in _context.Users
                        join depostis_table in _context.Deposits on users_table.Id equals depostis_table.MyApplicationUserId
                        select new SearchResultEntity
                        {
                            Id = depostis_table.Id,
                            Username = users_table.UserName == null ? users_table.Email : users_table.UserName,//to register you must fill the email field username is normally set to be == to email
                            DepositName = depostis_table.DepositName,
                            TypeOfCurrency = depostis_table.CurrencyType,
                            EffectiveAnnualInterestRate = depostis_table.EffectiveAnnualInterestRate,
                            MinSum = depostis_table.MinSum,
                            TypeOfInterest = depostis_table.TypeOfInterest
                        }).ToListAsync();

            /*var applicationDbContext = _context.Deposits.Include(d => d.MyApplicationUser);
            return View(await applicationDbContext.ToListAsync());*/

            return View(data);
        }
        public IActionResult ShowSearchForm()
        {
            return _context.Deposits != null ?
                          View() :
                          Problem("Entity set 'ApplicationDbContext.Deposit'  is null.");
        }

        // GET: Deposits/ShowSearchResult
        [HttpPost]
        public async Task<IActionResult> ShowSearchResult(SearchFormEntity SearchRequest)
        {
            if (_context.Deposits == null || _context.Users == null )
            {
                return NotFound();
            }
            return _context.Deposits != null & _context.Users != null ?
                        View("Index", await (
                        from users_table in _context.Users
                        join depostis_table in _context.Deposits on users_table.Id equals depostis_table.MyApplicationUserId
                        where                         
                        SearchRequest.CurrencyType != 0 ? depostis_table.CurrencyType.Equals(SearchRequest.CurrencyType):1==1 &&
                        SearchRequest.OwnershipType != 0 ? depostis_table.OwnershipType.Equals(SearchRequest.OwnershipType) : 1 == 1 &&
                        SearchRequest.InterestPaymentType != 0 ? depostis_table.InterestPaymentType.Equals(SearchRequest.InterestPaymentType) : 1 == 1 &&
                        SearchRequest.TypeOfDeposit != 0 ? depostis_table.TypeOfDeposit.Equals(SearchRequest.TypeOfDeposit) : 1 == 1
                        where//after too many conditions in the where it stops working dotnet :^)
                        SearchRequest.TypeOfInterest != 0 ? depostis_table.TypeOfInterest.Equals(SearchRequest.TypeOfInterest) : 1 == 1 &&
                        !SearchRequest.AllTimeDeposits ? depostis_table.TimeDeposit==SearchRequest.TimeDeposit: 1==1 &&
                        !SearchRequest.AllOverdraftPossabilities ? depostis_table.OverdraftPossability==SearchRequest.OverdraftPossability : 1 == 1 &&
                        !SearchRequest.AllCreditPossabilities ? depostis_table.CreditPossability==SearchRequest.CreditPossability : 1 == 1
                        where
                        depostis_table.MinSum <= SearchRequest.InvestmentSum &&
                        depostis_table.MaxDuration >= SearchRequest.DurrationPlanned && depostis_table.MinDuration <= SearchRequest.DurrationPlanned
                        //SearchRequest.DurrationPlanned <= depostis_table.MaxDuration && SearchRequest.DurrationPlanned >= depostis_table.MinDuration &&
                        // how is that different then the one above dotnet you are crazy
                        select new SearchResultEntity
                        {
                            Id = depostis_table.Id,
                            Username = users_table.UserName,//== null ? users_table.Email : users_table.UserName,//to register you must fill the email field username is normally set to be == to email
                            DepositName = depostis_table.DepositName,
                            TypeOfCurrency = depostis_table.CurrencyType,
                            EffectiveAnnualInterestRate = depostis_table.EffectiveAnnualInterestRate,
                            MinSum = depostis_table.MinSum,
                            TypeOfInterest = depostis_table.TypeOfInterest
                        })
                        .ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Deposit'  is null.");
        }
        // GET: DepositModels/CleanUp/id for the deposit
        public async Task<IActionResult> CleanUp(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }
            var depositModel = _context.Deposits
                //.Include(d => d.MyApplicationUser)                
                .FirstOrDefaultAsync(d => d.Id == id);
            if (depositModel == null)
            {
                return NotFound();
            }

            return View(await depositModel);
        }

        


        // GET: DepositModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }
            var depositModel = _context.Deposits
                //.Include(d => d.MyApplicationUser)                
                .FirstOrDefaultAsync(d => d.Id == id);
            if (depositModel == null)
            {
                return NotFound();
            }

            return View(await depositModel);
        }

        // GET: DepositModels/Create
        public IActionResult Create()
        {
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: DepositModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DepositModel depositModel)
        {
            ModelState.Remove("MyApplicationUser");           
            if (ModelState.IsValid)
            {
                _context.Add(depositModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }

        // GET: DepositModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var depositModel = await _context.Deposits.FindAsync(id);
            if (depositModel == null)
            {
                return NotFound();
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }

        // POST: DepositModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepositName,TypeOfDeposit,TypeOfInterest,TimeDeposit,OverdraftPossability,CreditPossability,MonthlyCompounding,TerminalCapitalization,ValidForClientsOnly,CurrencyType,InterestPaymentType,OwnershipType,EffectiveAnnualInterestRate,WebLinkToOffer,DescriptionOfNegotiatedInterestRate,MinSum,MinSumDescription,MinDuration,MaxSum,MaxSumDescription,MaxDuration,DurationDescription,MyApplicationUserId")] DepositModel depositModel)
        {
            if (id != depositModel.Id)
            {
                return NotFound();
            }
            ModelState.Remove("MyApplicationUser");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(depositModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepositModelExists(depositModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }

        // GET: DepositModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var depositModel = await _context.Deposits
                //.Include(d => d.MyApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (depositModel == null)
            {
                return NotFound();
            }

            return View(depositModel);
        }

        // POST: DepositModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deposits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Deposits'  is null.");
            }
            var depositModel = await _context.Deposits.FindAsync(id);
            if (depositModel != null)
            {
                _context.Deposits.Remove(depositModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepositModelExists(int id)
        {
          return (_context.Deposits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
